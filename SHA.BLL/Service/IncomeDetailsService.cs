using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SHA.BLL.Service
{
    public interface IIncomeDetailsService
    {
        List<DropDownItem> GetRecivableAccTypeDrpoDownData();
        List<IncomeDetailsGridModel> GetIncomeDetailsGridData(int userId);
        long AddIncomeDetail(IncomeDetails model);
        long EditIncomeDetail(IncomeDetails model);
        IncomeDetails FetchIncomeDetails(string selRowId);
        long DeleteIncomeDetail(string selRowId);
    }
    public class IncomeDetailsService : IIncomeDetailsService
    {
        private IDBHelper __dbHelper = null;
        public IncomeDetailsService()
        {
            __dbHelper = new DBHelper();
        }
        public List<DropDownItem> GetRecivableAccTypeDrpoDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetRecivableAccTypeDrpoDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<IncomeDetailsGridModel> GetIncomeDetailsGridData(int userId)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@CreatedBy", userId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetIncomeDetailsGridData", paramList, out msg);
                return this.__dbHelper.GetDataList<IncomeDetailsGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public long AddIncomeDetail(IncomeDetails model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditIncomeDetail"))
                {
                    connection.command.Parameters.AddWithValue("@InvoiceId", model.InvoiceId);
                    connection.command.Parameters.AddWithValue("@IsInvoice", model.IsInvoice);
                    connection.command.Parameters.AddWithValue("@InvoiceRefId", model.InvoiceRefId);
                    connection.command.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                    connection.command.Parameters.AddWithValue("@IsEmployee", model.IsEmployee);
                    connection.command.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
                    connection.command.Parameters.AddWithValue("@HasProject", model.HasProject);
                    connection.command.Parameters.AddWithValue("@ProjectId", model.ProjectId);
                    connection.command.Parameters.AddWithValue("@ReceivableAccTypeId", model.ReceivableAccountTypeId);
                    connection.command.Parameters.AddWithValue("@IncomeDescription", model.IncomeDescription);
                    connection.command.Parameters.AddWithValue("@CreditAmount", model.CreditAmount);
                    connection.command.Parameters.AddWithValue("@CreditedDate", model.CreditedDate.ToString("yyyy-MM-dd"));
                    connection.command.Parameters.AddWithValue("@IsGst", model.IsGST);
                    connection.command.Parameters.AddWithValue("@GstRate", model.GstRate);
                    connection.command.Parameters.AddWithValue("@AmountTypeId", model.AmountTypeId);
                    connection.command.Parameters.AddWithValue("@AdminId", model.AdminId);
                    connection.command.Parameters.AddWithValue("@TargetDescriptionId", model.TargetDescriptionId);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@IncomeId ", SqlDbType.BigInt);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt64(outputParam.Value);
                }
            }
            finally { }
        }
        public long EditIncomeDetail(IncomeDetails model)
        {
            try
            {
                if (model == null || model.IncomeId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditIncomeDetail"))
                {
                    connection.command.Parameters.AddWithValue("@IncomeId", model.IncomeId);
                    connection.command.Parameters.AddWithValue("@InvoiceId", model.InvoiceId);
                    connection.command.Parameters.AddWithValue("@IsInvoice", model.IsInvoice);
                    connection.command.Parameters.AddWithValue("@InvoiceRefId", model.InvoiceRefId);
                    connection.command.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                    connection.command.Parameters.AddWithValue("@IsEmployee", model.IsEmployee);
                    connection.command.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
                    connection.command.Parameters.AddWithValue("@HasProject", model.HasProject);
                    connection.command.Parameters.AddWithValue("@ProjectId", model.ProjectId);
                    connection.command.Parameters.AddWithValue("@ReceivableAccTypeId", model.ReceivableAccountTypeId);
                    connection.command.Parameters.AddWithValue("@IncomeDescription", model.IncomeDescription);
                    connection.command.Parameters.AddWithValue("@CreditAmount", model.CreditAmount);
                    connection.command.Parameters.AddWithValue("@CreditedDate", model.CreditedDate.ToString("yyyy-MM-dd"));
                    connection.command.Parameters.AddWithValue("@IsGst", model.IsGST);
                    connection.command.Parameters.AddWithValue("@GstRate", model.GstRate);
                    connection.command.Parameters.AddWithValue("@AmountTypeId", model.AmountTypeId);
                    connection.command.Parameters.AddWithValue("@AdminId", model.AdminId);
                    connection.command.Parameters.AddWithValue("@TargetDescriptionId", model.TargetDescriptionId);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.ExecuteNonQuery();
                    return model.IncomeId;
                }
            }
            finally { }
        }
        public IncomeDetails FetchIncomeDetails(string selRowId)
        {
            List<IncomeDetails> selIncomeDetailRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@IncomeId", long.Parse(selRowId)));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchIncomeDetails", paramList, out msg);
                selIncomeDetailRowData = this.__dbHelper.GetDataList<IncomeDetails>(dt);
                if (selIncomeDetailRowData == null || selIncomeDetailRowData.Count == 0) { return null; }
                return selIncomeDetailRowData[0];
            }
            finally { dt = null; msg = null; }
        }
        public long DeleteIncomeDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteIncomeDetail"))
                {
                    connection.command.Parameters.AddWithValue("@IncomeId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
