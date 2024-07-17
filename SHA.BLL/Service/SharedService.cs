using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SHA.BLL.Service
{
    public interface ISharedService
    {
        List<DropDownItem> GetCompanyDropDownData();
        List<DropDownItem> GetProjectDropDownData();
        List<DropDownItem> GetAmountTypeDropDownData();
        List<DropDownItem> GetGstRateDropDownData();
        List<DropDownItem> GetAdminDropDownData();
        List<DropDownItem> GetPaymentTargetDropDownData();
        PaymentTargetMasterModel GetTargetDetailsByProjectId(DateTime date, int projectId, out string msg);
    }
    public class SharedService : ISharedService
    {
        private IDBHelper __dbHelper = null;
        public SharedService()
        {
            __dbHelper = new DBHelper();
        }
        public List<DropDownItem> GetCompanyDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetCompanyDropDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<DropDownItem> GetProjectDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetProjectDropDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<DropDownItem> GetAmountTypeDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetAmountTypeDrpoDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<DropDownItem> GetGstRateDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetGstRateDropDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<DropDownItem> GetAdminDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetAdminDrpoDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<DropDownItem> GetPaymentTargetDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetPaymentTargetDropDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public PaymentTargetMasterModel GetTargetDetailsByProjectId(DateTime date, int projectId, out string msg)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            List<PaymentTargetMasterModel> targetModelList;
            try
            {
                msg = "";
                paramList.Add(new SqlParameter("@Date", date));
                paramList.Add(new SqlParameter("@ProjectId", projectId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetTargetDetailsByProjectId", paramList, out msg);
                targetModelList = this.__dbHelper.GetDataList<PaymentTargetMasterModel>(dt);
                if (targetModelList == null || targetModelList.Count == 0) { return null; }
                return targetModelList[0];
            }
            catch (Exception ex)
            {
                msg = (ex.Message ?? "") + (ex.InnerException != null ? ((ex.InnerException.Message ?? "") + (ex.InnerException.StackTrace ?? "")) : (ex.StackTrace ?? ""));
                return null;
            }
            finally { dt = null; msg = null; paramList = null; targetModelList = null; }
        }
    }
}
