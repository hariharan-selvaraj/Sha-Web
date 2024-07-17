using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SHA.BLL.Service
{
    public interface ITeamSupplyService
    {
        List<TeamSupplyMasterGridModel> GetTeamSupplyGridData();
        int AddTeamSupplyDetail(TeamSupplyMasterModel model);
        int EditTeamSupplyDetail(TeamSupplyMasterModel model);
        TeamSupplyMasterModel FetchTeamSupplyData(string selRowId);
        int DeleteTeamSupplyDetail(string selRowId);
    }
    public class TeamSupplyService : ITeamSupplyService
    {
        private IDBHelper __dbHelper = null;
        public TeamSupplyService()
        {
            __dbHelper = new DBHelper();
        }
        public List<TeamSupplyMasterGridModel> GetTeamSupplyGridData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetTeamSupplyGridData", null, out msg);
                return this.__dbHelper.GetDataList<TeamSupplyMasterGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public int AddTeamSupplyDetail(TeamSupplyMasterModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditTeamSupplyDetails"))
                {
                    connection.command.Parameters.AddWithValue("@TeamId", model.TeamId);
                    connection.command.Parameters.AddWithValue("@TeamHeadName", model.TeamHeadName);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@TeamSupplyId ", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            finally { }
        }
        public int EditTeamSupplyDetail(TeamSupplyMasterModel model)
        {
            try
            {
                if (model == null || model.TeamSupplyId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditTeamSupplyDetails"))
                {
                    connection.command.Parameters.AddWithValue("@TeamSupplyId", model.TeamSupplyId);
                    connection.command.Parameters.AddWithValue("@TeamId", model.TeamId);
                    connection.command.Parameters.AddWithValue("@TeamHeadName", model.TeamHeadName);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.ExecuteNonQuery();
                    return model.TeamSupplyId;
                }
            }
            finally { }
        }
        public TeamSupplyMasterModel FetchTeamSupplyData(string selRowId)
        {
            List<TeamSupplyMasterModel> selTeamSupplyRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@TeamSupplyId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchTeamSupplyData", paramList, out msg);
                selTeamSupplyRowData = this.__dbHelper.GetDataList<TeamSupplyMasterModel>(dt);
                if (selTeamSupplyRowData == null || selTeamSupplyRowData.Count == 0) { return null; }
                return selTeamSupplyRowData[0];
            }
            finally { dt = null; msg = null; selTeamSupplyRowData = null; }
        }
        public int DeleteTeamSupplyDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteTeamSupplyDetail"))
                {
                    connection.command.Parameters.AddWithValue("@TeamSupplyId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
