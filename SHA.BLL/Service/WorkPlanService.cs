using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SHA.BLL.Service
{
    public interface IWorkPlanService
    {
        List<DropDownItem> GetTeamSupplyDropDownData();
        List<WorkPlanMasterGridModel> GetWorkPlanGridData();
        long AddWorkPlanDetail(WorkPlanMasterModel model);
        long EditWorkPlanDetail(WorkPlanMasterModel model);
        WorkPlanMasterModel FetchWorkPlanData(string selRowId);
        int DeleteWorkPlanDetail(string selRowId);
    }
    public class WorkPlanService : IWorkPlanService
    {
        private IDBHelper __dbHelper = null;
        public WorkPlanService()
        {
            __dbHelper = new DBHelper();
        }
        public List<DropDownItem> GetTeamSupplyDropDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetTeamSupplyDropDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<WorkPlanMasterGridModel> GetWorkPlanGridData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetWorkPlanGridData", null, out msg);
                return this.__dbHelper.GetDataList<WorkPlanMasterGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public long AddWorkPlanDetail(WorkPlanMasterModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditWorkPlanDetail"))
                {
                    //connection.command.Parameters.AddWithValue("@TeamId", model.TeamId);
                    //connection.command.Parameters.AddWithValue("@TeamHeadName", model.TeamHeadName);
                    //connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@TeamSupplyId ", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt64(outputParam.Value);
                }
            }
            finally { }
        }
        public long EditWorkPlanDetail(WorkPlanMasterModel model)
        {
            try
            {
                if (model == null || model.WorkPlanId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditWorkPlanDetail"))
                {
                    //connection.command.Parameters.AddWithValue("@TeamSupplyId", model.TeamSupplyId);
                    //connection.command.Parameters.AddWithValue("@TeamId", model.TeamId);
                    //connection.command.Parameters.AddWithValue("@TeamHeadName", model.TeamHeadName);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.ExecuteNonQuery();
                    return model.WorkPlanId;
                }
            }
            finally { }
        }
        public WorkPlanMasterModel FetchWorkPlanData(string selRowId)
        {
            List<WorkPlanMasterModel> selWorkPlanRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@WorkPlanId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchWorkPlanData", paramList, out msg);
                selWorkPlanRowData = this.__dbHelper.GetDataList<WorkPlanMasterModel>(dt);
                if (selWorkPlanRowData == null || selWorkPlanRowData.Count == 0) { return null; }
                return selWorkPlanRowData[0];
            }
            finally { dt = null; msg = null; selWorkPlanRowData = null; }
        }
        public int DeleteWorkPlanDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteWorkPlanDetail"))
                {
                    connection.command.Parameters.AddWithValue("@WorkPlanId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
