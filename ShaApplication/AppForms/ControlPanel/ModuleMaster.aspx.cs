using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class ModuleMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IModuleService moduleService = null;
        private ILogFileService logFileService
        {
            get
            {
                if (__logFileService == null) { __logFileService = new LogFileService(); }
                return __logFileService;
            }
        }
        private ILogFileService __logFileService = null;
        private string selectedRowId = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            BindModuleMasterGrid();
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
        private void BindModuleMasterGrid()
        {
            List<ModuleMasterGridModel> moduleMasterGridData;
            try
            {
                moduleService = new ModuleService();
                moduleMasterGridData = moduleService.GetModuleMasterGridData();
                if (moduleMasterGridData != null && moduleMasterGridData.Count > 0) { ModuleMasterGridView.DataSource = moduleMasterGridData; }
                else { ModuleMasterGridView.DataSource = new List<ModuleMasterGridModel>(); }
                ModuleMasterGridView.DataBind();
            }
            finally { moduleMasterGridData = null; moduleService = null; }
        }
        protected void ModuleMasterGridGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            ModuleMasterGridModel moduleMasterDetail;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    moduleMasterDetail = e.Row.DataItem as ModuleMasterGridModel;
                    if (moduleMasterDetail != null && moduleMasterDetail.ModuleId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = moduleMasterDetail.ModuleId.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnModuleMasterRowClick(this);";
                    }
                }
            }
            finally { moduleMasterDetail = null; }
        }
        protected void ModuleMasterGridGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in ModuleMasterGridView.Rows)
                {
                    if (row.RowIndex == ModuleMasterGridView.SelectedIndex)
                    {
                        row.CssClass = "selected_row";
                        selectedRowId = row.Attributes["data-row-id"];
                        editBtn.EnableViewState = !string.IsNullOrEmpty(selectedRowId) ? true : false;
                        SelectedRowIdHiddenField.Value = selectedRowId;
                    }
                    else { row.CssClass = ""; }
                }
            }
            finally { }
        }
        protected void SaveModuleMasterDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            int flag;
            ModuleMasterGridModel model = new ModuleMasterGridModel();
            try
            {
                model = GetModuleMasterDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg)) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert(msg);", true); return; }
                moduleService = new ModuleService();
                if (model.ModuleId == 0) { flag = moduleService.AddModuleDetail(model); }
                else { flag = moduleService.EditModuleDetail(model); }
                if (flag > 0) { BindModuleMasterGrid(); popup_container.Visible = false; }
                else
                {
                    if (flag == -2) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Module Name is Already Exist.');", true); return; }
                    else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true); return; }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "MODULE MASTER", "ModuleMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.moduleService = null; this.__logFileService = null; }
        }
        private ModuleMasterGridModel GetModuleMasterDetails()
        {
            ModuleMasterGridModel model;
            int moduleIdValue;
            try
            {
                model = new ModuleMasterGridModel();
                model.ModuleId = int.TryParse(ModuleMasterId.Value, out moduleIdValue) ? moduleIdValue : 0;
                model.ModuleName = ModuleName.Text.Trim();
                model.ModuleIcon = ModuleIcon.Text.Trim();
                model.ModuleDescription = ModuleMasterDescription.Text.Trim();
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(ModuleMasterGridModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ModuleName)) { return "Please fill Module Name."; }
            if (string.IsNullOrWhiteSpace(model.ModuleIcon)) { return "Please fill Module Icon."; }
            if (string.IsNullOrWhiteSpace(model.ModuleDescription)) { return "Please fill Description."; }
            return "";
        }
        protected void AddModule_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add Module";
            ModuleMasterId.Value = "0";
            ModuleName.Text = "";
            ModuleIcon.Text = "";
            ModuleMasterDescription.Text = "";
            popup_container.Visible = true;
        }
        protected void EditModule_Click(object sender, EventArgs e)
        {
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                PopupHeaderText.InnerText = "Edit Module";
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    this.moduleService = new ModuleService();
                    ModuleMasterGridModel selModuleDetailsRowData = moduleService.FetchModuleDetails(selectedRowId);
                    ModuleMasterId.Value = selModuleDetailsRowData.ModuleId.ToString();
                    ModuleName.Text = selModuleDetailsRowData.ModuleName;
                    ModuleIcon.Text = selModuleDetailsRowData.ModuleIcon;
                    ModuleMasterDescription.Text = selModuleDetailsRowData.ModuleDescription;
                    popup_container.Visible = true;
                    SelectedRowIdHiddenField.Value = "";
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Edit.');", true); return; }
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "MODULE MASTER", "ModuleMaster.aspx.cs", ex, "");
            }
        }
        protected void DeleteModule_Click(object sender, EventArgs e)
        {
            selectedRowId = SelectedRowIdHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedRowId)) { popup_confirm_container.Visible = true; }
            else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Delete.');", true); }
        }
        protected void ConfirmDeleteModule_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.moduleService = new ModuleService();
                flag = moduleService.DeleteModuleDetail(selectedRowId);
                if (flag > 0)
                {
                    BindModuleMasterGrid();
                    popup_confirm_container.Visible = false;
                    SelectedRowIdHiddenField.Value = "";
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again.');", true);return; }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Deleted Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "MODULE MASTER", "ModuleMaster.aspx.cs", ex, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
    }
}