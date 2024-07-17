using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.BLL.Utility;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class TimeSheetMaster : System.Web.UI.Page
    {
        #region Common Variables
        private ITimeSheetService timeSheetService = null;
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
        public List<TimeSheetGridModel> timeSheetGridModelList;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //DateTime defaultDate;
            DateTime currentMonthFromDate, currentMonthToDate;
            try
            {
                if (!IsPostBack)
                {
                    PopulateAdminDropDownList();
                    currentMonthFromDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                    currentMonthToDate = currentMonthFromDate.AddMonths(1).AddDays(-1);
                    FromDate.Text = currentMonthFromDate.ToString("yyyy/MM/dd");
                    ToDate.Text = currentMonthToDate.ToString("yyyy/MM/dd");
                    UserDropDownList.SelectedValue = "1";
                    //defaultDate = DateTime.MinValue;
                    BindTimeSheetGrid(1, currentMonthFromDate, currentMonthToDate);
                }
            }
            finally { }
        }
        private void PopulateAdminDropDownList()
        {
            List<DropDownItem> adminList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                adminList = sharedService.GetAdminDropDownData();
                adminList.Insert(0, new DropDownItem { Key = 0, Value = "-Select-" });
                UserDropDownList.DataSource = adminList;
                UserDropDownList.DataValueField = "Key";
                UserDropDownList.DataTextField = "Value";
                UserDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(adminList);
                this.logFileService.LogError(SessionManager.UserId, "TIME SHEET MASTER", "TimeSheetMaster.aspx.cs", ex, modelJson);
            }
            finally { adminList = null; }
        }
        protected void BindTimeSheetGrid(int selUserId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                this.timeSheetService = new TimeSheetService();
                timeSheetGridModelList = new List<TimeSheetGridModel>();
                timeSheetGridModelList = timeSheetService.GetTimeSheetGridData(selUserId, fromDate, toDate);
                if (timeSheetGridModelList != null && timeSheetGridModelList.Count > 0) { TimeSheetGridView.DataSource = timeSheetGridModelList; }
                else { TimeSheetGridView.DataSource = new List<TimeSheetGridModel>(); }
                TimeSheetGridView.DataBind();
            }
            finally { timeSheetGridModelList = null; }
        }
        protected void TimeSheetGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TimeSheetGridModel timeSheetGridModel = e.Row.DataItem as TimeSheetGridModel;
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        foreach (var model in timeSheetGridModelList)
                        {
                            if (model.HolidayType != null)
                            {
                                foreach (GridViewRow row in TimeSheetGridView.Rows)
                                {
                                    row.CssClass = "Holiday_Row";
                                }
                            }
                            else { e.Row.CssClass = ""; }
                        }
                    }
                }
            }
            finally { }
        }
        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            DateTime fromDate;
            try
            {
                ToDate.Text = "";
                fromDate = DateTime.Parse(FromDate.Text);
                ToDate.Attributes["min"] = fromDate.ToString("yyyy-MM-dd");
            }
            finally { }
        }
        protected void SearchTimeSheetDetails(object sender, EventArgs e)
        {
            int selUserId;
            DateTime fromDate;
            DateTime toDate;
            try
            {
                selUserId = int.Parse(UserDropDownList.SelectedValue);
                if (selUserId <= 0) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select User.');", true); return; }
                if (DateTime.TryParse(FromDate.Text, out fromDate) && DateTime.TryParse(ToDate.Text, out toDate))
                {
                    //if (fromDate == null || toDate == null) {  }
                    BindTimeSheetGrid(selUserId, fromDate, toDate);
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select Date.');", true); return; }
            }
            finally { }
        }
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            DateTime defaultDate;
            try
            {
                UserDropDownList.SelectedValue = "0";
                FromDate.Text = "";
                ToDate.Text = "";
                FromDate.Text = string.Empty;
                ToDate.Text = string.Empty;
                defaultDate = DateTime.MinValue;
                BindTimeSheetGrid(0, defaultDate, defaultDate);
            }
            finally { }
        }
        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
            string filePath, fileName, ftpUrl;
            FileResponse fileResponse;
            try
            {
                filePath = (sender as LinkButton).CommandArgument;
                fileName = Path.GetFileName(filePath);
                ftpUrl = ($"{FileHelper.FtpServer}/" + $"{FileHelper.DownloadTargetFolder}/" + fileName);
                byte[] imageData = FileHelper.DownloadImageFromFtp(ftpUrl);
                if (imageData != null)
                {
                    Response.Clear();
                    Response.ContentType = "image/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment; filename=image.jpg");
                    Response.BinaryWrite(imageData);
                    Response.End();
                }
                //fileResponse = FileHelper.DownloadImageFromFtp(ftpUrl);
                //if (fileResponse.Status == "S") { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{fileResponse.Message}');", true); return; }
                //else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{fileResponse.Message}');", true); return; }
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }
        }
    }
}

