using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class PayableAccTypeMaster : System.Web.UI.Page
    {
        #region Common Variables
        private IAccountTypeService accountTypeService = null;
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
                BindPayAccTypeDetailsGrid();
                popup_container.Visible = false;
                popup_confirm_container.Visible = false;
            }
        }
        private void BindPayAccTypeDetailsGrid()
        {
            List<PayableAccTypeGridModel> payAccTypeGridModel;
            try
            {
                this.accountTypeService = new AccountTypeService();
                payAccTypeGridModel = accountTypeService.GetPayAccTypeGridData(SessionManager.UserId);
                if (accountTypeService != null && payAccTypeGridModel.Count > 0) { PayAccTypeGridView.DataSource = payAccTypeGridModel; }
                else
                {
                    PayAccTypeGridView.DataSource = new List<PayableAccTypeGridModel>();
                }
                PayAccTypeGridView.DataBind();
            }
            finally { }
        }
        protected void PayAccTypeGridView_SetDataRowId(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    PayableAccTypeGridModel payableAccType = e.Row.DataItem as PayableAccTypeGridModel;
                    if (payableAccType != null && payableAccType.PayableAccTypeId > 0)
                    {
                        e.Row.Attributes["data-row-id"] = payableAccType.PayableAccTypeId.ToString();
                        e.Row.CssClass = "clickable_row";
                        e.Row.Attributes["onclick"] = "fnPayableAccTypeRowClick(this);";
                    }
                }
            }
            finally { }
        }
        protected void PayAccTypeGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in PayAccTypeGridView.Rows)
                {
                    if (row.RowIndex == PayAccTypeGridView.SelectedIndex)
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
        protected void SavePayAccTypeDetails(object sender, EventArgs e)
        {
            string msg = "";
            string modelJson = "";
            int flag;
            PayableAccTypeModel model = new PayableAccTypeModel();
            try
            {
                model = GetPayAccType();
                msg = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{msg}');", true);
                    return;
                }
                this.accountTypeService = new AccountTypeService();
                if (model.PayableAccTypeId == 0)
                {
                    flag = accountTypeService.AddPayAccType(model);
                }
                else
                {
                    flag = accountTypeService.EditPayAccType(model);
                }
                if (flag > 0)
                {
                    BindPayAccTypeDetailsGrid();
                    popup_container.Visible = false;
                }
                else
                {
                    if (flag == -2) { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Name is Already Exist.');", true); return; }
                    else { ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Problem In Save.Please try again.');", true); return; }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Saved Successfully.');", true); 
                return;
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(model);
                this.logFileService.LogError(SessionManager.UserId, "PAYABLE ACCOUNT TYPE MASTER", "PayableAccTypeMaster.aspx.cs", ex, modelJson);
            }
            finally { model = null; msg = null; this.accountTypeService = null; this.__logFileService = null; }
        }
        private PayableAccTypeModel GetPayAccType()
        {
            PayableAccTypeModel model;
            int payAccTypeValue;
            try
            {
                model = new PayableAccTypeModel();
                model.PayableAccTypeId = int.TryParse(PayableAccId.Value, out payAccTypeValue) ? payAccTypeValue : 0;
                model.PayableAccTypeName = PayAccTypeName.Text.Trim();
                model.PayableAccTypeDescription = PayAccTypeDescription.Text.Trim();
                model.CreatedBy = SessionManager.UserId;
                return model;
            }
            finally { model = null; }
        }
        private string ValidateModel(PayableAccTypeModel model)
        {
            if (string.IsNullOrEmpty(model.PayableAccTypeName)) { return "Please fill Payable Account Type Name."; }
            if (string.IsNullOrEmpty(model.PayableAccTypeDescription)) { return "Please fill Payable Account Type Description."; }
            return "";
        }
        protected void AddPayAccType_Click(object sender, EventArgs e)
        {
            PopupHeaderText.InnerText = "Add Payable AccountType";
            PayableAccId.Value = "0";
            PayAccTypeName.Text = "";
            PayAccTypeDescription.Text = "";
            popup_container.Visible = true;
        }
        protected void EditPayAccType_Click(object sender, EventArgs e)
        {
            PayableAccTypeModel payableAccType;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                PopupHeaderText.InnerText = "Edit Payable AccountType";
                if (!string.IsNullOrEmpty(selectedRowId))
                {
                    payableAccType = new PayableAccTypeModel();
                    this.accountTypeService = new AccountTypeService();
                    payableAccType = accountTypeService.FetchPayAccType(selectedRowId);
                    PayableAccId.Value = payableAccType.PayableAccTypeId.ToString();
                    PayAccTypeName.Text = payableAccType.PayableAccTypeName;
                    PayAccTypeDescription.Text = payableAccType.PayableAccTypeDescription;
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
                this.logFileService.LogError(SessionManager.UserId, "PAYABLE ACCOUNT TYPE MASTER", "PayableAccTypeMaster.aspx.cs", ex, "");
            }
        }
        protected void DeletePayAccType_Click(object sender, EventArgs e)
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
        protected void ConfirmDeletePayAccType_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                selectedRowId = SelectedRowIdHiddenField.Value;
                this.accountTypeService = new AccountTypeService();
                flag = accountTypeService.DeletePayAccType(selectedRowId);
                if (flag > 0)
                {
                    BindPayAccTypeDetailsGrid();
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
                this.logFileService.LogError(SessionManager.UserId, "PAYABLE ACCOUNT TYPE MASTER", "PayableAccTypeMaster.aspx.cs", ex, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            popup_container.Visible = false;
            popup_confirm_container.Visible = false;
        }
    }
}