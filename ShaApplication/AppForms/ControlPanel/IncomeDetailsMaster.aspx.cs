using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class IncomeDetailsMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IIncomeDetailsService incomeDetailsService = null;
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
                PopulateCompanyDropDownList();
                PopulateProjectDropDownList();
                PopulateAccTypeDropDownList();
                PopulateAmountTypeDropDownList();
                PopulateGstRateDropDownList();
                PopulateAdminDropDownList();
                PopulatePaymentTargetDropDownList();
                CreditedDate.Attributes["max"] = DateTime.UtcNow.ToString("yyyy-MM-dd");
                BindIncomeDetailsGrid();
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
                ProjectNameDropDownList.DataSource = projectList;
                ProjectNameDropDownList.DataValueField = "Key";
                ProjectNameDropDownList.DataTextField = "Value";
                ProjectNameDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(projectList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { projectList = null; }
        }
        private void PopulateCompanyDropDownList()
        {
            List<DropDownItem> companyList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                companyList = sharedService.GetCompanyDropDownData();
                companyList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Company-" });
                CompanyDropDownList.DataSource = companyList;
                CompanyDropDownList.DataValueField = "Key";
                CompanyDropDownList.DataTextField = "Value";
                CompanyDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(companyList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { companyList = null; }
        }
        private void PopulateAccTypeDropDownList()
        {
            List<DropDownItem> accTypeList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.incomeDetailsService = new IncomeDetailsService();
                accTypeList = incomeDetailsService.GetRecivableAccTypeDrpoDownData();
                accTypeList.Insert(0, new DropDownItem { Key = 0, Value = "-Select AccountType-" });
                AccTypeDropDownList.DataSource = accTypeList;
                AccTypeDropDownList.DataValueField = "Key";
                AccTypeDropDownList.DataTextField = "Value";
                AccTypeDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(accTypeList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { accTypeList = null; }
        }
        private void PopulateAmountTypeDropDownList()
        {
            List<DropDownItem> amountTypeList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                amountTypeList = sharedService.GetAmountTypeDropDownData();
                amountTypeList.Insert(0, new DropDownItem { Key = 0, Value = "-Select AmountType-" });
                AmountTypeDropDownList.DataSource = amountTypeList;
                AmountTypeDropDownList.DataValueField = "Key";
                AmountTypeDropDownList.DataTextField = "Value";
                AmountTypeDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(amountTypeList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { amountTypeList = null; }
        }
        private void PopulateGstRateDropDownList()
        {
            List<DropDownItem> gstRateList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                gstRateList = sharedService.GetGstRateDropDownData();
                gstRateList.Insert(0, new DropDownItem { Key = 0, Value = "-Select GSTRate-" });
                GSTDropDownList.DataSource = gstRateList;
                GSTDropDownList.DataValueField = "Key";
                GSTDropDownList.DataTextField = "Value";
                GSTDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(gstRateList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { gstRateList = null; }
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
                AdminDropDownList.DataSource = adminList;
                AdminDropDownList.DataValueField = "Key";
                AdminDropDownList.DataTextField = "Value";
                AdminDropDownList.DataBind();
                EmployeeDropDownList.DataSource = adminList;
                EmployeeDropDownList.DataValueField = "Key";
                EmployeeDropDownList.DataTextField = "Value";
                EmployeeDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(adminList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { adminList = null; }
        }
        private void PopulatePaymentTargetDropDownList()
        {
            List<DropDownItem> paymentTargetList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                paymentTargetList = sharedService.GetPaymentTargetDropDownData();
                paymentTargetList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Target-" });
                TargetDescriptionDropDownList.DataSource = paymentTargetList;
                TargetDescriptionDropDownList.DataValueField = "Key";
                TargetDescriptionDropDownList.DataTextField = "Value";
                TargetDescriptionDropDownList.SelectedValue = null;
                TargetDescriptionDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(paymentTargetList);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { TargetDescriptionDropDownList = null; }
        }
        private void BindIncomeDetailsGrid()
        {
            List<IncomeDetailsGridModel> financialGridData;
            try
            {
                this.incomeDetailsService = new IncomeDetailsService();
                financialGridData = incomeDetailsService.GetIncomeDetailsGridData(SessionManager.UserId);
                if (financialGridData != null && financialGridData.Count > 0) { IncomeDetailsGridView.DataSource = financialGridData; }
                else { IncomeDetailsGridView.DataSource = new List<IncomeDetailsGridModel>(); }
                IncomeDetailsGridView.DataBind();
            }
            finally { financialGridData = null; }
        }
        protected void IncomeDetailsGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    IncomeDetailsGridModel incomeDetail = e.Row.DataItem as IncomeDetailsGridModel;
                    if (incomeDetail != null && incomeDetail.IncomeId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = incomeDetail.IncomeId.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnIncomeDetailRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void IncomeDetailsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in IncomeDetailsGridView.Rows)
                {
                    if (row.RowIndex == IncomeDetailsGridView.SelectedIndex)
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
        protected void ShowGstRateDropDownList(object sender, EventArgs e)
        {
            IncomeDetails model = new IncomeDetails();
            try
            {
                model.IsGST = IsGST.Checked;
                if (!model.IsGST) { GSTDropDownList.Attributes["disabled"] = "true"; GstLbl.Text = string.Empty; GSTDropDownList.SelectedValue = "0"; }
                else { GSTDropDownList.Attributes.Remove("disabled"); GstLbl.Text = "*"; }
            }
            finally { model = null; }
        }
        protected void ShowInvoiceRefDropDownList(object sender, EventArgs e)
        {
            IncomeDetails model = new IncomeDetails();
            try
            {
                model.IsInvoice = IsInvoice.Checked;
                if (!model.IsInvoice) { InvoiceRefId.Attributes["disabled"] = "true"; InvoiceRefIdMandatoryLbl.Text = string.Empty; InvoiceRefId.Text = ""; InvoiceRefId.Attributes.Remove("required"); }
                else { InvoiceRefId.Attributes.Remove("disabled"); InvoiceRefIdMandatoryLbl.Text = "*"; InvoiceRefId.Attributes.Add("required", "required"); }
            }
            finally { model = null; }
        }
        protected void ShowEmployeeDropDownList(object sender, EventArgs e)
        {
            IncomeDetails model = new IncomeDetails();
            try
            {
                model.IsEmployee = IsEmployee.Checked;
                if (!model.IsEmployee) { EmployeeDropDownList.Attributes["disabled"] = "true"; EmployeeLbl.Text = string.Empty; EmployeeDropDownList.SelectedValue = "0"; }
                else { EmployeeDropDownList.Attributes.Remove("disabled"); EmployeeLbl.Text = "*"; }
            }
            finally { model = null; }
        }
        protected void ShowProjectNameDropDownList(object sender, EventArgs e)
        {
            IncomeDetails model = new IncomeDetails();
            try
            {
                model.HasProject = IsProject.Checked;
                if (!model.HasProject) { ProjectNameDropDownList.Attributes["disabled"] = "true"; ProjectNameLbl.Text = string.Empty; ProjectNameDropDownList.SelectedValue = "0"; }
                else { ProjectNameDropDownList.Attributes.Remove("disabled"); ProjectNameLbl.Text = "*"; }
            }
            finally { model = null; }
        }
        protected void SaveIncomeDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            long flag;
            IncomeDetails model = new IncomeDetails();
            try
            {
                model = GetIncomeDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true);
                    return;
                }
                incomeDetailsService = new IncomeDetailsService();
                if (model.IncomeId == 0) { flag = incomeDetailsService.AddIncomeDetail(model); }
                else { flag = incomeDetailsService.EditIncomeDetail(model); }
                if (flag > 0) { BindIncomeDetailsGrid(); popup_container.Visible = false; }
                else
                {
                    if (flag == -2) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Invoice Reference Number is Already Exist.');", true); return; }
                    else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true); return; }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.incomeDetailsService = null; this.__logFileService = null; }
        }
        private IncomeDetails GetIncomeDetails()
        {
            IncomeDetails model;
            long incomeIdValue;
            decimal creditAmount;
            DateTime creditedDate;
            try
            {
                IsInvoice.Enabled = true;
                IsEmployee.Enabled = true;
                IsProject.Enabled = true;
                IsGST.Enabled = true;
                InvoiceRefId.Attributes.Remove("disabled");
                AccTypeDropDownList.Attributes.Remove("disabled");
                AmountTypeDropDownList.Attributes.Remove("disabled");
                InvoiceRefId.Attributes.Remove("disabled");
                GSTDropDownList.Attributes.Remove("disabled");
                CreditedDate.Attributes.Remove("disabled");
                model = new IncomeDetails();
                model.IncomeId = long.TryParse(IncomeID.Value, out incomeIdValue) ? incomeIdValue : 0;
                model.IsInvoice = IsInvoice.Checked;
                model.InvoiceRefId = InvoiceRefId.Text;
                model.CompanyId = int.Parse(CompanyDropDownList.SelectedValue);
                model.IsEmployee = IsEmployee.Checked;
                model.HasProject = IsProject.Checked;
                model.ProjectId = int.Parse(ProjectNameDropDownList.SelectedValue);
                model.ReceivableAccountTypeId = int.Parse(AccTypeDropDownList.SelectedValue);
                model.IncomeDescription = IncomeDescription.Text.Trim();
                model.CreditAmount = decimal.TryParse(CreditAmount.Text, out creditAmount) ? creditAmount : 0;
                model.CreditedDate = DateTime.TryParse(CreditedDate.Text, out creditedDate) ? creditedDate.Date : DateTime.Now.Date;
                model.IsGST = IsGST.Checked;
                model.GstRate = int.Parse(GSTDropDownList.SelectedValue);
                model.EmployeeId = int.Parse(EmployeeDropDownList.SelectedValue);
                model.AmountTypeId = int.Parse(AmountTypeDropDownList.SelectedValue);
                model.AdminId = int.Parse(AdminDropDownList.SelectedValue);
                model.TargetDescriptionId = int.Parse(TargetDescriptionDropDownList.SelectedValue);
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(IncomeDetails model)
        {
            if (IsInvoice.Checked)
            {
                if (string.IsNullOrWhiteSpace(model.InvoiceRefId)) { return "Please Enter Invoice RefId."; }
            }
            if (model.CompanyId <= 0) { return "Please select a Company."; }
            if (IsProject.Checked)
            {
                if (model.ProjectId <= 0) { return "Please Select Project."; }
            }
            if (IsGST.Checked)
            {
                if (model.GstRate <= 0) { return "Please select a Gst."; }
            }
            if (model.ReceivableAccountTypeId <= 0) { return "Please Select a Account Type."; }
            if (string.IsNullOrWhiteSpace(model.IncomeDescription)) { return "Please fill Description."; }
            if (model.CreditAmount <= 0) { return "Please Enter CreditAmount."; }
            if (model.CreditedDate == null) { return "Please Select Credited Date."; }
            if (model.AmountTypeId <= 0) { return "Please select a Amount Type."; }
            if (model.AdminId <= 0) { return "Please select Receiver Name."; }
            if (model.TargetDescriptionId <= 0) { return "Please select a TargetDescription."; }
            return "";
        }
        protected void AddIncomeDetail_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add Income";
            InvoiceRefId.Attributes.Remove("disabled");
            AccTypeDropDownList.Attributes.Remove("disabled");
            AmountTypeDropDownList.Attributes.Remove("disabled");
            InvoiceRefId.Attributes.Remove("disabled");
            GSTDropDownList.Attributes.Remove("disabled");
            CreditedDate.Attributes["disabled"] = "true";
            TargetDescriptionDropDownList.Attributes["disabled"] = "true";
            //TargetDescriptionDropDownList.Attributes.Add("onclick", "return false;");
            IsInvoice.Enabled = true;
            IsEmployee.Enabled = true;
            IsProject.Enabled = true;
            IsGST.Enabled = true;
            IncomeID.Value = "0";
            IsInvoice.Checked = true;
            InvoiceRefId.Text = "";
            CompanyDropDownList.SelectedValue = "0";
            EmployeeDropDownList.SelectedValue = "0";
            IsProject.Checked = true;
            ProjectNameDropDownList.SelectedValue = "0";
            AccTypeDropDownList.SelectedValue = "0";
            IncomeDescription.Text = "";
            CreditAmount.Text = "";
            CreditedDate.Text = "";
            AmountTypeDropDownList.SelectedValue = "0";
            AdminDropDownList.SelectedValue = "0";
            TargetDescriptionDropDownList.SelectedValue = "0";
            GSTDropDownList.SelectedValue = "0";
            IsGST.Checked = true;
            IsEmployee.Checked = true;
            popup_container.Visible = true;
        }
        protected void EditIncomeDetail_Click(object sender, EventArgs e)
        {
            IncomeDetails selIncomeDetailsRowData;
            PaymentTargetMasterModel paymentTargetMasterModel;
            DateTime creditedDate;
            string msg = "";
            try
            {
                PopupHeaderText.InnerText = "Edit Income";
                selectedRowId = SelectedRowIdHiddenField.Value;
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    this.incomeDetailsService = new IncomeDetailsService();
                    selIncomeDetailsRowData = new IncomeDetails();
                    selIncomeDetailsRowData = incomeDetailsService.FetchIncomeDetails(selectedRowId);
                    paymentTargetMasterModel = new PaymentTargetMasterModel();
                    sharedService = new SharedService();
                    creditedDate = DateTime.Parse(selIncomeDetailsRowData.CreditedDate.ToString());
                    paymentTargetMasterModel = sharedService.GetTargetDetailsByProjectId(creditedDate, (int)selIncomeDetailsRowData.ProjectId, out msg);
                    total_target_amount.Visible = true;
                    total_target_amount.InnerText = "Total Amount : $" + paymentTargetMasterModel.Amount.ToString("0.00");
                    IncomeID.Value = selIncomeDetailsRowData.IncomeId.ToString();
                    IsInvoice.Checked = selIncomeDetailsRowData.IsInvoice;
                    InvoiceRefId.Text = selIncomeDetailsRowData.InvoiceRefId;
                    CompanyDropDownList.SelectedValue = selIncomeDetailsRowData.CompanyId.ToString();
                    IsEmployee.Checked = selIncomeDetailsRowData.IsEmployee;
                    EmployeeDropDownList.SelectedValue=selIncomeDetailsRowData.EmployeeId.ToString();
                    IsProject.Checked = selIncomeDetailsRowData.HasProject;
                    ProjectNameDropDownList.SelectedValue = selIncomeDetailsRowData.ProjectId.ToString();
                    AccTypeDropDownList.SelectedValue = selIncomeDetailsRowData.ReceivableAccountTypeId.ToString();
                    IncomeDescription.Text = selIncomeDetailsRowData.IncomeDescription;
                    CreditAmount.Text = selIncomeDetailsRowData.CreditAmount.ToString("0.00");
                    CreditedDate.Text = selIncomeDetailsRowData.CreditedDate.ToString("yyyy-MM-dd");
                    IsGST.Checked = selIncomeDetailsRowData.IsGST;
                    GSTDropDownList.SelectedValue = selIncomeDetailsRowData.GstRate.ToString();
                    AmountTypeDropDownList.SelectedValue = selIncomeDetailsRowData.AmountTypeId.ToString();
                    AdminDropDownList.SelectedValue = selIncomeDetailsRowData.AdminId.ToString();
                    TargetDescriptionDropDownList.SelectedValue = selIncomeDetailsRowData.TargetDescriptionId.ToString();
                    IsInvoice.Enabled = false;
                    IsEmployee.Enabled = false;
                    IsProject.Enabled = false;
                    IsGST.Enabled = false;
                    InvoiceRefId.Attributes["disabled"] = "true";
                    AccTypeDropDownList.Attributes["disabled"] = "true";
                    AccTypeDropDownList.Attributes["onclick"] = "makeReadOnly(event)";
                    AmountTypeDropDownList.Attributes["disabled"] = "true";
                    AmountTypeDropDownList.Attributes["onclick"] = "makeReadOnly(event)";
                    GSTDropDownList.Attributes["disabled"] = "true";
                    EmployeeDropDownList.Attributes["disabled"] = "true";
                    GSTDropDownList.Attributes["onclick"] = "makeReadOnly(event)";
                    TargetDescriptionDropDownList.Attributes["disabled"] = "true";
                    TargetDescriptionDropDownList.Attributes["onclick"] = "makeReadOnly(event)";
                    popup_container.Visible = true;
                    SelectedRowIdHiddenField.Value = "";
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Edit.');", true); return; }
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, "");
            }
            finally { selIncomeDetailsRowData = null; }
        }
        protected void DeleteIncomeDetail_Click(object sender, EventArgs e)
        {
            selectedRowId = SelectedRowIdHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedRowId)) { popup_confirm_container.Visible = true; }
            else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Delete.');", true); return; }
        }
        protected void ConfirmDeleteIncome_Click(object sender, EventArgs e)
        {
            long flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.incomeDetailsService = new IncomeDetailsService();
                flag = incomeDetailsService.DeleteIncomeDetail(selectedRowId);
                if (flag > 0)
                {
                    BindIncomeDetailsGrid();
                    popup_confirm_container.Visible = false;
                    SelectedRowIdHiddenField.Value = "";
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again.');", true); return; }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Deleted Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
        protected void ProjectDropDownOnChange(object sender, EventArgs e)
        {
            int projectId = int.Parse(ProjectNameDropDownList.SelectedValue);
            if (projectId > 0) { CreditedDate.Attributes.Remove("disabled"); }
            else { CreditedDate.Attributes["disabled"] = "true"; }
        }
        protected void CreditedDate_TextChanged(object sender, EventArgs e)
        {
            DateTime creditedDate;
            int projectId;
            string msg = "";
            PaymentTargetMasterModel paymentTargetMasterModel;
            List<DropDownItem> targetList = new List<DropDownItem>();
            try
            {
                this.sharedService = new SharedService();
                creditedDate = DateTime.Parse(CreditedDate.Text);
                projectId = int.Parse(ProjectNameDropDownList.SelectedValue);
                if (creditedDate == null || projectId <= 0)
                {
                    targetList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Target-" });
                    TargetDescriptionDropDownList.DataSource = targetList;
                    TargetDescriptionDropDownList.DataValueField = "Key";
                    TargetDescriptionDropDownList.DataTextField = "Value";
                    TargetDescriptionDropDownList.DataBind();
                    TargetDescriptionDropDownList.SelectedValue = "0";
                    TargetDescriptionDropDownList.Attributes["disabled"] = "true";
                    return;
                }
                paymentTargetMasterModel = sharedService.GetTargetDetailsByProjectId(creditedDate, projectId, out msg);
                if (paymentTargetMasterModel == null || !string.IsNullOrWhiteSpace(msg))
                {
                    CreditedDate.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Date And Project Name MisMatch.');", true);
                    return;
                }
                targetList.Insert(0, new DropDownItem { Key = paymentTargetMasterModel.PaymentTargetId, Value = paymentTargetMasterModel.PaymentTargetDescription });
                TargetDescriptionDropDownList.DataSource = targetList;
                TargetDescriptionDropDownList.DataValueField = "Key";
                TargetDescriptionDropDownList.DataTextField = "Value";
                TargetDescriptionDropDownList.DataBind();
                TargetDescriptionDropDownList.SelectedValue = paymentTargetMasterModel.PaymentTargetId.ToString();
                TargetDescriptionDropDownList.Attributes["disabled"] = "true";
                total_target_amount.Visible = true;
                total_target_amount.InnerText = "Total Amount : $" + paymentTargetMasterModel.Amount.ToString("0.00");
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "INCOME DETAILS MASTER", "IncomeDetailsMaster.aspx.cs", ex, "");
            }
            finally { sharedService = null; paymentTargetMasterModel = null; targetList = null; }
        }
    }
}