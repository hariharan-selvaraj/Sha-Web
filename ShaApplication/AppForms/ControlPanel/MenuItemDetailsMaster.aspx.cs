using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class MenuItemDetailsMaster : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                PopulateModuleDropDownList();
                BindMenuItemGrid();
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        private void PopulateModuleDropDownList()
        {
            List<DropDownItem> moduleList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.moduleService = new ModuleService();
                moduleList = moduleService.GetModuleDrpoDownData();
                moduleList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Module-" });
                ModuleNameDropDownList.DataSource = moduleList;
                ModuleNameDropDownList.DataValueField = "Key";
                ModuleNameDropDownList.DataTextField = "Value";
                ModuleNameDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(moduleList);
                this.logFileService.LogError(SessionManager.UserId, "MENU ITEM MASTER", "MenuItemDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { moduleList = null; }
        }
        private void BindMenuItemGrid()
        {
            List<MenuItemDetailsGridModel> menuItemGridData;
            try
            {
                moduleService = new ModuleService();
                menuItemGridData = moduleService.GetMenuItemGridData();
                if (menuItemGridData != null && menuItemGridData.Count > 0) { MenuItemGridView.DataSource = menuItemGridData; }
                else
                {
                    MenuItemGridView.DataSource = new List<MenuItemDetailsGridModel>();
                }
                MenuItemGridView.DataBind();
            }
            finally { }
        }
        protected void MenuItemGridGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    MenuItemDetailsGridModel menuItemDetail = e.Row.DataItem as MenuItemDetailsGridModel;
                    if (menuItemDetail != null && menuItemDetail.MenuItemId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = menuItemDetail.MenuItemId.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnMenuItemDetailRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void MenuItemGridGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in MenuItemGridView.Rows)
                {
                    if (row.RowIndex == MenuItemGridView.SelectedIndex)
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
        protected void SaveMenuItemDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            int flag;
            MenuItemDetailsModel model = new MenuItemDetailsModel();
            try
            {
                model = GetMenuItemDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg)) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true); return; }
                moduleService = new ModuleService();
                if (model.MenuItemId == 0) { flag = moduleService.AddMenuItem(model); }
                else { flag = moduleService.EditMenuItem(model); }
                if (flag > 0)
                {
                    BindMenuItemGrid();
                    popup_container.Visible = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true);
                    return;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "MENU ITEM MASTER", "MenuItemDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.moduleService = null; this.__logFileService = null; }
        }
        private MenuItemDetailsModel GetMenuItemDetails()
        {
            MenuItemDetailsModel model;
            int menuItemIdValue;
            try
            {
                model = new MenuItemDetailsModel();
                model.MenuItemId = int.TryParse(MenuItemId.Value, out menuItemIdValue) ? menuItemIdValue : 0;
                model.ModuleMasterId = int.Parse(ModuleNameDropDownList.SelectedValue);
                model.MenuItemName = MenuItemName.Text.Trim();
                model.MenuItemDescription = MenuItemDescription.Text.Trim();
                model.TaskURL = TaskURL.Text.Trim();
                model.MenuItemIcon = MenuItemIcon.Text.Trim();
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(MenuItemDetailsModel model)
        {
            moduleService = new ModuleService();
            if (model.ModuleMasterId <= 0) { return "Please Select Module Name.";  }
            if (string.IsNullOrEmpty(model.MenuItemName)) { return "Please fill MenuTtem Name."; }
            if (string.IsNullOrEmpty(model.MenuItemDescription)) { return "Please fill Description."; }
            if (string.IsNullOrEmpty(model.TaskURL)) { return "Please fill URL."; }
            if (this.moduleService.HasDuplicateUrl(model.MenuItemId, model.TaskURL)) { return "URL is Already Exist."; }
            if (string.IsNullOrEmpty(model.MenuItemIcon)) { return "Please fill URL."; }
            return "";
        }
        protected void AddMenuItem_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add MenuItem";
            MenuItemId.Value = "0";
            ModuleNameDropDownList.SelectedValue = "0";
            MenuItemName.Text = "";
            MenuItemDescription.Text = "";
            TaskURL.Text = "";
            MenuItemIcon.Text = "";
            popup_container.Visible = true;
        }
        protected void EditMenuItem_Click(object sender, EventArgs e)
        {
            MenuItemDetailsModel selMenuItemDetailsRowData;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                PopupHeaderText.InnerText = "Edit MenuItem";
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    this.moduleService = new ModuleService();
                    selMenuItemDetailsRowData = moduleService.FetchMenuItemData(selectedRowId);
                    MenuItemId.Value = selMenuItemDetailsRowData.MenuItemId.ToString();
                    ModuleNameDropDownList.SelectedValue = selMenuItemDetailsRowData.ModuleMasterId.ToString();
                    MenuItemName.Text = selMenuItemDetailsRowData.MenuItemName;
                    MenuItemDescription.Text = selMenuItemDetailsRowData.MenuItemDescription;
                    TaskURL.Text = selMenuItemDetailsRowData.TaskURL;
                    MenuItemIcon.Text = selMenuItemDetailsRowData.MenuItemIcon;
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
                this.logFileService.LogError(SessionManager.UserId, "MENU ITEM MASTER", "MenuItemDetailsMaster.aspx.cs", ex, "");
            }
            finally { selMenuItemDetailsRowData = null; }
        }
        protected void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            selectedRowId = SelectedRowIdHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedRowId))
            {
                popup_confirm_container.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Delete.');", true);
                return;
            }
        }
        protected void ConfirmDeleteMenuItem_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.moduleService = new ModuleService();
                flag = moduleService.DeleteMenuItemDetail(selectedRowId);
                if (flag > 0)
                {
                    BindMenuItemGrid();
                    popup_confirm_container.Visible = false;
                    SelectedRowIdHiddenField.Value = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again');", true);
                    return;
                }
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