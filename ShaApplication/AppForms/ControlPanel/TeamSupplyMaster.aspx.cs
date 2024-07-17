using System;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Reflection;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class TeamSupplyMaster : System.Web.UI.Page
    {
        #region Common Variables
        private ITeamSupplyService teamSupplyService = null;
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
                PopulateTeamDropDownList();
                BindTeamSupplyGrid();
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        #endregion
        #region Populate DropDown List
        private void PopulateTeamDropDownList()
        {
            List<DropDownItem> teamList= new List<DropDownItem>();
            string modelJson = "";
            try
            {
                foreach (var teamName in Enum.GetValues(typeof(TeamName)))
                {
                    teamList.Add(new DropDownItem
                    {
                        Key = (int)teamName,
                        Value = teamName.ToString()
                    });
                }
                teamList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Team-" });
                TeamNameDropDownList.DataSource = teamList;
                TeamNameDropDownList.DataValueField = "Key";
                TeamNameDropDownList.DataTextField = "Value";
                TeamNameDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(teamList);
                this.logFileService.LogError(SessionManager.UserId, "TEAM SUPPLY MASTER", "TeamSupplyMaster.aspx.cs", ex, modelJson);
            }
            finally { teamList = null;  }
        }
        #endregion
        #region Load Grid
        private void BindTeamSupplyGrid()
        {
            List<TeamSupplyMasterGridModel> teamSupplyGridData;
            try
            {
                this.teamSupplyService = new TeamSupplyService();
                teamSupplyGridData = teamSupplyService.GetTeamSupplyGridData();
                if (teamSupplyGridData != null && teamSupplyGridData.Count > 0) { TeamSupplyGridView.DataSource = teamSupplyGridData; }
                else
                {
                    TeamSupplyGridView.DataSource = new List<TeamSupplyMasterGridModel>();
                }
                TeamSupplyGridView.DataBind();
            }
            finally { teamSupplyGridData = null; }
        }
        protected void TeamSupplyGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TeamSupplyMasterGridModel teamSupplyMasterDetail = e.Row.DataItem as TeamSupplyMasterGridModel;
                    if (teamSupplyMasterDetail != null && teamSupplyMasterDetail.Team_Supply_Id > 0)
                    {
                        e.Row.Attributes["data-row-id"] = teamSupplyMasterDetail.Team_Supply_Id.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnTeamSupplyMasterDetailRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void TeamSupplyGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in TeamSupplyGridView.Rows)
                {
                    if (row.RowIndex == TeamSupplyGridView.SelectedIndex)
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
        protected void AddTeamSupply_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add Team Supply";
            Team_Supply_Id.Value = "0";
            TeamNameDropDownList.SelectedValue = "0";
            TeamHeadName.Text = "";
            popup_container.Visible = true;
        }
        protected void EditTeamSupply_Click(object sender, EventArgs e)
        {
            TeamSupplyMasterModel selTeamSupplyRowData;
            try
            {
                PopupHeaderText.InnerText = "Edit Team Supply";
                selectedRowId = SelectedRowIdHiddenField.Value;
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    selTeamSupplyRowData = new TeamSupplyMasterModel();
                    this.teamSupplyService = new TeamSupplyService();
                    selTeamSupplyRowData = teamSupplyService.FetchTeamSupplyData(selectedRowId);
                    Team_Supply_Id.Value = selTeamSupplyRowData.TeamSupplyId.ToString();
                    TeamNameDropDownList.SelectedValue = selTeamSupplyRowData.TeamId.ToString();
                    TeamHeadName.Text = selTeamSupplyRowData.TeamHeadName;
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
                this.logFileService.LogError(SessionManager.UserId, "TEAM SUPPLY MASTER", "TeamSupplyMaster.aspx.cs", ex, "");
            }
            finally { selTeamSupplyRowData = null; }
        }
        protected void DeleteTeamSupply_Click(object sender, EventArgs e)
        {
            selectedRowId = SelectedRowIdHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedRowId)) { popup_confirm_container.Visible = true; }
            else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Delete.');", true); return; }
        }
        protected void ConfirmDeleteTeamSupply_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.teamSupplyService = new TeamSupplyService();
                flag = teamSupplyService.DeleteTeamSupplyDetail(selectedRowId);
                if (flag > 0)
                {
                    BindTeamSupplyGrid();
                    popup_confirm_container.Visible = false;
                    SelectedRowIdHiddenField.Value = "";
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again.');", true); return; }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Deleted Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "TEAM SUPPLY MASTER", "TeamSupplyMaster.aspx.cs", ex, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
        #endregion
        #region Save Team Supply
        protected void SaveTeamSupplyDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            int flag;
            TeamSupplyMasterModel model = new TeamSupplyMasterModel();
            try
            {
                model = GetTeamSupplyDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg)) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true); return; }
                this.teamSupplyService = new TeamSupplyService();
                if (model.TeamSupplyId == 0) { flag = teamSupplyService.AddTeamSupplyDetail(model); }
                else { flag = teamSupplyService.EditTeamSupplyDetail(model); }
                if (flag > 0)
                {
                    BindTeamSupplyGrid();
                    popup_container.Visible = false;
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true); return; }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "TEAM SUPPLY MASTER", "TeamSupplyMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.teamSupplyService = null; this.__logFileService = null; }
        }
        private TeamSupplyMasterModel GetTeamSupplyDetails()
        {
            TeamSupplyMasterModel model;
            int teamSupplyIdValue;
            try
            {
                model = new TeamSupplyMasterModel();
                model.TeamSupplyId = int.TryParse(Team_Supply_Id.Value, out teamSupplyIdValue) ? teamSupplyIdValue : 0;
                model.TeamId = short.Parse(TeamNameDropDownList.SelectedValue);
                model.TeamHeadName = TeamHeadName.Text.Trim();
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(TeamSupplyMasterModel model)
        {
            if (model.TeamId <= 0) { return "Please Select Team Name."; }
            if (string.IsNullOrWhiteSpace(model.TeamHeadName)) { return "Please fill Team Head Name."; }
            return "";
        }
        #endregion
    }
}
