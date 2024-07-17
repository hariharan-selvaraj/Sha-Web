using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SHA.BLL.Utility;

namespace SHA.BLL.Service
{
    public interface IExpenseDetailsService
    {
        List<DropDownItem> GetPayableAccTypeDropDownData();
        List<ExpenseHeaderGridModel> GetExpenseHeaderGridData(int userId);
        List<ExpenseDetails> GetExpenseOtherDetailsGridData(long selRowId);
        long AddExpenseDetail(ExpenseHeader model);
        long EditExpenseDetail(ExpenseHeader model);
        ExpenseHeader FetchExpenseHeader(long selRowId);
        long DeleteExpenseDetail(string selRowId);
    }
    public class ExpenseDetailsService : IExpenseDetailsService
    {
        private IDBHelper __dbHelper = null;
        public ExpenseDetailsService()
        {
            __dbHelper = new DBHelper();
        }
        public List<DropDownItem> GetPayableAccTypeDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetPayableAccTypeDrpoDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<ExpenseHeaderGridModel> GetExpenseHeaderGridData(int userId)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@CreatedBy", userId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetExpenseHeaderGridData", paramList, out msg);
                return this.__dbHelper.GetDataList<ExpenseHeaderGridModel>(dt);
            }
            finally { dt = null; msg = null; paramList = null; }
        }
        public List<ExpenseDetails> GetExpenseOtherDetailsGridData(long selRowId)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@ExpenseId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchExpenseDetails", paramList, out msg);
                return this.__dbHelper.GetDataList<ExpenseDetails>(dt);
            }
            finally { dt = null; msg = null; paramList = null; }
        }
        public long AddExpenseDetail(ExpenseHeader model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditExpenseDetail"))
                {
                    connection.command.Parameters.AddWithValue("@IsInvoice", model.IsInvoice);
                    connection.command.Parameters.AddWithValue("@InvoiceId", model.InvoiceId);
                    connection.command.Parameters.AddWithValue("@InvoiceRefId", model.InvoiceRefId);
                    connection.command.Parameters.AddWithValue("@SupplierId", model.SupplierId);
                    connection.command.Parameters.AddWithValue("@ExpenseDescription", model.ExpenseDescription);
                    connection.command.Parameters.AddWithValue("@DebitAmount", model.DebitOverAllAmount);
                    connection.command.Parameters.AddWithValue("@DebitedDate", model.DebitedDate);
                    connection.command.Parameters.AddWithValue("@IsGst", model.IsGST);
                    connection.command.Parameters.AddWithValue("@GST", model.Header_GST);
                    connection.command.Parameters.AddWithValue("@IsEmployee", model.IsEmployee);
                    connection.command.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
                    connection.command.Parameters.AddWithValue("@AccTypeId", model.PayableAccTypeId);
                    connection.command.Parameters.AddWithValue("@AmountTypeId", model.AmountTypeId);
                    connection.command.Parameters.AddWithValue("@PaidById", model.PaidBy);
                    connection.command.Parameters.AddWithValue("@ReceivedId", model.ReceivedBy);
                    connection.command.Parameters.AddWithValue("@ExpenseFile", model.ExpenseFile);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    DataTable otherDetailsTable = new DataTable("ExpenseDetailType");
                    otherDetailsTable.Columns.Add("ExpenseDetailId", typeof(int));
                    otherDetailsTable.Columns.Add("CompanyId", typeof(int));
                    otherDetailsTable.Columns.Add("ProjectId", typeof(int));
                    otherDetailsTable.Columns.Add("TargetDescriptionId", typeof(int));
                    otherDetailsTable.Columns.Add("Description", typeof(string));
                    otherDetailsTable.Columns.Add("Amount", typeof(decimal));
                    otherDetailsTable.Columns.Add("Gst", typeof(int));
                    foreach (var item in model.ExpenseOtherDetails)
                    {
                        DataRow row = otherDetailsTable.NewRow();
                        row["ExpenseDetailId"] = item.Id;
                        row["CompanyId"] = item.CompanyId;
                        row["ProjectId"] = item.ProjectId;
                        row["TargetDescriptionId"] = item.TargetDescriptionId;
                        row["Description"] = item.Description;
                        row["Amount"] = item.Amount;
                        row["Gst"] = item.Gst;
                        otherDetailsTable.Rows.Add(row);
                    }
                    SqlParameter otherDetailsParam = connection.command.Parameters.AddWithValue("@ExpenseOtherDetails", otherDetailsTable);
                    otherDetailsParam.SqlDbType = SqlDbType.Structured;
                    SqlParameter outputParam = connection.command.Parameters.Add("@ExpenseId ", SqlDbType.BigInt);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt64(outputParam.Value);
                }
            }
            finally { }
        }
        public long EditExpenseDetail(ExpenseHeader model)
        {
            try
            {
                if (model == null || model.ExpenseId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditExpenseDetail"))
                {
                    connection.command.Parameters.AddWithValue("@ExpenseId", model.ExpenseId);
                    connection.command.Parameters.AddWithValue("@IsInvoice", model.IsInvoice);
                    connection.command.Parameters.AddWithValue("@InvoiceId", model.InvoiceId);
                    connection.command.Parameters.AddWithValue("@InvoiceRefId", model.InvoiceRefId);
                    connection.command.Parameters.AddWithValue("@SupplierId", model.SupplierId);
                    connection.command.Parameters.AddWithValue("@ExpenseDescription", model.ExpenseDescription);
                    connection.command.Parameters.AddWithValue("@DebitAmount", model.DebitOverAllAmount);
                    connection.command.Parameters.AddWithValue("@DebitedDate", model.DebitedDate);
                    connection.command.Parameters.AddWithValue("@IsGst", model.IsGST);
                    connection.command.Parameters.AddWithValue("@GST", model.Header_GST);
                    connection.command.Parameters.AddWithValue("@IsEmployee", model.IsEmployee);
                    connection.command.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
                    connection.command.Parameters.AddWithValue("@AccTypeId", model.PayableAccTypeId);
                    connection.command.Parameters.AddWithValue("@AmountTypeId", model.AmountTypeId);
                    connection.command.Parameters.AddWithValue("@PaidById", model.PaidBy);
                    connection.command.Parameters.AddWithValue("@ReceivedId", model.ReceivedBy);
                    connection.command.Parameters.AddWithValue("@ExpenseFile", model.ExpenseFile);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    DataTable otherDetailsTable = new DataTable("ExpenseDetailType");
                    otherDetailsTable.Columns.Add("ExpenseDetailId", typeof(int));
                    otherDetailsTable.Columns.Add("CompanyId", typeof(int));
                    otherDetailsTable.Columns.Add("ProjectId", typeof(int));
                    otherDetailsTable.Columns.Add("TargetDescriptionId", typeof(int));
                    otherDetailsTable.Columns.Add("Description", typeof(string));
                    otherDetailsTable.Columns.Add("Amount", typeof(decimal));
                    otherDetailsTable.Columns.Add("Gst", typeof(int));
                    foreach (var item in model.ExpenseOtherDetails)
                    {
                        DataRow row = otherDetailsTable.NewRow();
                        row["ExpenseDetailId"] = item.Id;
                        row["CompanyId"] = item.CompanyId;
                        row["ProjectId"] = item.ProjectId;
                        row["TargetDescriptionId"] = item.TargetDescriptionId;
                        row["Description"] = item.Description;
                        row["Amount"] = item.Amount;
                        row["Gst"] = item.Gst;
                        otherDetailsTable.Rows.Add(row);
                    }
                    SqlParameter otherDetailsParam = connection.command.Parameters.AddWithValue("@ExpenseOtherDetails", otherDetailsTable);
                    otherDetailsParam.SqlDbType = SqlDbType.Structured;
                    connection.command.ExecuteNonQuery();
                    return model.ExpenseId;
                }
            }
            finally { }
        }
        public ExpenseHeader FetchExpenseHeader(long selRowId)
        {
            List<ExpenseHeader> selExpenseDetailRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@ExpenseId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchExpenseHeader", paramList, out msg);
                selExpenseDetailRowData = this.__dbHelper.GetDataList<ExpenseHeader>(dt);
                if (selExpenseDetailRowData == null || selExpenseDetailRowData.Count == 0) { return null; }
                return selExpenseDetailRowData[0];
            }
            finally { dt = null; msg = null; selExpenseDetailRowData = null; paramList = null; }
        }
        public long DeleteExpenseDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteExpenseDetail"))
                {
                    connection.command.Parameters.AddWithValue("@ExpenseId", long.Parse(selRowId));
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
