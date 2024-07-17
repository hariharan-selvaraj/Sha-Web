using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class PaymentTargetMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IPaymentTargetService paymentTargetService = null;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProjectDropDownList();
                BindPaymentTargetGrid();
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        private void PopulateProjectDropDownList()
        {
            List<DropDownItem> projectList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                projectList = sharedService.GetProjectDropDownData();
                projectList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Project-" });
                ProjectDropDownList.DataSource = projectList;
                ProjectDropDownList.DataValueField = "Key";
                ProjectDropDownList.DataTextField = "Value";
                ProjectDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(projectList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { projectList = null; }
        }
        private void BindPaymentTargetGrid()
        {
            List<PaymentTargetMasterGridModel> paymentTargetGridData;
            try
            {
                this.paymentTargetService = new PaymentTargetService();
                paymentTargetGridData = paymentTargetService.GetPaymentTargetGridData(SessionManager.UserId);
                if (paymentTargetGridData != null && paymentTargetGridData.Count > 0) { PaymentTargetGridView.DataSource = paymentTargetGridData; }
                else
                {
                    PaymentTargetGridView.DataSource = new List<PaymentTargetMasterGridModel>();
                }
                PaymentTargetGridView.DataBind();
            }
            finally { }
        }
        protected void PaymentTargetGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    PaymentTargetMasterGridModel paymentTargetDetail = e.Row.DataItem as PaymentTargetMasterGridModel;
                    if (paymentTargetDetail != null && paymentTargetDetail.PaymentTargetId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = paymentTargetDetail.PaymentTargetId.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnPaymentTargetDetailRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void PaymentTargetGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in PaymentTargetGridView.Rows)
                {
                    if (row.RowIndex == PaymentTargetGridView.SelectedIndex)
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
        protected void TargetFromDate_TextChanged(object sender, EventArgs e)
        {
            PaymentTargetToDate.Text = "";
            DateTime paymentTargetFromDate = DateTime.Parse(PaymentTargetFromDate.Text);
            PaymentTargetToDate.Attributes.Add("min", paymentTargetFromDate.ToString("yyyy-MM-dd"));
        }
        protected void SavePaymentTargetDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            int flag;
            PaymentTargetMasterModel model = new PaymentTargetMasterModel();
            try
            {
                model = GetPaymentTargetDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg)) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true); return; }
                this.paymentTargetService = new PaymentTargetService();
                if (model.PaymentTargetId == 0) { flag = paymentTargetService.AddPaymentTargetDetail(model); }
                else { flag = paymentTargetService.EditPaymentTargetDetail(model); }
                if (flag > 0)
                {
                    BindPaymentTargetGrid();
                    popup_container.Visible = false;
                }
                else
                {
                    if (flag == -2) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Selected Date is Already in Used.Please Select Another Date');", true); return; }
                    else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true); return; }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "PAYMENT TARGET MASTER", "PaymentTargetMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.paymentTargetService = null; this.__logFileService = null; }
        }
        private PaymentTargetMasterModel GetPaymentTargetDetails()
        {
            PaymentTargetMasterModel model;
            int menuItemIdValue;
            decimal amount;
            DateTime paymentTargetFromDate, paymentTargetToDate;
            try
            {
                model = new PaymentTargetMasterModel();
                model.PaymentTargetId = int.TryParse(PaymentTargetId.Value, out menuItemIdValue) ? menuItemIdValue : 0;
                model.ProjectId = int.Parse(ProjectDropDownList.SelectedValue);
                model.PaymentTargetType = TargetTypeDropDownList.SelectedValue;
                model.PaymentTargetFromDate = DateTime.TryParse(PaymentTargetFromDate.Text, out paymentTargetFromDate) ? paymentTargetFromDate.Date : DateTime.Now.Date;
                //string formattedDateTime = model.PaymentTargetFromDate.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                //model.PaymentTargetFromDate = Convert.ToDateTime(formattedDateTime,"yyyy-MM-dd HH:mm:ss.fff");
                model.PaymentTargetToDate = DateTime.TryParse(PaymentTargetToDate.Text, out paymentTargetToDate) ? paymentTargetToDate.Date : DateTime.Now.Date;
                model.Amount = decimal.TryParse(Amount.Text, out amount) ? amount : 0;
                model.PaymentTargetDescription = PaymentTargetDescription.Text.Trim();
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(PaymentTargetMasterModel model)
        {
            if (model.ProjectId <= 0) { return "Please Select Project Name."; }
            if (string.IsNullOrWhiteSpace(model.PaymentTargetType)) { return "Please fill PaymentTarget Type."; }
            if (string.IsNullOrWhiteSpace(model.PaymentTargetDescription)) { return "Please fill PaymentTarget Description."; }
            return "";
        }
        protected void AddPaymentTarget_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add PaymentTarget";
            PaymentTargetId.Value = "0";
            ProjectDropDownList.SelectedValue = "0";
            TargetTypeDropDownList.SelectedValue = "0";
            PaymentTargetFromDate.Text = "";
            PaymentTargetToDate.Text = "";
            Amount.Text = "";
            PaymentTargetDescription.Text = "";
            popup_container.Visible = true;
        }
        protected void EditPaymentTarget_Click(object sender, EventArgs e)
        {
            PaymentTargetMasterModel selPaymentTargetDetailsRowData;
            try
            {
                PopupHeaderText.InnerText = "Edit PaymentTarget";
                selectedRowId = SelectedRowIdHiddenField.Value;
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    selPaymentTargetDetailsRowData = new PaymentTargetMasterModel();
                    this.paymentTargetService = new PaymentTargetService();
                    selPaymentTargetDetailsRowData = paymentTargetService.FetchPaymentTargetData(selectedRowId);
                    PaymentTargetId.Value = selPaymentTargetDetailsRowData.PaymentTargetId.ToString();
                    ProjectDropDownList.SelectedValue = selPaymentTargetDetailsRowData.ProjectId.ToString();
                    TargetTypeDropDownList.Text = selPaymentTargetDetailsRowData.PaymentTargetType;
                    PaymentTargetFromDate.Text = selPaymentTargetDetailsRowData.PaymentTargetFromDate.ToString("yyyy-MM-dd");
                    PaymentTargetToDate.Text = selPaymentTargetDetailsRowData.PaymentTargetToDate.ToString("yyyy-MM-dd");
                    Amount.Text = selPaymentTargetDetailsRowData.Amount.ToString("0.00");
                    PaymentTargetDescription.Text = selPaymentTargetDetailsRowData.PaymentTargetDescription;
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
                this.logFileService.LogError(SessionManager.UserId, "PAYMENT TARGET MASTER", "PaymentTargetMaster.aspx.cs", ex, "");
            }
            finally { selPaymentTargetDetailsRowData = null; }
        }
        protected void DeletePaymentTarget_Click(object sender, EventArgs e)
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
        protected void ConfirmDeletePaymentTarget_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.paymentTargetService = new PaymentTargetService();
                flag = paymentTargetService.DeletePaymentTargetDetail(selectedRowId);
                if (flag > 0)
                {
                    BindPaymentTargetGrid();
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
                this.logFileService.LogError(SessionManager.UserId, "PAYMENT TARGET MASTER", "PaymentTargetMaster.aspx.cs", ex, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
    }
}