using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SHA.BLL.Service
{
    public interface ITimeSheetService
    {
        List<DropDownItem> GetPhotoTypeDropDownData();
        List<TimeSheetGridModel> GetTimeSheetGridData(int userId,DateTime fromDate,DateTime toDate);
    }
    public class TimeSheetService : ITimeSheetService
    {
        private IDBHelper __dbHelper = null;
        public TimeSheetService()
        {
            __dbHelper = new DBHelper();
        }
        public List<DropDownItem> GetPhotoTypeDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetPhotoTypeDropDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<TimeSheetGridModel> GetTimeSheetGridData(int userId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@CreatedBy", userId));
                paramList.Add(new SqlParameter("@FromDate", fromDate));
                paramList.Add(new SqlParameter("@ToDate", toDate));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetTimeSheetGridData", paramList, out msg);
                return this.__dbHelper.GetDataList<TimeSheetGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public int AddPaymentTargetDetail(PaymentTargetMasterModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditPaymentTargetDetail"))
                {
                    connection.command.Parameters.AddWithValue("@ProjectId", model.ProjectId);
                    connection.command.Parameters.AddWithValue("@PaymentTargetType", model.PaymentTargetType);
                    connection.command.Parameters.AddWithValue("@PaymentTargetFromDate", model.PaymentTargetFromDate);
                    connection.command.Parameters.AddWithValue("@PaymentTargetToDate", model.PaymentTargetToDate);
                    connection.command.Parameters.AddWithValue("@Amount", model.Amount);
                    connection.command.Parameters.AddWithValue("@PaymentTargetDescription", model.PaymentTargetDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@PaymentTargetId ", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            finally { }
        }
        public int EditPaymentTargetDetail(PaymentTargetMasterModel model)
        {
            try
            {
                if (model == null || model.PaymentTargetId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditPaymentTargetDetail"))
                {
                    connection.command.Parameters.AddWithValue("@PaymentTargetId", model.PaymentTargetId);
                    connection.command.Parameters.AddWithValue("@ProjectId", model.ProjectId);
                    connection.command.Parameters.AddWithValue("@PaymentTargetType", model.PaymentTargetType);
                    connection.command.Parameters.AddWithValue("@PaymentTargetFromDate", model.PaymentTargetFromDate);
                    connection.command.Parameters.AddWithValue("@PaymentTargetToDate", model.PaymentTargetToDate);
                    connection.command.Parameters.AddWithValue("@Amount", model.Amount);
                    connection.command.Parameters.AddWithValue("@PaymentTargetDescription", model.PaymentTargetDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.ExecuteNonQuery();
                    return model.PaymentTargetId;
                }
            }
            finally { }
        }
        public PaymentTargetMasterModel FetchPaymentTargetData(string selRowId)
        {
            List<PaymentTargetMasterModel> selPaymentTargetRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@PaymentTargetId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchPaymentTargetData", paramList, out msg);
                selPaymentTargetRowData = this.__dbHelper.GetDataList<PaymentTargetMasterModel>(dt);
                if (selPaymentTargetRowData == null || selPaymentTargetRowData.Count == 0) { return null; }
                return selPaymentTargetRowData[0];
            }
            finally { dt = null; msg = null; selPaymentTargetRowData = null; }
        }
        public int DeletePaymentTargetDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeletePaymentTargetDetail"))
                {
                    connection.command.Parameters.AddWithValue("@PaymentTargetId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
