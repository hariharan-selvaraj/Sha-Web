using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class ExpenseDetailsMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IExpenseDetailsService expenseDetailsService = null;
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
                string message = Request.QueryString["message"];
                if (!string.IsNullOrEmpty(message))
                {
                    message = HttpUtility.UrlDecode(message);
                    string script = $@"
                <script type='text/javascript'>
                    alert('{message}');
                    window.history.replaceState(null, null, window.location.pathname);
                </script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", script, false);
                }
                BindExpenseDetailsGrid();
                popup_confirm_container.Visible = false;
            }
        }

        private void BindExpenseDetailsGrid()
        {
            List<ExpenseHeaderGridModel> ExpenseDetailsGridData;
            try
            {
                this.expenseDetailsService = new ExpenseDetailsService();
                ExpenseDetailsGridData = expenseDetailsService.GetExpenseHeaderGridData(SessionManager.UserId);
                if (ExpenseDetailsGridData != null && ExpenseDetailsGridData.Count > 0) { ExpenseGridView.DataSource = ExpenseDetailsGridData; }
                else
                {
                    ExpenseGridView.DataSource = new List<ExpenseHeaderGridModel>();
                }
                ExpenseGridView.DataBind();
            }
            finally { }
        }
        protected void ExpenseGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ExpenseHeaderGridModel expenseDetail = e.Row.DataItem as ExpenseHeaderGridModel;
                    if (expenseDetail != null && expenseDetail.ExpenseId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = expenseDetail.ExpenseId.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnExpenseDetailRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void ExpenseGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in ExpenseGridView.Rows)
                {
                    if (row.RowIndex == ExpenseGridView.SelectedIndex)
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
        protected void AddExpense_Click(object sender, EventArgs e)
        {
            Session["selectedRowId"] = "";
            string redirectUrl = WebHelper.GetNavigationUrl("AddExpense.aspx");
            Response.Redirect(redirectUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        protected void EditExpense_Click(object sender, EventArgs e)
        {
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    Session["selectedRowId"] = selectedRowId;
                    string redirectUrl = WebHelper.GetNavigationUrl("AddExpense.aspx");
                    Response.Redirect(redirectUrl, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
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
        }
        protected void DeleteExpense_Click(object sender, EventArgs e)
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
        protected void ConfirmDeleteExpense_Click(object sender, EventArgs e)
        {
            long flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.expenseDetailsService = new ExpenseDetailsService();
                flag = expenseDetailsService.DeleteExpenseDetail(selectedRowId);
                if (flag > 0)
                {
                    BindExpenseDetailsGrid();
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
                this.logFileService.LogError(SessionManager.UserId, "EXPENSEDETAILS MASTER", "ExpenseDetailsMaster.aspx.cs", ex, "");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            popup_confirm_container.Visible = false;
        }
    }
}