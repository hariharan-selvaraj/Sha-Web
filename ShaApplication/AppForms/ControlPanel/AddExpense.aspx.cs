using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.BLL.Utility;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class AddExpense : System.Web.UI.Page
    {
        #region Common Variables
        private IExpenseDetailsService expenseDetailsService = null;
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
        private long __selectedRowId = 0;
        private string selectedDetailsRowId = null;
        private string selectedDetailsId = null;
        private long selectedRowId
        {
            get
            {
                if (Session["selectedRowId"] != null && !string.IsNullOrWhiteSpace(Session["selectedRowId"].ToString()))
                {
                    __selectedRowId = long.Parse(Session["selectedRowId"].ToString());
                }
                else
                {
                    __selectedRowId = 0;
                }
                return __selectedRowId;
            }
        }
        private static List<ExpenseDetails> ExpenseDetailsDataList = new List<ExpenseDetails>();
        private static List<ExpenseDetailsGridModel> ExpenseDetailsGridDataList = new List<ExpenseDetailsGridModel>();
        #endregion
        #region PageLoad Functions
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                PopulateCompanyDropDownList();
                PopulatePayableAccTypeDropDownList();
                PopulateAmountTypeDropDownList();
                PopulatePaidReceivedByDropDownList();
                PopulateTargetDescriptionDropDownList();
                PopulateProjectDropDownList();
                PopulateGSTRateDropDownList();
                DebitedDate.Attributes["max"] = DateTime.UtcNow.ToString("yyyy-MM-dd");
                DebitedDate.Attributes["min"] = DateTime.UtcNow.ToString("2021-11-19");
                if (selectedRowId > 0) { EditExpenseHeader(); }
                else
                {
                    ExpenseDetailsDataList = new List<ExpenseDetails>();
                    GSTDropDownList.Attributes["readonly"] = "true";
                    Expensefileupload.Visible = true; FileUploadBtn.Visible = true; Expense_file_div.Visible = false;
                    AddExpenseHeader();
                }
                //if (SessionManager.ExpenseUploadEDFilePath != null) { Expensefileupload.Visible = false; FileUploadBtn.Visible = false; upload_fileName_div.Visible = true; }
                //else { Expensefileupload.Visible = true; FileUploadBtn.Visible = true; Expense_file_div.Visible = false; }
                IsInvoiceCheckbox.Checked = true;
                //Expense_file_div.Visible = false;
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        #endregion
        #region DropDowns Load Functions
        private void PopulateCompanyDropDownList()
        {
            List<DropDownItem> companyList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                companyList = sharedService.GetCompanyDropDownData();
                companyList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Supplier-" });
                SupplierDownList.DataSource = companyList;
                SupplierDownList.DataValueField = "Key";
                SupplierDownList.DataTextField = "Value";
                SupplierDownList.DataBind();
                CompanyDropDownList.DataSource = companyList;
                CompanyDropDownList.DataValueField = "Key";
                CompanyDropDownList.DataTextField = "Value";
                CompanyDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(companyList);
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { companyList = null; }
        }
        private void PopulatePayableAccTypeDropDownList()
        {
            List<DropDownItem> accTypeList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.expenseDetailsService = new ExpenseDetailsService();
                accTypeList = expenseDetailsService.GetPayableAccTypeDropDownData();
                accTypeList.Insert(0, new DropDownItem { Key = 0, Value = "-Select AccountType-" });
                AccountTypeDropDownList.DataSource = accTypeList;
                AccountTypeDropDownList.DataValueField = "Key";
                AccountTypeDropDownList.DataTextField = "Value";
                AccountTypeDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(accTypeList);
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
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
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { amountTypeList = null; }
        }
        private void PopulatePaidReceivedByDropDownList()
        {
            List<DropDownItem> adminList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                adminList = sharedService.GetAdminDropDownData();
                adminList.Insert(0, new DropDownItem { Key = 0, Value = "-Select-" });
                PaidByDropDownList.DataSource = adminList;
                PaidByDropDownList.DataValueField = "Key";
                PaidByDropDownList.DataTextField = "Value";
                PaidByDropDownList.DataBind();
                ReceivedByDropDownList.DataSource = adminList;
                ReceivedByDropDownList.DataValueField = "Key";
                ReceivedByDropDownList.DataTextField = "Value";
                ReceivedByDropDownList.DataBind();
                EmployeeDropDownList.DataSource = adminList;
                EmployeeDropDownList.DataValueField = "Key";
                EmployeeDropDownList.DataTextField = "Value";
                EmployeeDropDownList.DataBind();
                popup_confirm_container.Visible = false;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(adminList);
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { adminList = null; }
        }
        private void PopulateTargetDescriptionDropDownList()
        {
            List<DropDownItem> targetList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                targetList = sharedService.GetPaymentTargetDropDownData();
                targetList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Target-" });
                TargetDescriptionDropDownList.DataSource = targetList;
                TargetDescriptionDropDownList.DataValueField = "Key";
                TargetDescriptionDropDownList.DataTextField = "Value";
                TargetDescriptionDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(targetList);
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { targetList = null; }
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
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { projectList = null; }
        }
        private void PopulateGSTRateDropDownList()
        {
            List<DropDownItem> gstList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.sharedService = new SharedService();
                gstList = sharedService.GetGstRateDropDownData();
                gstList.Insert(0, new DropDownItem { Key = 0, Value = "-Select GST-" });
                GSTDropDownList.DataSource = gstList;
                GSTDropDownList.DataValueField = "Key";
                GSTDropDownList.DataTextField = "Value";
                GSTDropDownList.DataBind();
                ExpenseGSTDropDownList.DataSource = gstList;
                ExpenseGSTDropDownList.DataValueField = "Key";
                ExpenseGSTDropDownList.DataTextField = "Value";
                ExpenseGSTDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(gstList);
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { gstList = null; }
        }
        #endregion
        #region CHeckBox Onchanged Events
        protected void FnDisabledInvoiceRefId(object sender, EventArgs e)
        {
            ExpenseHeader model = new ExpenseHeader();
            try
            {
                model.IsInvoice = IsInvoiceCheckbox.Checked;
                if (!model.IsInvoice)
                {
                    InvoiceRefNo.Text = "";
                    InvoiceRefNo.Attributes["readonly"] = "false";
                    InvoiceRefIdMandatoryLbl.Text = string.Empty;
                    InvoiceRefNo.Attributes.Remove("required");
                }
                else { InvoiceRefNo.Attributes.Remove("readonly"); InvoiceRefIdMandatoryLbl.Text = "*"; InvoiceRefNo.Attributes.Add("required", "required"); }
            }
            finally { model = null; }
        }
        protected void FnShowGSTDropDownList(object sender, EventArgs e)
        {
            ExpenseHeader model = new ExpenseHeader();
            try
            {
                model.IsGST = IsGSTCheckbox.Checked;
                if (!model.IsGST)
                {
                    GSTDropDownList.SelectedValue = "0";
                    GSTDropDownList.Attributes["readonly"] = "true";
                    GstMandatoryLbl.Text = string.Empty;
                }
                else { GSTDropDownList.Attributes.Remove("readonly"); GstMandatoryLbl.Text = "*"; }
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
        #endregion
        #region Upload FIle In FTP server
        protected void FileUploadBtn_Click(object sender, EventArgs e)
        {
            ExpenseHeader model = new ExpenseHeader();
            FileInputParam fileDetails;
            long fileSize;
            string fileInfo;
            try
            {
                if (Expensefileupload.HasFile)
                {
                    fileDetails = new FileInputParam();
                    fileDetails.BaseDirectory = ConfigurationManager.AppSettings["FileBaseURL"];
                    fileDetails.FolderName = ConfigurationManager.AppSettings["FolderPath"];
                    fileDetails.DirectoryName = "ExpenseFiles";
                    fileDetails.FileName = Expensefileupload.FileName;
                    fileDetails.InputStream = Expensefileupload.FileContent;
                    fileSize = Expensefileupload.PostedFile.ContentLength;
                    FileHelper.Upload(fileDetails);
                    SessionManager.ExpenseUploadEDFilePath = FileHelper.FtpServer + FileHelper.TargetFolder + '/' + Expensefileupload.FileName;
                    if (!string.IsNullOrEmpty(SessionManager.ExpenseUploadEDFilePath) && SessionManager.ExpenseUploadEDFilePath != null)
                    {
                        Expensefileupload.Visible = false;
                        FileUploadBtn.Visible = false;
                        fileInfo = $"{Expensefileupload.FileName} ({fileSize / 1024} KB)";
                        upload_fileName_div.InnerText = fileInfo;
                        //Expense_file_div.Style["display"] = "flex";
                        DeleteUploadFileBtn.Visible = true;
                        Expense_file_div.Attributes["class"] = "show_uploaded_div";
                        Expense_file_div.Visible = true;
                    }
                    else { Expensefileupload.Visible = true; FileUploadBtn.Visible = true; Expense_file_div.Visible = false; }
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('Please Choose File.');", true); model.ExpenseFile = ""; }
            }
            finally { Expensefileupload.Attributes.Clear(); }
        }
        #endregion
        #region Add,Edit,Delete For Expense Header
        protected void SaveExpenseHeader(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            long flag;
            ExpenseHeader model = new ExpenseHeader();
            try
            {
                model = GetExpenseDetails(false, out msg);
                if (model == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}.');", true);
                    return;
                }
                msg = ValidateModel(model, false);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true);
                    return;
                }
                this.expenseDetailsService = new ExpenseDetailsService();
                if (model.ExpenseId == 0)
                {
                    flag = expenseDetailsService.AddExpenseDetail(model);
                }
                else
                {
                    flag = expenseDetailsService.EditExpenseDetail(model);
                }
                string absolutePath = WebHelper.GetNavigationUrl("ExpenseDetailsMaster.aspx");
                if (flag > 0)
                {
                    string redirectUrl = $"{absolutePath}?message={HttpUtility.UrlEncode("Saved Sussessfully.")}";
                    Response.Redirect(redirectUrl, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    if (flag == -2) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('ReferenceId is Already Exist.');", true); return; }
                    else
                    {
                        string redirectUrl = $"{absolutePath}?message={HttpUtility.UrlEncode("Problem In Save.Please try again.")}";
                        Response.Redirect(redirectUrl, false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "SAVE EXPENSE HEADER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.expenseDetailsService = null; this.__logFileService = null; }
        }
        private ExpenseHeader GetExpenseDetails(bool isFromDetails, out string msg)
        {
            ExpenseHeader model;
            long expenseIdValue;
            decimal debitAmount;
            DateTime debitedDate;
            string formattedDebitAmount;
            try
            {
                msg = "";
                model = new ExpenseHeader();
                model.ExpenseId = long.TryParse(ExpenseID.Value, out expenseIdValue) ? expenseIdValue : 0;
                model.IsInvoice = IsInvoiceCheckbox.Checked;
                model.InvoiceRefId = InvoiceRefNo.Text.Trim();
                model.SupplierId = int.Parse(SupplierDownList.SelectedItem.Value);
                model.ExpenseDescription = ExpenseDescription.Text.Trim();
                formattedDebitAmount = DebitAmount.Text.Replace("₹", "").Replace(",", "").Trim();
                model.DebitOverAllAmount = decimal.TryParse(formattedDebitAmount, out debitAmount) ? debitAmount : 0;
                model.DebitedDate = DateTime.TryParse(DebitedDate.Text, out debitedDate) ? debitedDate : DateTime.Now;
                model.IsGST = IsGSTCheckbox.Checked;
                model.IsEmployee = IsEmployee.Checked;
                model.EmployeeId = int.Parse(EmployeeDropDownList.SelectedValue);
                model.Header_GST = int.Parse(GSTDropDownList.SelectedItem.Value);
                model.PayableAccTypeId = int.Parse(AccountTypeDropDownList.SelectedItem.Value);
                model.AmountTypeId = int.Parse(AmountTypeDropDownList.SelectedItem.Value);
                model.PaidBy = int.Parse(PaidByDropDownList.SelectedItem.Value);
                model.ReceivedBy = int.Parse(ReceivedByDropDownList.SelectedItem.Value);
                model.CreatedBy = SessionManager.UserId;
                if (!isFromDetails)
                {
                    foreach (var expense in ExpenseDetailsDataList)
                    {
                        expense.Amount = new string((expense.Amount.Where(c => char.IsDigit(c) || c == '.').ToArray()));
                    }
                    model.ExpenseOtherDetails = ExpenseDetailsDataList;
                    model.ExpenseFile = SessionManager.ExpenseUploadEDFilePath;
                }
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(ExpenseHeader model, bool isFromDetails)
        {
            if (string.IsNullOrWhiteSpace(model.InvoiceRefId)) { return "Please Enter InvoiceRef Number."; }
            if (model.SupplierId <= 0) { return "Please select a Supplier."; }
            if (string.IsNullOrEmpty(model.ExpenseDescription)) { return "Please fill Description."; }
            if (model.DebitOverAllAmount <= 0) { return "Please Enter Debited Amount."; }
            if (model.DebitedDate == null) { return "Please Select Debited Date."; }
            if (model.PayableAccTypeId <= 0) { return "Please select a Account Type."; }
            if (model.AmountTypeId <= 0) { return "Please select a Amount Type."; }
            if (model.PaidBy <= 0) { return "Please select a PaidBy."; }
            if (model.ReceivedBy <= 0) { return "Please select a ReceivedBy."; }
            if (IsInvoiceCheckbox.Checked)
            {
                if (string.IsNullOrWhiteSpace(model.InvoiceRefId)) { return "Please Enter Invoice refId."; }
            }
            if (IsGSTCheckbox.Checked)
            {
                if (model.Header_GST <= 0) { return "Please Select Gst."; }
            }
            if (!isFromDetails)
            {
                if (model.ExpenseOtherDetails == null || model.ExpenseOtherDetails.Count <= 0) { return "Please Enter Expense Details."; }
                if (model.DebitOverAllAmount != model.ExpenseOtherDetails.Sum(detail => decimal.Parse(detail.Amount))) { return "Mismatch in Debited Amount."; }
                if (IsGSTCheckbox.Checked)
                {
                    if (!model.ExpenseOtherDetails.Any(e => e.Gst == model.Header_GST)) { return "Mismatch of Gst."; }
                }
            }
            return "";
        }
        private void AddExpenseHeader()
        {
            Header_text.InnerText = "Add Expense";
            ExpenseID.Value = "0";
            InvoiceRefNo.Text = "";
            SupplierDownList.SelectedIndex = 0;
            AccountTypeDropDownList.SelectedIndex = 0;
            ExpenseDescription.Text = "";
            DebitAmount.Text = "";
            DebitedDate.Text = "";
            AmountTypeDropDownList.SelectedIndex = 0;
            PaidByDropDownList.SelectedIndex = 0;
            ReceivedByDropDownList.SelectedIndex = 0;
            Expensefileupload.TabIndex = 0;
            GstMandatoryLbl.Text = string.Empty;
            ExpenseDetailsGridView.DataSource = new List<ExpenseDetailsGridModel>();
            DeleteUploadFileBtn.Visible = true;
            IsEmployee.Enabled = true;
            IsEmployee.Checked = true;
            ImageViewPopUp.Visible = false;
            popupImage.Visible = false;
            view_file_div.Visible = false;
            ExpenseDetailsGridView.DataBind();
        }
        private void EditExpenseHeader()
        {
            ExpenseHeader selExpenseDetailsRowData;
            try
            {
                if (selectedRowId > 0)
                {
                    view_file_div.Visible = true;
                    this.expenseDetailsService = new ExpenseDetailsService();
                    selExpenseDetailsRowData = new ExpenseHeader();
                    selExpenseDetailsRowData = expenseDetailsService.FetchExpenseHeader(selectedRowId);
                    if (selExpenseDetailsRowData == null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('No Record Found.');", true);
                        return;
                    }
                    Header_text.InnerText = "Edit Expense";
                    ExpenseID.Value = selExpenseDetailsRowData.ExpenseId.ToString();
                    IsInvoiceCheckbox.Checked = selExpenseDetailsRowData.IsInvoice;
                    IsInvoiceCheckbox.Enabled = false;
                    InvoiceRefNo.Text = selExpenseDetailsRowData.InvoiceRefId;
                    InvoiceRefNo.Attributes["readonly"] = "true";
                    SupplierDownList.SelectedValue = selExpenseDetailsRowData.SupplierId.ToString();
                    SupplierDownList.Attributes["readonly"] = "true";
                    ExpenseDescription.Text = selExpenseDetailsRowData.ExpenseDescription;
                    DebitAmount.Text = selExpenseDetailsRowData.DebitOverAllAmount.ToString("0.00");
                    DebitedDate.Text = selExpenseDetailsRowData.DebitedDate.ToString("yyyy-MM-dd");
                    IsGSTCheckbox.Checked = selExpenseDetailsRowData.IsGST;
                    IsGSTCheckbox.Enabled = false;
                    IsEmployee.Checked = selExpenseDetailsRowData.IsEmployee;
                    IsEmployee.Enabled = false;
                    EmployeeDropDownList.Attributes["readonly"] = "true";
                    GSTDropDownList.SelectedValue = selExpenseDetailsRowData.Header_GST.ToString();
                    IsEmployee.Checked = selExpenseDetailsRowData.IsEmployee;
                    EmployeeDropDownList.SelectedValue = selExpenseDetailsRowData.EmployeeId.ToString();
                    if (IsGSTCheckbox.Checked)
                    {
                        GSTDropDownList.Attributes.Remove("readonly");
                    }
                    else
                    {
                        GSTDropDownList.Attributes["readonly"] = "false";
                    }
                    AccountTypeDropDownList.SelectedValue = selExpenseDetailsRowData.PayableAccTypeId.ToString();
                    AccountTypeDropDownList.Attributes["readonly"] = "true";
                    AmountTypeDropDownList.SelectedValue = selExpenseDetailsRowData.AmountTypeId.ToString();
                    AmountTypeDropDownList.Attributes["readonly"] = "true";
                    PaidByDropDownList.SelectedValue = selExpenseDetailsRowData.PaidBy.ToString();
                    PaidByDropDownList.Attributes["readonly"] = "true";
                    ReceivedByDropDownList.SelectedValue = selExpenseDetailsRowData.ReceivedBy.ToString();
                    ReceivedByDropDownList.Attributes["readonly"] = "true";
                    if (!string.IsNullOrWhiteSpace(selExpenseDetailsRowData.ExpenseFile))
                    {
                        this.ShowUploadFile(selExpenseDetailsRowData.ExpenseFile);
                    }
                    BindExpenseDetailsGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Edit.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "GET EXPENSE HEADER DATA", "ExpenseDetailsMaster.aspx.cs", ex, "");
            }
            finally { selExpenseDetailsRowData = null; }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            string redirectUrl = WebHelper.GetNavigationUrl("ExpenseDetailsMaster.aspx");
            Response.Redirect(redirectUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion
        #region Grid Bind ,Add,Edit,Delete For Expense Details
        private void BindExpenseDetailsGrid()
        {
            try
            {
                this.expenseDetailsService = new ExpenseDetailsService();
                ExpenseDetailsDataList = expenseDetailsService.GetExpenseOtherDetailsGridData(selectedRowId);
                if (ExpenseDetailsDataList != null && ExpenseDetailsDataList.Count > 0)
                {
                    ExpenseDetailsGridDataList = FilledExpenseDetailsModelToGridModel(ExpenseDetailsDataList);
                    ExpenseDetailsGridView.DataSource = ExpenseDetailsGridDataList;
                }
                else
                {
                    ExpenseDetailsGridView.DataSource = new List<ExpenseDetailsGridModel>();
                }
                ExpenseDetailsGridView.DataBind();
            }
            finally { }
        }
        protected void ExpenseDetailsGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ExpenseDetailsGridModel expenseDetail = e.Row.DataItem as ExpenseDetailsGridModel;
                    if (expenseDetail != null && expenseDetail.DetailsRowId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = expenseDetail.DetailsRowId.ToString();
                        e.Row.Attributes["data-id"] = expenseDetail.Id.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnExpenseDetailsRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void ExpenseDetailsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in ExpenseDetailsGridView.Rows)
                {
                    if (row.RowIndex == ExpenseDetailsGridView.SelectedIndex)
                    {
                        row.CssClass = "selected_row";
                        selectedDetailsRowId = row.Attributes["data-row-id"];
                        selectedDetailsId = row.Attributes["data-id"];
                        editBtn.EnableViewState = !string.IsNullOrEmpty(selectedDetailsRowId) ? true : false;
                        editBtn.EnableViewState = !string.IsNullOrEmpty(selectedDetailsId) ? true : false;
                        SelectedDetailsRowIdHiddenField.Value = selectedDetailsRowId;
                        SelectedDetailsIdHiddenField.Value = selectedDetailsId;
                    }
                    else
                    {
                        row.CssClass = "";
                    }
                }
            }
            finally { }
        }
        protected void AddExpenseDetails(object sender, EventArgs e)
        {
            ExpenseHeader model;
            List<DropDownItem> targetList = new List<DropDownItem>();
            string msg = "";
            try
            {
                model = GetExpenseDetails(true, out msg);
                msg = ValidateModel(model, true);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    SelectedDetailsRowIdHiddenField.Value = "0";
                    SelectedDetailsIdHiddenField.Value = "0";
                    Details_text.InnerText = "Add Expense Details";
                    total_target_amount.Visible = false;
                    balance_target_amount.Visible = false;
                    ExpenseDetailsRowId.Value = "0";
                    ExpenseDetailsId.Value = "0";
                    CompanyDropDownList.SelectedValue = "0";
                    ProjectNameDropDownList.SelectedValue = "0";
                    targetList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Target-" });
                    TargetDescriptionDropDownList.DataSource = targetList;
                    TargetDescriptionDropDownList.DataValueField = "Key";
                    TargetDescriptionDropDownList.DataTextField = "Value";
                    TargetDescriptionDropDownList.DataBind();
                    TargetDescriptionDropDownList.SelectedValue = "0";
                    TargetDescriptionDropDownList.Attributes["readonly"] = "true";
                    Description.Text = "";
                    Amount.Text = "";
                    Amount.Attributes["readonly"] = "true";
                    if (IsGSTCheckbox.Checked && int.Parse(GSTDropDownList.SelectedValue) > 0)
                    {
                        ExpenseGSTDropDownList.SelectedValue = GSTDropDownList.SelectedValue;
                        ExpenseGSTDropDownList.Attributes["readonly"] = "true";
                    }
                    else
                    {
                        ExpenseGSTDropDownList.SelectedValue = "0";
                        ExpenseGSTDropDownList.Attributes["readonly"] = "true";
                    }
                    popup_container.Visible = true;
                    CompanyDropDownList.Attributes.Remove("readonly");
                    ProjectNameDropDownList.Attributes.Remove("readonly");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true);
                    return;
                }
            }
            finally { model = null; targetList = null; }
        }

        protected void BtnCancelExpensePopup_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            SelectedDetailsRowIdHiddenField.Value = "0";
            SelectedDetailsIdHiddenField.Value = "0";
        }
        protected void EditExpenseDetails(object sender, EventArgs e)
        {
            ExpenseDetails model;
            PaymentTargetMasterModel paymentTargetMasterModel;
            DateTime debitedDate;
            string msg = "";
            decimal total_Amount, balance_Amount, details_Added_Amount;
            try
            {
                selectedDetailsRowId = SelectedDetailsRowIdHiddenField.Value;
                selectedDetailsId = SelectedDetailsIdHiddenField.Value;
                if (!string.IsNullOrEmpty(selectedDetailsRowId))
                {
                    Details_text.InnerText = "Edit Expense Details";
                    ExpenseDetailsRowId.Value = selectedDetailsRowId;
                    ExpenseDetailsId.Value = selectedDetailsId;
                    model = ExpenseDetailsDataList.Where(c => c.RowId == int.Parse(selectedDetailsRowId) && c.Id == int.Parse(ExpenseDetailsId.Value) && c.ExpenseId == selectedRowId).FirstOrDefault();
                    if (model == null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('No Record Found.');", true);
                        return;
                    }
                    CompanyDropDownList.SelectedValue = model.CompanyId.ToString();
                    CompanyDropDownList.Attributes["readonly"] = "true";
                    ProjectNameDropDownList.SelectedValue = model.ProjectId.ToString();
                    ProjectNameDropDownList.Attributes["readonly"] = "true";
                    PopulateTargetDescriptionDropDownList();
                    TargetDescriptionDropDownList.SelectedValue = model.TargetDescriptionId.ToString();
                    TargetDescriptionDropDownList.Attributes["readonly"] = "true";
                    Description.Text = model.Description;
                    string numericString = new string((model.Amount.Where(c => char.IsDigit(c) || c == '.').ToArray()));
                    Amount.Text = decimal.Parse(numericString).ToString("0.00");
                    if (IsGSTCheckbox.Checked)
                    {
                        ExpenseGSTDropDownList.SelectedValue = GSTDropDownList.SelectedValue;
                    }
                    debitedDate = DateTime.Parse(DebitedDate.Text);
                    paymentTargetMasterModel = new PaymentTargetMasterModel();
                    sharedService = new SharedService();
                    paymentTargetMasterModel = sharedService.GetTargetDetailsByProjectId(debitedDate, (int)model.ProjectId, out msg);
                    total_target_amount.Visible = true;
                    total_target_amount.InnerText = "Total Amount : $" + paymentTargetMasterModel.Amount.ToString("0.00");
                    total_Amount = decimal.Parse(paymentTargetMasterModel.Amount.ToString("0.00"));
                    details_Added_Amount = ExpenseDetailsDataList.Sum(a => decimal.Parse(new string(a.Amount.Where(c => char.IsDigit(c) || c == '.').ToArray())));
                    balance_Amount = total_Amount - details_Added_Amount;
                    balance_target_amount.Visible = true;
                    balance_target_amount.InnerText = "Balance Amount : $" + balance_Amount.ToString("0.00");
                    ExpenseGSTDropDownList.Attributes["readonly"] = "true";
                    popup_container.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Edit.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, "");
            }
            finally { paymentTargetMasterModel = null; sharedService = null; msg = null; }
        }

        protected void SaveExpenseDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            ExpenseDetails model = new ExpenseDetails();
            try
            {
                model = GetExpenseDetails();
                if (model == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('No Record Found.');", true);
                    return;
                }
                msg = ValidateExpenseModel(model);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true);
                    return;
                }
                this.expenseDetailsService = new ExpenseDetailsService();
                if (!IsExistInList(model.RowId))
                {
                    ExpenseDetailsDataList.Add(model);
                }
                else
                {
                    ExpenseDetailsDataList = UpdateExpenseDetails(model);
                }
                if (ExpenseDetailsDataList == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true);
                    return;
                }
                ExpenseDetailsGridDataList = FilledExpenseDetailsModelToGridModel(ExpenseDetailsDataList);
                if (ExpenseDetailsGridDataList == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true);
                    return;
                }
                ExpenseDetailsGridView.DataSource = ExpenseDetailsGridDataList;
                ExpenseDetailsGridView.DataBind();
                popup_container.Visible = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.expenseDetailsService = null; this.__logFileService = null; }
        }
        private bool IsExistInList(int rowId)
        {
            if (ExpenseDetailsDataList == null || ExpenseDetailsDataList.Count <= 0)
            {
                return false;
            }
            return ExpenseDetailsDataList.Any(e => e.RowId == rowId);
        }
        private ExpenseDetails GetExpenseDetails()
        {
            ExpenseDetails model;
            int details_id, row_id;
            try
            {
                model = new ExpenseDetails();
                if (int.Parse(ExpenseDetailsRowId.Value) > 0)
                {
                    model.RowId = int.TryParse(ExpenseDetailsRowId.Value, out row_id) ? row_id : 0;
                }
                else
                {
                    model.RowId = ExpenseDetailsDataList.Count + 1;
                }
                model.Id = int.TryParse(ExpenseDetailsId.Value, out details_id) ? details_id : 0;
                model.ExpenseId = selectedRowId > 0 ? selectedRowId : 0;
                model.CompanyId = int.Parse(CompanyDropDownList.SelectedItem.Value);
                model.CompanyName = CompanyDropDownList.SelectedItem.Text;
                model.ProjectId = int.Parse(ProjectNameDropDownList.SelectedItem.Value);
                model.ProjectName = ProjectNameDropDownList.SelectedItem.Text;
                model.TargetDescriptionId = int.Parse(TargetDescriptionDropDownList.SelectedItem.Value);
                model.TargetDescription = TargetDescriptionDropDownList.SelectedItem.Text;
                model.Description = Description.Text.Trim();
                model.Amount = Amount.Text.Trim();
                model.Gst = int.Parse(ExpenseGSTDropDownList.SelectedItem.Value);
                model.ExpenseDetailsGSTName = ExpenseGSTDropDownList.SelectedItem.Text;
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateExpenseModel(ExpenseDetails model)
        {
            if (model.CompanyId <= 0) { return "Please select a Company."; }
            if (model.ProjectId <= 0) { return "Please select a Project."; }
            if (model.TargetDescriptionId <= 0) { return "Please select a Target Description."; }
            if (string.IsNullOrEmpty(model.Description)) { return "Please fill Description."; }
            if (decimal.Parse(model.Amount) <= 0) { return "Please Enter a Amount."; }
            if (IsGSTCheckbox.Checked)
            {
                if (model.Gst <= 0) { return "Please Select Gst."; }
            }
            if (ExpenseDetailsDataList.Any(e => e.ProjectId == model.ProjectId)) { return "This Project is Already Added.Please Select Another Project."; }
            return "";
        }
        private List<ExpenseDetails> UpdateExpenseDetails(ExpenseDetails model)
        {
            ExpenseDetails expenseModel = new ExpenseDetails();
            try
            {
                expenseModel = ExpenseDetailsDataList.Where(c => c.RowId == int.Parse(ExpenseDetailsRowId.Value) && c.Id == int.Parse(ExpenseDetailsId.Value) && c.ExpenseId == selectedRowId).FirstOrDefault();
                if (expenseModel == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('No Record Found to Update.');", true);
                    return null;
                }
                expenseModel.ExpenseId = model.ExpenseId;
                expenseModel.CompanyId = model.CompanyId;
                expenseModel.ProjectId = model.ProjectId;
                expenseModel.TargetDescriptionId = model.TargetDescriptionId;
                expenseModel.Description = model.Description;
                expenseModel.Amount = model.Amount;
                expenseModel.Gst = model.Gst;
                expenseModel.CreatedBy = SessionManager.UserId;
                return ExpenseDetailsDataList;
            }
            finally
            {
                expenseModel = null;
            }
        }
        private List<ExpenseDetailsGridModel> FilledExpenseDetailsModelToGridModel(List<ExpenseDetails> ExpenseModelList)
        {
            List<ExpenseDetailsGridModel> gridModelList = new List<ExpenseDetailsGridModel>();
            ExpenseDetailsGridModel gridModel;
            try
            {
                foreach (var Expense in ExpenseModelList)
                {
                    gridModel = new ExpenseDetailsGridModel();
                    if (Expense.Id > 0)
                    {
                        gridModel.ExpenseId = Expense.ExpenseId;
                        gridModel.Id = Expense.Id;
                        gridModel.DetailsRowId = ExpenseModelList.FindIndex(e => e.Id == Expense.Id) + 1;
                        Expense.RowId = gridModel.DetailsRowId;
                    }
                    else
                    {
                        if (selectedRowId > 0)
                        {
                            gridModel.Id = Expense.Id;
                        }
                        gridModel.DetailsRowId = Expense.RowId;
                    }
                    gridModel.CompanyName = Expense.CompanyName;
                    gridModel.ProjectName = Expense.ProjectName;
                    gridModel.TargetDescription = Expense.TargetDescription;
                    gridModel.Description = Expense.Description;
                    string numericString = new string((Expense.Amount.Where(c => char.IsDigit(c) || c == '.').ToArray()));
                    gridModel.Amount = "$ " + decimal.Parse(numericString).ToString("0.00");
                    gridModel.GST = Expense.Gst + "%";
                    gridModelList.Add(gridModel);
                }
                return gridModelList;
            }
            finally
            {
                gridModelList = null; gridModel = null;
            }
        }

        protected void DeleteExpenseDetails(object sender, EventArgs e)
        {
            selectedDetailsRowId = SelectedDetailsRowIdHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedDetailsRowId))
            {
                popup_confirm_container.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select the Record to Delete.');", true);
                return;
            }
        }
        protected void ConfirmDeleteExpenseDetails(object sender, EventArgs e)
        {
            try
            {
                selectedDetailsRowId = SelectedDetailsRowIdHiddenField.Value;
                selectedDetailsId = SelectedDetailsIdHiddenField.Value;
                int rowId = ExpenseDetailsDataList.FindIndex(c => c.RowId == int.Parse(selectedDetailsRowId) && c.Id == int.Parse(selectedDetailsId) && c.ExpenseId == selectedRowId);
                if (rowId >= 0 && rowId < ExpenseDetailsDataList.Count)
                {
                    ExpenseDetailsDataList.RemoveAt(rowId);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again.');", true);
                    return;
                }
                ExpenseDetailsGridDataList = FilledExpenseDetailsModelToGridModel(ExpenseDetailsDataList);
                if (ExpenseDetailsGridDataList == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Delete.Please try again.');", true);
                    return;
                }
                ExpenseDetailsGridView.DataSource = ExpenseDetailsGridDataList;
                ExpenseDetailsGridView.DataBind();
                popup_confirm_container.Visible = false;
                SelectedDetailsRowIdHiddenField.Value = "";
                SelectedDetailsIdHiddenField.Value = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Delete Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, "");
            }
        }
        protected void btnCancelDeleteExpenseDetails(object sender, EventArgs e)
        {
            popup_confirm_container.Visible = false;
        }
        #endregion
        #region Get Target Details And Show Total Amount And Balance Amount

        protected void GetTargetDetails(object sender, EventArgs e)
        {
            DateTime debitedDate;
            int projectId;
            string msg = "";
            PaymentTargetMasterModel paymentTargetMasterModel;
            List<DropDownItem> targetList = new List<DropDownItem>();
            try
            {
                this.sharedService = new SharedService();
                debitedDate = DateTime.Parse(DebitedDate.Text);
                projectId = int.Parse(ProjectNameDropDownList.SelectedItem.Value);
                if (debitedDate == null || projectId <= 0)
                {
                    targetList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Target-" });
                    TargetDescriptionDropDownList.DataSource = targetList;
                    TargetDescriptionDropDownList.DataValueField = "Key";
                    TargetDescriptionDropDownList.DataTextField = "Value";
                    TargetDescriptionDropDownList.DataBind();
                    TargetDescriptionDropDownList.SelectedValue = "0";
                    TargetDescriptionDropDownList.Attributes["readonly"] = "true";
                    return;
                }
                paymentTargetMasterModel = sharedService.GetTargetDetailsByProjectId(debitedDate, projectId, out msg);
                if (paymentTargetMasterModel == null || !string.IsNullOrWhiteSpace(msg))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Date And Project Name MisMatch.');", true);
                    return;
                }
                targetList.Insert(0, new DropDownItem { Key = paymentTargetMasterModel.PaymentTargetId, Value = paymentTargetMasterModel.PaymentTargetDescription });
                TargetDescriptionDropDownList.DataSource = targetList;
                TargetDescriptionDropDownList.DataValueField = "Key";
                TargetDescriptionDropDownList.DataTextField = "Value";
                TargetDescriptionDropDownList.DataBind();
                TargetDescriptionDropDownList.SelectedValue = paymentTargetMasterModel.PaymentTargetId.ToString();
                TargetDescriptionDropDownList.Attributes["readonly"] = "true";
                Amount.Attributes.Remove("readonly");
                total_target_amount.Visible = true;
                total_target_amount.InnerText = "Total Amount : $" + paymentTargetMasterModel.Amount.ToString("0.00");
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "GetTargetDetails.aspx.cs", ex, "");
            }
            finally { expenseDetailsService = null; paymentTargetMasterModel = null; targetList = null; }
        }
        protected void ShowBalanceTargetAmount(object sender, EventArgs e)
        {
            decimal details_Amount, total_Amount, balance_Amount, details_Added_Amount;
            try
            {
                details_Amount = decimal.Parse(Amount.Text);
                total_Amount = decimal.Parse(total_target_amount.InnerText.Split('$')[1]);
                details_Added_Amount = ExpenseDetailsDataList.Sum(a => decimal.Parse(new string(a.Amount.Where(c => char.IsDigit(c) || c == '.').ToArray())));
                balance_Amount = total_Amount - details_Amount - details_Added_Amount;
                //if (balance_Amount < 0)
                //{
                //    Amount.Text = "";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('No Available Amount.');", true);
                //    return;
                //}
                balance_target_amount.Visible = true;
                balance_target_amount.InnerText = "Balance Amount : $" + balance_Amount.ToString("0.00");
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ShowBalanceTargetAmount.aspx.cs", ex, "");
            }
        }
        #endregion
        #region Show Upload File in edit
        private void ShowUploadFile(string filePath)
        {
            string modelJson = "";
            try
            {
                if (string.IsNullOrWhiteSpace(filePath)) { return; }
                string directoryPath = Path.GetDirectoryName(filePath);
                if (string.IsNullOrWhiteSpace(directoryPath)) { return; }
                string fileName = Path.GetFileName(filePath);
                if (string.IsNullOrWhiteSpace(fileName)) { return; }
                long fileSize = FileHelper.CheckFileIsExist(filePath);
                if (fileSize <= 0)
                {
                    Expensefileupload.Visible = true; FileUploadBtn.Visible = true; upload_fileName_div.Visible = false;
                    return;
                }
                Expense_file_div.Attributes["class"] = "show_uploaded_div";
                SessionManager.ExpenseUploadEDFilePath = filePath;
                Expensefileupload.Visible = false;
                FileUploadBtn.Visible = false;
                Expense_file_div.Style["display"] = "flex";
                upload_fileName_div.Visible = true;
                upload_fileName_div.InnerText = fileName + " (" + (fileSize / 1024.0).ToString("0.00") + " KB)";
                DeleteUploadFileBtn.Visible = true;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(filePath);
                this.logFileService.LogError(SessionManager.UserId, "SHOW UPLOAD FILE", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { modelJson = null; }
        }
        #endregion
        #region Delete Upload File
        protected void DeleteUploadFile(object sender, EventArgs e)
        {
            FileInputParam fileDetails = new FileInputParam();
            string modelJson = "";
            try
            {
                fileDetails.FileName = GetFileName(upload_fileName_div.InnerText);
                fileDetails.BaseDirectory = ConfigurationManager.AppSettings["FileBaseURL"];
                fileDetails.FolderName = ConfigurationManager.AppSettings["FolderPath"];
                fileDetails.DirectoryName = "ExpenseFiles";
                if ((File.Exists(fileDetails.BaseDirectory + "/" + fileDetails.FolderName + "/" + fileDetails.DirectoryName + "/" + fileDetails.FileName)))
                {
                    File.Delete(fileDetails.BaseDirectory + "/" + fileDetails.FolderName + "/" + fileDetails.DirectoryName + "/" + fileDetails.FileName);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('No File Found.');", true);
                    return;
                }
                Expense_file_div.Style["display"] = "none";
                upload_fileName_div.InnerText = "";
                DeleteUploadFileBtn.Visible = true;
                Expense_file_div.Attributes.Remove("show_uploaded_div");
                Expense_file_div.Visible = false;
                DeleteUploadFileBtn.Visible = false;
                Expensefileupload.Visible = true;
                FileUploadBtn.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('File Deleted Successfully.');", true);
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(fileDetails);
                this.logFileService.LogError(SessionManager.UserId, "DELETE UPLOAD FILE", "ExpenseDetailsMaster.aspx.cs", ex, modelJson);
            }
            finally { fileDetails = null; modelJson = null; }
        }
        private string GetFileName(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return "";
                }
                int lastParenIndex = fileName.LastIndexOf('(');
                if (lastParenIndex < 0)
                {
                    return "";
                }

                return fileName.Substring(0, lastParenIndex).Trim();
            }
            finally { }
        }
        protected void ClearUploadFile(object sender, EventArgs e)
        {
            string currentClasses = Expense_file_div.Attributes["class"];
            if (!string.IsNullOrEmpty(currentClasses))
            {
                var classList = currentClasses.Split(' ').ToList();
                classList.Remove("show_uploaded_div");
                Expense_file_div.Attributes["class"] = string.Join(" ", classList);
            }
            upload_fileName_div.InnerText = "";
            Expense_file_div.Visible = false;
            DeleteUploadFileBtn.Visible = false;
            Expensefileupload.Visible = true;
            FileUploadBtn.Visible = true;
        }
        #endregion
        #region Show Upload File In Popup
        protected void ViewUploadFile(object sender, EventArgs e)
        {
            string filePath;
            try
            {
                //filePath = "C:\\Users\\muthu\\OneDrive\\Pictures\\wallpaperflare.com_wallpaper (2) (1).jpg";
                //byte[] imageData = File.ReadAllBytes(filePath);
                filePath = SessionManager.ExpenseUploadEDFilePath;
                //byte[] imageData = FileHelper.DownloadFromFTP(filePath);
                //if (imageData != null)
                //{
                //    string base64String = Convert.ToBase64String(imageData);
                //    string format = GetImageFormat(filePath);
                //    string imageUrl = $"data:image/{format};base64,{base64String}";
                //    popupImage.ImageUrl = imageUrl;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowImagePopup", "showImagePopup();", true);
                //}

                string base64String = FileHelper.DownloadFromFTP(filePath);
                string format = GetImageFormat(filePath);
                string imageUrl = $"data:image/{format};base64,{base64String}";
                popupImage.ImageUrl = imageUrl;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowImagePopup", "showImagePopup();", true);

            }
            finally { filePath = null; }
        }
        private string GetImageFormat(string filePath)
        {
            string extension;
            try
            {
                extension = Path.GetExtension(filePath).ToLower();
                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                        return "jpeg";
                    case ".png":
                        return "png";
                    case ".gif":
                        return "gif";
                    case ".bmp":
                        return "bmp";
                    default:
                        return extension;
                }
            }
            finally { extension = null; }
        }
        #endregion
    }
}