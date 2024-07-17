using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SHA.BLL.Service
{
    public interface IAccountTypeService
    {
        List<RecievableAccTypeGridModel> GetRecAccTypeGridData(int userId);
        int AddRecAccType(RecievableAccTypeModel model);
        int EditRecAccType(RecievableAccTypeModel model);
        RecievableAccTypeModel FetchRecAccType(string selRowId);
        int DeleteRecAccType(string selRowId);
        List<PayableAccTypeGridModel> GetPayAccTypeGridData(int userId);
        int AddPayAccType(PayableAccTypeModel model);
        int EditPayAccType(PayableAccTypeModel model);
        PayableAccTypeModel FetchPayAccType(string selRowId);
        int DeletePayAccType(string selRowId);
    }
    public class AccountTypeService : IAccountTypeService
    {
        private IDBHelper __dbHelper = null;
        public AccountTypeService()
        {
            __dbHelper = new DBHelper();
        }
        public List<RecievableAccTypeGridModel> GetRecAccTypeGridData(int userId)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@CreatedBy", userId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetRecAccTypeGridData", paramList, out msg);
                return this.__dbHelper.GetDataList<RecievableAccTypeGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public int AddRecAccType(RecievableAccTypeModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditRecAccType"))
                {
                    connection.command.Parameters.AddWithValue("@RecAccTypeName", model.RecivableAccTypeName);
                    connection.command.Parameters.AddWithValue("@RecAccTypeDescription", model.RecivableAccTypeDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@RecAccTypeId ", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            finally { }
        }
        public int EditRecAccType(RecievableAccTypeModel model)
        {
            try
            {
                if (model == null || model.RecivableAccTypeId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditRecAccType"))
                {
                    connection.command.Parameters.AddWithValue("@RecAccTypeId", model.RecivableAccTypeId);
                    connection.command.Parameters.AddWithValue("@RecAccTypeName", model.RecivableAccTypeName);
                    connection.command.Parameters.AddWithValue("@RecAccTypeDescription", model.RecivableAccTypeDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.ExecuteNonQuery();
                    return model.RecivableAccTypeId;
                }
            }
            finally { }
        }
        public RecievableAccTypeModel FetchRecAccType(string selRowId)
        {
            List<RecievableAccTypeModel> selRecAccTypeRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@RecAccTypeId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchRecAccType", paramList, out msg);
                selRecAccTypeRowData = this.__dbHelper.GetDataList<RecievableAccTypeModel>(dt);
                if (selRecAccTypeRowData == null || selRecAccTypeRowData.Count == 0) { return null; }
                return selRecAccTypeRowData[0];
            }
            finally { dt = null; msg = null; }
        }
        public int DeleteRecAccType(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteRecAccType"))
                {
                    connection.command.Parameters.AddWithValue("@RecAccTypeId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
        public List<PayableAccTypeGridModel> GetPayAccTypeGridData(int userId)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@CreatedBy", userId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetPayAccTypeGridData", paramList, out msg);
                return this.__dbHelper.GetDataList<PayableAccTypeGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public int AddPayAccType(PayableAccTypeModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditPayAccType"))
                {
                    connection.command.Parameters.AddWithValue("@payAccTypeName", model.PayableAccTypeName);
                    connection.command.Parameters.AddWithValue("@PayAccTypeDescription", model.PayableAccTypeDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@PayAccTypeId ", SqlDbType.BigInt);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            finally { }
        }
        public int EditPayAccType(PayableAccTypeModel model)
        {
            try
            {
                if (model == null || model.PayableAccTypeId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditPayAccType"))
                {
                    connection.command.Parameters.AddWithValue("@PayAccTypeId", model.PayableAccTypeId);
                    connection.command.Parameters.AddWithValue("@payAccTypeName", model.PayableAccTypeName);
                    connection.command.Parameters.AddWithValue("@PayAccTypeDescription", model.PayableAccTypeDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.ExecuteNonQuery();
                    return model.PayableAccTypeId;
                }
            }
            finally { }
        }
        public PayableAccTypeModel FetchPayAccType(string selRowId)
        {
            List<PayableAccTypeModel> selPayAccTypeRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@PayAccTypeId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchPayAccType", paramList, out msg);
                selPayAccTypeRowData = this.__dbHelper.GetDataList<PayableAccTypeModel>(dt);
                if (selPayAccTypeRowData == null || selPayAccTypeRowData.Count == 0) { return null; }
                return selPayAccTypeRowData[0];
            }
            finally { dt = null; msg = null; }
        }
        public int DeletePayAccType(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeletePayAccType"))
                {
                    connection.command.Parameters.AddWithValue("@PayAccTypeId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
