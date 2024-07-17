using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class MenuAccessDetailsMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IModuleService moduleService = null;
        private ISharedService sharedService = null;
        private ILogFileService logFileService
        {
            get
            {
                if (__logFileService == null) { __logFileService = new LogFileService(); }
                return __logFileService;
            }
        }
        private ILogFileService __logFileService = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateAdminDropDownList();
                PopulateModuleDropDownList();
                BindMenuAccessGrid(0);
                popup_container.Visible = false;
            }
        }
        private void PopulateAdminDropDownList()
        {
            List<DropDownItem> adminList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                adminList = sharedService.GetAdminDropDownData();
                adminList.Insert(0, new DropDownItem { Key = 0, Value = "-Select AdminName-" });
                AdminDropDownList.DataSource = adminList;
                AdminDropDownList.DataValueField = "Key";
                AdminDropDownList.DataTextField = "Value";
                AdminDropDownList.DataBind();
                AdminNameDropDownList.DataSource = adminList;
                AdminNameDropDownList.DataValueField = "Key";
                AdminNameDropDownList.DataTextField = "Value";
                AdminNameDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(adminList);
                this.logFileService.LogError(SessionManager.UserId, "MENU ACCESS DETAILS MASTER", "MenuAccessDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { adminList = null; }
        }
        protected void AdminDropDownOnChange(object sender, EventArgs e)
        {
            int selIndexValue = int.Parse(AdminNameDropDownList.SelectedValue);
            BindMenuAccessGrid(selIndexValue);
        }
        private void PopulateModuleDropDownList()
        {
            List<DropDownItem> menuItemList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.moduleService = new ModuleService();
                menuItemList = moduleService.GetModuleDrpoDownData();
                menuItemList.Insert(0, new DropDownItem { Key = 0, Value = "-Select MenuItem-" });
                ModuleNameDropDownList.DataSource = menuItemList;
                ModuleNameDropDownList.DataValueField = "Key";
                ModuleNameDropDownList.DataTextField = "Value";
                ModuleNameDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(menuItemList);
                this.logFileService.LogError(SessionManager.UserId, "MENU ACCESS DETAILS MASTER", "MenuAccessDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { menuItemList = null; }
        }
        private void BindMenuAccessGrid(int selIndexValue)
        {
            List<MenuAccessDetailsGridModel> menuAccessGridData;
            try
            {
                moduleService = new ModuleService();
                menuAccessGridData = moduleService.GetMenuAccessGridData(selIndexValue);
                if (menuAccessGridData != null && menuAccessGridData.Count > 0) { MenuAccessGridView.DataSource = menuAccessGridData; }
                else
                {
                    MenuAccessGridView.DataSource = new List<MenuAccessDetailsGridModel>();
                }
                MenuAccessGridView.DataBind();
            }
            finally { }
        }
        protected void SaveMenuAccessDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            int flag;
            int selIndexValue;
            MenuAccessDetailsModel model = new MenuAccessDetailsModel();
            try
            {
                model = GetMenuItemDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg)) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true); return; }
                moduleService = new ModuleService();
                flag = moduleService.SaveMenuAccess(model);
                if (flag > 0)
                {
                    selIndexValue = int.Parse(AdminNameDropDownList.SelectedValue);
                    BindMenuAccessGrid(selIndexValue);
                    popup_container.Visible = false;
                }
                else
                {
                    if (flag == -2) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('MenuItem Name is Already Exist for this User.');", true); return; }
                    else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Application.Please Contact Admin');", true); return; }
                }
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "MENU ACCESS DETAILS MASTER", "MenuAccessDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.moduleService = null; this.__logFileService = null; }
        }
        private MenuAccessDetailsModel GetMenuItemDetails()
        {
            MenuAccessDetailsModel model;
            int menuItemIdValue;
            try
            {
                model = new MenuAccessDetailsModel();
                model.MenuAccessId = int.TryParse(MenuAccessId.Value, out menuItemIdValue) ? menuItemIdValue : 0;
                model.AdminId = int.Parse(AdminDropDownList.SelectedValue);
                model.ModuleId = int.Parse(ModuleNameDropDownList.SelectedValue);
                model.CreatedBy = SessionManager.UserId;
                model.SelectedMenuItems = new List<string>();
                foreach (ListItem item in chkBoxList.Items)
                {
                    if (item.Selected)
                    {
                        model.SelectedMenuItems.Add(item.Value);
                    }
                }
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(MenuAccessDetailsModel model)
        {
            if (model.ModuleId <= 0) { return "Please Select Module Name."; }
            if (model.AdminId <= 0) { return "Please Select Admin Name."; }
            //if (model.SelectedMenuItems == null || model.SelectedMenuItems.Count == 0) { return "Please Check Atleast One MenuItem Name."; }
            return "";
        }
        protected void AddEditMenuAccess_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Enable Menu Access";
            MenuAccessId.Value = "0";
            AdminDropDownList.SelectedValue = "0";
            ModuleNameDropDownList.SelectedValue = "0";
            chkBoxList.Items.Clear();
            popup_container.Visible = true;
        }
        protected void AdminDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModuleNameDropDownList.SelectedValue = "0";
            chkBoxList.Items.Clear();
        }
        protected void ModuleDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedModuleId, selectedUserId;
            List<MenuAccessDetailsModel> selectedChkBoxItemList;
            try
            {
                selectedUserId = AdminDropDownList.SelectedValue;
                selectedModuleId = ModuleNameDropDownList.SelectedValue;
                selectedChkBoxItemList = new List<MenuAccessDetailsModel>();
                this.moduleService = new ModuleService();
                selectedChkBoxItemList = moduleService.FetchMenuAccessData(selectedUserId, selectedModuleId);
                List<DropDownItem> chkBoxItemList = LoadMenuItemsChkBox(selectedModuleId);
                chkBoxList.Items.Clear();
                foreach (var item in chkBoxItemList)
                {
                    ListItem listItem = new ListItem(item.Value, item.Key.ToString());
                    if(selectedChkBoxItemList != null && selectedChkBoxItemList.Count > 0)
                    {
                        listItem.Selected = selectedChkBoxItemList.Any(x => x.MenuItemId == item.Key);
                    }
                    chkBoxList.Items.Add(listItem);
                }
            }
            finally { selectedModuleId = null; selectedUserId = null; selectedChkBoxItemList = null; moduleService = null; }
        }
        private List<DropDownItem> LoadMenuItemsChkBox(string selectedModuleId)
        {
            List<DropDownItem> menuItemList = new List<DropDownItem>();
            this.moduleService = new ModuleService();
            menuItemList = moduleService.GetMenuItemChkBoxData(selectedModuleId);
            return menuItemList;
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
        }
    }
}