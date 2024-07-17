using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SHA.BLL.Service
{
    public interface IPaymentTargetService
    {
        List<PaymentTargetMasterGridModel> GetPaymentTargetGridData(int userId);
        int AddPaymentTargetDetail(PaymentTargetMasterModel model);
        int EditPaymentTargetDetail(PaymentTargetMasterModel model);
        PaymentTargetMasterModel FetchPaymentTargetData(string selRowId);
        int DeletePaymentTargetDetail(string selRowId);
    }
    public class PaymentTargetService : IPaymentTargetService
    {
        private IDBHelper __dbHelper = null;
        public PaymentTargetService()
        {
            __dbHelper = new DBHelper();
        }
        public List<PaymentTargetMasterGridModel> GetPaymentTargetGridData(int userId)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@CreatedBy", userId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetPaymentTargetGridData", paramList, out msg);
                return this.__dbHelper.GetDataList<PaymentTargetMasterGridModel>(dt);
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
