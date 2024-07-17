using System;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Linq;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class WorkPlanMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IWorkPlanService workPlanService = null;
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
        private string selectedRowId = null;
        #endregion
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProjectDropDownList();
                PopulateTeamSupplyDropDownList();
                BindWorkPlanGrid();
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        #endregion
        #region Populate DropDown List
        private void PopulateProjectDropDownList()
        {
            List<DropDownItem> projectList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                projectList = sharedService.GetProjectDropDownData();
                projectList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Project-" });
                ProjectNameDropDownList.DataSource = projectList;
                ProjectNameDropDownList.DataValueField = "Key";
                ProjectNameDropDownList.DataTextField = "Value";
                ProjectNameDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(projectList);
                this.logFileService.LogError(SessionManager.UserId, "WORKPLAN MASTER", "WorkPlanMaster.aspx.cs", ex, modelJson);
            }
            finally { projectList = null; }
        }
        private void PopulateTeamSupplyDropDownList()
        {
            List<DropDownItem> teamSupplyList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.workPlanService = new WorkPlanService();
                teamSupplyList = workPlanService.GetTeamSupplyDropDownData();
                teamSupplyList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Team-" });
                TeamHeadNameDropDownList.DataSource = teamSupplyList;
                TeamHeadNameDropDownList.DataValueField = "Key";
                TeamHeadNameDropDownList.DataTextField = "Value";
                TeamHeadNameDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(teamSupplyList);
                this.logFileService.LogError(SessionManager.UserId, "WORKPLAN MASTER", "WorkPlanMaster.aspx.cs", ex, modelJson);
            }
            finally { teamSupplyList = null; }
        }
        #endregion
        #region Load Grid
        private void BindWorkPlanGrid()
        {
            List<WorkPlanMasterGridModel> workPlanGridData;
            try
            {
                this.workPlanService = new WorkPlanService();
                workPlanGridData = workPlanService.GetWorkPlanGridData();
                if (workPlanGridData != null && workPlanGridData.Count > 0) { WorkPlanGridView.DataSource = workPlanGridData; }
                else { WorkPlanGridView.DataSource = new List<WorkPlanMasterGridModel>(); }
                WorkPlanGridView.DataBind();
            }
            finally { workPlanGridData = null; }
        }
        protected void WorkPlanGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    WorkPlanMasterGridModel workPlanMasterDetail = e.Row.DataItem as WorkPlanMasterGridModel;
                    if (workPlanMasterDetail != null && workPlanMasterDetail.Work_Plan_Id > 0)
                    {
                        e.Row.Attributes["data-row-id"] = workPlanMasterDetail.Work_Plan_Id.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnWorkPlanMasterDetailRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void WorkPlanGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in WorkPlanGridView.Rows)
                {
                    if (row.RowIndex == WorkPlanGridView.SelectedIndex)
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
        #endregion
        #region Add,Edit and Delete Team Supply
        protected void AddWorkPlan_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add Team Supply";
            Work_Plan_Id.Value = "0";
            ProjectNameDropDownList.SelectedValue = "0";
            PlannedDate.Text = "";
            WorkDescription.Text = "";
            TeamHeadNameDropDownList.SelectedValue = "0";
            chkBoxList.Items.Clear();
            WorkersCount.Text = "";
            popup_container.Visible = true;
        }
        protected void TeamDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<WorkPlanMasterModel> selectedChkBoxItemList;
            string selectedTeamId;
            try
            {
                selectedTeamId = TeamHeadNameDropDownList.SelectedValue;
                if (selectedTeamId == "1")
                {
                    Workers_chkBox_Container.Visible = true;
                    selectedChkBoxItemList = new List<WorkPlanMasterModel>();
                    this.workPlanService = new WorkPlanService();
                    //selectedChkBoxItemList = workPlanService.FetchWorkersList();
                    //List<DropDownItem> chkBoxItemList = LoadWorkersChkBox(selectedModuleId);
                    //chkBoxList.Items.Clear();
                    //foreach (var item in chkBoxItemList)
                    //{
                    //    ListItem listItem = new ListItem(item.Value, item.Key.ToString());
                    //    if (selectedChkBoxItemList != null && selectedChkBoxItemList.Count > 0)
                    //    {
                    //        listItem.Selected = selectedChkBoxItemList.Any(x => x.MenuItemId == item.Key);
                    //    }
                    //    chkBoxList.Items.Add(listItem);
                    //}
                }
                else if (selectedTeamId == "2") {
                    Workers_chkBox_Container.Visible = false;
                }
                else { }
            }
            finally { selectedTeamId = null; selectedChkBoxItemList = null; workPlanService = null; }
        }
        protected void EditWorkPlan_Click(object sender, EventArgs e)
        {
            WorkPlanMasterModel selWorkPlanRowData = null;
            string modelJson = "";
            try
            {
                PopupHeaderText.InnerText = "Edit Team Supply";
                selectedRowId = SelectedRowIdHiddenField.Value;
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    selWorkPlanRowData = new WorkPlanMasterModel();
                    this.workPlanService = new WorkPlanService();
                    selWorkPlanRowData = workPlanService.FetchWorkPlanData(selectedRowId);
                    Work_Plan_Id.Value = selWorkPlanRowData.WorkPlanId.ToString();
                    ProjectNameDropDownList.SelectedValue = selWorkPlanRowData.ProjectId.ToString();
                    PlannedDate.Text = selWorkPlanRowData.PlannedDate.ToString("yyyy-MM-dd");
                    WorkDescription.Text = selWorkPlanRowData.WorkDescription.ToString();
                    TeamHeadNameDropDownList.SelectedValue = selWorkPlanRowData.Team_Supply_Id.ToString();
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
                modelJson = JsonConvert.SerializeObject(selWorkPlanRowData);
                this.logFileService.LogError(SessionManager.UserId, "WORKPLAN MASTER", "WorkPlanMaster.aspx.cs", ex, modelJson);
            }
            finally { selWorkPlanRowData = null; }
        }
        protected void DeleteWorkPlan_Click(object sender, EventArgs e)
        {
            selectedRowId = SelectedRowIdHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedRowId)) { popup_confirm_container.Visible = true; }
            else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Delete.');", true); return; }
        }
        protected void ConfirmDeleteWorkPlan_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.workPlanService = new WorkPlanService();
                flag = workPlanService.DeleteWorkPlanDetail(selectedRowId);
                if (flag > 0)
                {
                    BindWorkPlanGrid();
                    popup_confirm_container.Visible = false;
                    SelectedRowIdHiddenField.Value = "";
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again.');", true); return; }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Deleted Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "WORKPLAN MASTER", "WorkPlanMaster.aspx.cs", ex, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
        #endregion
        #region Save Team Supply
        protected void SaveWorkPlanDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            long flag;
            WorkPlanMasterModel model = new WorkPlanMasterModel();
            try
            {
                model = GetWorkPlanDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg)) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true); return; }
                this.workPlanService = new WorkPlanService();
                if (model.WorkPlanId == 0) { flag = workPlanService.AddWorkPlanDetail(model); }
                else { flag = workPlanService.EditWorkPlanDetail(model); }
                if (flag > 0)
                {
                    BindWorkPlanGrid();
                    popup_container.Visible = false;
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true); return; }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "WORKPLAN MASTER", "WorkPlanMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.workPlanService = null; this.__logFileService = null; }
        }
        private WorkPlanMasterModel GetWorkPlanDetails()
        {
            WorkPlanMasterModel model;
            long workPalnIdValue;
            DateTime plannedDate;
            try
            {
                model = new WorkPlanMasterModel();
                model.WorkPlanId = long.TryParse(Work_Plan_Id.Value, out workPalnIdValue) ? workPalnIdValue : 0;
                model.ProjectId = short.Parse(ProjectNameDropDownList.SelectedValue);
                model.PlannedDate = DateTime.TryParse(PlannedDate.Text, out plannedDate) ? plannedDate.Date : DateTime.Now.Date;
                model.WorkDescription = WorkDescription.Text.Trim();
                model.Team_Supply_Id = int.Parse(TeamHeadNameDropDownList.SelectedValue);
                model.SelectedWorkers = new List<string>();
                foreach (ListItem item in chkBoxList.Items)
                {
                    if (item.Selected) { model.SelectedWorkers.Add(item.Value); }
                }
                model.CreatedBy = SessionManager.UserId;
                model.WorkersCount = int.Parse(WorkersCount.Text);
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(WorkPlanMasterModel model)
        {
            if (model.ProjectId <= 0) { return "Please Select Project Name."; }
            if (model.PlannedDate == null) { return "Please Select Planned Date."; }
            if (string.IsNullOrWhiteSpace(model.WorkDescription)) { return "Please fill Description/Location."; }
            if (model.Team_Supply_Id <= 0) { return "Please Select Team Head Name."; }
            return "";
        }
        #endregion
    }
}