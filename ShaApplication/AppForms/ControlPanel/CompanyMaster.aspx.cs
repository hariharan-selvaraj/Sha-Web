using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class CompanyMaster : System.Web.UI.Page
    {
        #region Common Variables
        private ICompanyService companyService = null;
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
                PopulateCompanyTypeDropDownList();
                BindCompanyGrid();
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        private void PopulateCompanyTypeDropDownList()
        {
            List<DropDownItem> companyList = new List<DropDownItem>();
            string modelJson = "";
            try
            {
                this.companyService = new CompanyService();
                companyList = companyService.GetCompanyTypeDrpoDownData();
                companyList.Insert(0, new DropDownItem { Key = 0, Value = "-Select Company-" });
                companyTypeDropDownList.DataSource = companyList;
                companyTypeDropDownList.DataValueField = "Key";
                companyTypeDropDownList.DataTextField = "Value";
                companyTypeDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(companyList);
                this.logFileService.LogError(0, "COMPANY MASTER", "CompanyMaster.aspx.cs", ex, modelJson);
            }
            finally { companyList = null; }
        }
        public void SaveCompanyDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            long flag;
            companyDetails model = new companyDetails();
            try
            {
                model = GetCompanyDetails();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true);
                    return;
                }
                companyService = new CompanyService();
                if (model.CompanyId == 0)
                {
                    flag = companyService.AddCompany(model);
                }
                else
                {
                    flag = companyService.EditCompany(model);
                }
                if (flag > 0)
                {
                    BindCompanyGrid();
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
                this.logFileService.LogError(0, "COMPANY MASTER", "CompanyMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.companyService = null; this.__logFileService = null; }
        }
        private companyDetails GetCompanyDetails()
        {
            companyDetails model;
            long companyIdValue;
            try
            {
                model = new companyDetails();
                model.CompanyId = long.TryParse(companyId.Value, out companyIdValue) ? companyIdValue : 0;
                model.CompanyName = companyName.Text.Trim();
                model.CompanyTypeId = int.Parse(companyTypeDropDownList.SelectedItem.Value);
                model.CompanyUen = companyUen.Text.Trim();
                model.PhoneNo = phNo.Text.Trim();
                model.Address = address.Value.Trim();
                model.EmailAddress = emailAddress.Text.Trim();
                model.PostalCode = postalCode.Text.Trim();
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(companyDetails model)
        {

            if (string.IsNullOrEmpty(model.CompanyName)) { return "Please fill Company Name."; }
            if (model.CompanyTypeId <= 0) { return "Please select a Company Type."; }
            if (string.IsNullOrEmpty(model.PhoneNo)) { return "Please fill Phone Number."; }
            if (string.IsNullOrEmpty(model.Address)) { return "Please fill Address."; }
            if (string.IsNullOrEmpty(model.EmailAddress)) { return "Please fill Email Address."; }
            if (string.IsNullOrEmpty(model.PostalCode)) { return "Please fill Postal Code."; }
            return "";
        }
        private void BindCompanyGrid()
        {
            List<companyGridModel> companiesGridData;
            try
            {
                companyService = new CompanyService();
                companiesGridData = companyService.GetCompanyGridData();
                CompanyGridView.DataSource = companiesGridData;
                CompanyGridView.DataBind();
            }
            finally { companiesGridData = null; }
        }
        private void BindPageNumbers()
        {
            int currentPageIndex = CompanyGridView.PageIndex + 1;
            int totalPages = CompanyGridView.PageCount;

            List<int> pages = new List<int>();

            if (totalPages <= 5)
            {
                for (int i = 1; i <= totalPages; i++)
                {
                    pages.Add(i);
                }
            }
            else
            {
                pages.Add(1);
                int startPage = Math.Max(currentPageIndex - 2, 2);
                int endPage = Math.Min(currentPageIndex + 2, totalPages - 1);

                if (startPage > 2)
                {
                    pages.Add(-1); 
                }

                for (int i = startPage; i <= endPage; i++)
                {
                    pages.Add(i);
                }

                if (endPage < totalPages - 1)
                {
                    pages.Add(-1);
                }
                pages.Add(totalPages);
            }

            Repeater rptPages = (Repeater)CompanyGridView.BottomPagerRow.FindControl("rptPages");
            rptPages.DataSource = pages;
            rptPages.DataBind();

            LinkButton lnkFirst = (LinkButton)CompanyGridView.BottomPagerRow.FindControl("lnkFirst");
            LinkButton lnkPrev = (LinkButton)CompanyGridView.BottomPagerRow.FindControl("lnkPrev");
            LinkButton lnkNext = (LinkButton)CompanyGridView.BottomPagerRow.FindControl("lnkNext");
            LinkButton lnkLast = (LinkButton)CompanyGridView.BottomPagerRow.FindControl("lnkLast");

            lnkFirst.Enabled = lnkPrev.Enabled = CompanyGridView.PageIndex > 0;
            lnkNext.Enabled = lnkLast.Enabled = CompanyGridView.PageIndex < CompanyGridView.PageCount - 1;
        }


        protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int page = (int)e.Item.DataItem;
                LinkButton lnkPage = (LinkButton)e.Item.FindControl("lnkPage");

                if (page == -1)
                {
                    lnkPage.Text = "...";
                    lnkPage.Enabled = false;
                }
                else
                {
                    lnkPage.Text = page.ToString();
                    if (page == CompanyGridView.PageIndex + 1)
                    {
                        lnkPage.Font.Bold = true;
                        lnkPage.Enabled = false;
                    }
                }
            }
        }


        protected void rptPages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                int pageIndex;
                if (int.TryParse(e.CommandArgument.ToString(), out pageIndex))
                {
                    CompanyGridView.PageIndex = pageIndex - 1;
                }
                else
                {
                    switch (e.CommandArgument.ToString().ToLower())
                    {
                        case "first":
                            CompanyGridView.PageIndex = 0;
                            break;
                        case "prev":
                            CompanyGridView.PageIndex = CompanyGridView.PageIndex - 1;
                            break;
                        case "next":
                            CompanyGridView.PageIndex = CompanyGridView.PageIndex + 1;
                            break;
                        case "last":
                            CompanyGridView.PageIndex = CompanyGridView.PageCount - 1;
                            break;
                    }
                }
                BindCompanyGrid();
            }
        }
        protected void CompanyGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex == -1)
            {
                switch (((LinkButton)sender).CommandArgument.ToLower())
                {
                    case "first":
                        CompanyGridView.PageIndex = 0;
                        break;
                    case "prev":
                        CompanyGridView.PageIndex = CompanyGridView.PageIndex - 1;
                        break;
                    case "next":
                        CompanyGridView.PageIndex = CompanyGridView.PageIndex + 1;
                        break;
                    case "last":
                        CompanyGridView.PageIndex = CompanyGridView.PageCount - 1;
                        break;
                }
            }
            else
            {
                CompanyGridView.PageIndex = e.NewPageIndex;
            }
            BindCompanyGrid();
        }
        protected void CompanyGridView_DataBound(object sender, EventArgs e)
        {
            BindPageNumbers();
        }
        protected void SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    companyGridModel company = e.Row.DataItem as companyGridModel;
                    if (company != null && company.CompanyId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = company.CompanyId.ToString();
                        //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(CompanyGridView, "Select$" + e.Row.RowIndex);
                        //e.Row.Attributes["style"] = "cursor: pointer;";
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "companyRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void CompanyGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in CompanyGridView.Rows)
                {
                    if (row.RowIndex == CompanyGridView.SelectedIndex)
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
        protected void AddCompany_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add Company";
            companyId.Value = "0";
            companyName.Text = "";
            companyTypeDropDownList.SelectedValue = "0";
            companyUen.Text = "";
            phNo.Text = "";
            address.Value = "";
            emailAddress.Text = "";
            postalCode.Text = "";
            popup_container.Visible = true;
        }
        protected void EditCompany_Click(object sender, EventArgs e)
        {
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                PopupHeaderText.InnerText = "Edit Company";
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    companyService = new CompanyService();
                    companyDetails selCompanyRowData = companyService.FetchCompanyDetails(selectedRowId);
                    companyId.Value = selCompanyRowData.CompanyId.ToString();
                    companyName.Text = selCompanyRowData.CompanyName;
                    companyTypeDropDownList.SelectedValue = selCompanyRowData.CompanyTypeId.ToString();
                    companyUen.Text = selCompanyRowData.CompanyUen;
                    phNo.Text = selCompanyRowData.PhoneNo;
                    address.Value = selCompanyRowData.Address;
                    emailAddress.Text = selCompanyRowData.EmailAddress;
                    postalCode.Text = selCompanyRowData.PostalCode;
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
                this.logFileService.LogError(0, "COMPANY MASTER", "CompanyMaster.aspx.cs", ex, "");
            }
        }
        protected void DeleteCompany_Click(object sender, EventArgs e)
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
        protected void ConfirmDeleteCompany_Click(object sender, EventArgs e)
        {
            long flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                companyService = new CompanyService();
                flag = companyService.DeleteCompany(selectedRowId);
                if (flag > 0)
                {
                    BindCompanyGrid();
                    popup_confirm_container.Visible = false;
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
                this.logFileService.LogError(0, "COMPANY MASTER", "CompanyMaster.aspx.cs", ex, "");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //popup_container.Style["display"] = "none";
            //string script = @"<script type='text/javascript'>document.getElementById('popup_container').style.display = 'none';</script>";
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
    }
}