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
    public partial class RecivableAccTypeMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IAccountTypeService accountTypeService = null;
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
            if (!IsPostBack)
            {
                BindRecAccTypeDetailsGrid();
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        private void BindRecAccTypeDetailsGrid()
        {
            List<RecievableAccTypeGridModel> recAccTypeGridModel;
            try
            {
                this.accountTypeService = new AccountTypeService();
                recAccTypeGridModel = accountTypeService.GetRecAccTypeGridData(SessionManager.UserId);
                if (accountTypeService != null && recAccTypeGridModel.Count > 0) { RecAccTypeGridView.DataSource = recAccTypeGridModel; }
                else
                {
                    RecAccTypeGridView.DataSource = new List<RecievableAccTypeGridModel>();
                }
                RecAccTypeGridView.DataBind();
            }
            finally { }
        }
        protected void RecAccTypeGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    RecievableAccTypeGridModel recievableAccType = e.Row.DataItem as RecievableAccTypeGridModel;
                    if (recievableAccType != null && recievableAccType.RecivableAccTypeId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = recievableAccType.RecivableAccTypeId.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnRecieveAccTypeRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void RecAccTypeGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in RecAccTypeGridView.Rows)
                {
                    if (row.RowIndex == RecAccTypeGridView.SelectedIndex)
                    {
                        row.CssClass = "selected_row";
                        selectedRowId = row.Attributes["data-row-id"];
                        editBtn.EnableViewState = !string.IsNullOrEmpty(selectedRowId) ? true : false;
                        SelectedRowIdHiddenField.Value = selectedRowId;
                    }
                    else
                    {
                        row.CssClass = "";
                    }
                }
            }
            finally { }
        }
        protected void SaveRecAccTypeDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            int flag;
            RecievableAccTypeModel model = new RecievableAccTypeModel();
            try
            {
                model = GetRecAccType();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true);
                    return;
                }
                this.accountTypeService = new AccountTypeService();
                if (model.RecivableAccTypeId == 0)
                {
                    flag = accountTypeService.AddRecAccType(model);
                }
                else
                {
                    flag = accountTypeService.EditRecAccType(model);
                }
                if (flag > 0)
                {
                    BindRecAccTypeDetailsGrid();
                    popup_container.Visible = false;
                }
                else
                {
                    if (flag == -2) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Name is Already Exist.');", true); return; }
                    else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true); return; }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "RECIEVABLE ACCOUNT TYPE MASTER", "RecievableAccTypeMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.accountTypeService = null; this.__logFileService = null; }
        }
        private RecievableAccTypeModel GetRecAccType()
        {
            RecievableAccTypeModel model;
            int recAccTypeValue;
            try
            {
                model = new RecievableAccTypeModel();
                model.RecivableAccTypeId = int.TryParse(RecivableAccTypeId.Value, out recAccTypeValue) ? recAccTypeValue : 0;
                model.RecivableAccTypeName = RecAccTypeName.Text.Trim();
                model.RecivableAccTypeDescription = RecAccTypeDescription.Text.Trim();
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(RecievableAccTypeModel model)
        {
            if (string.IsNullOrEmpty(model.RecivableAccTypeName)) { return "Please fill Recievable Account Type Name."; }
            if (string.IsNullOrEmpty(model.RecivableAccTypeDescription)) { return "Please fill Recievable Account Type Description."; }
            return "";
        }
        protected void AddRecAccType_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add Recievable AccountType";
            RecivableAccTypeId.Value = "0";
            RecAccTypeName.Text = "";
            RecAccTypeDescription.Text = "";
            popup_container.Visible = true;
        }
        protected void EditRecAccType_Click(object sender, EventArgs e)
        {
            RecievableAccTypeModel recievableAccType;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                PopupHeaderText.InnerText = "Edit Recievable AccountType";
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    recievableAccType = new RecievableAccTypeModel();
                    this.accountTypeService = new AccountTypeService();
                    recievableAccType = accountTypeService.FetchRecAccType(selectedRowId);
                    RecivableAccTypeId.Value = recievableAccType.RecivableAccTypeId.ToString();
                    RecAccTypeName.Text = recievableAccType.RecivableAccTypeName;
                    RecAccTypeDescription.Text = recievableAccType.RecivableAccTypeDescription;
                    popup_container.Visible = true;
                    SelectedRowIdHiddenField.Value = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Edit.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "RECIEVABLE ACCOUNT TYPE MASTER", "RecievableAccTypeMaster.aspx.cs", ex, "");
            }
        }
        protected void DeleteRecAccType_Click(object sender, EventArgs e)
        {
            selectedRowId = SelectedRowIdHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedRowId))
            {
                popup_confirm_container.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Delete.');", true);
            }
        }
        protected void ConfirmDeleteRecAccType_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.accountTypeService = new AccountTypeService();
                flag = accountTypeService.DeleteRecAccType(selectedRowId);
                if (flag > 0)
                {
                    BindRecAccTypeDetailsGrid();
                    popup_confirm_container.Visible = false;
                    SelectedRowIdHiddenField.Value = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again.');", true);
                    return;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Deleted Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "RECIEVABLE ACCOUNT TYPE MASTER", "RecievableAccTypeMaster.aspx.cs", ex, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
    }
}