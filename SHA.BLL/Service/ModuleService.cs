using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SHA.BLL.Service
{
    public interface IModuleService
    {
        List<ModuleMasterGridModel> GetModuleMasterGridData();
        int AddModuleDetail(ModuleMasterGridModel model);
        int EditModuleDetail(ModuleMasterGridModel model);
        ModuleMasterGridModel FetchModuleDetails(string selRowId);
        int DeleteModuleDetail(string selRowId);
        List<DropDownItem> GetModuleDrpoDownData();
        List<MenuItemDetailsGridModel> GetMenuItemGridData();
        bool HasDuplicateUrl(int MenuItemId, string taskurl);
        int AddMenuItem(MenuItemDetailsModel model);
        int EditMenuItem(MenuItemDetailsModel model);
        MenuItemDetailsModel FetchMenuItemData(string selRowId);
        int DeleteMenuItemDetail(string selRowId);
        List<DropDownItem> GetMenuItemChkBoxData(string selectedModuleId);
        List<MenuAccessDetailsGridModel> GetMenuAccessGridData(int userId);
        int SaveMenuAccess(MenuAccessDetailsModel model);
        List<MenuAccessDetailsModel> FetchMenuAccessData(string AdminId,string ModuleId);
        int DeleteMenuAccessDetail(string selRowId);
    }
    public class ModuleService : IModuleService
    {
        private IDBHelper __dbHelper = null;
        public ModuleService()
        {
            __dbHelper = new DBHelper();
        }
        public List<ModuleMasterGridModel> GetModuleMasterGridData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetModuleMasterGridData", null, out msg);
                return this.__dbHelper.GetDataList<ModuleMasterGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public int AddModuleDetail(ModuleMasterGridModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditModule"))
                {
                    connection.command.Parameters.AddWithValue("@ModuleName", model.ModuleName);
                    connection.command.Parameters.AddWithValue("@ModuleIcon", model.ModuleIcon);
                    connection.command.Parameters.AddWithValue("@ModuleDescription", model.ModuleDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@ModuleId ", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            finally { }
        }
        public int EditModuleDetail(ModuleMasterGridModel model)
        {
            try
            {
                if (model == null || model.ModuleId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditModule"))
                {
                    connection.command.Parameters.AddWithValue("@ModuleName", model.ModuleName);
                    connection.command.Parameters.AddWithValue("@ModuleIcon", model.ModuleIcon);
                    connection.command.Parameters.AddWithValue("@ModuleDescription", model.ModuleDescription);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.Parameters.AddWithValue("@ModuleId", model.ModuleId);
                    connection.command.ExecuteNonQuery();
                    return model.ModuleId;
                }
            }
            finally { }
        }
        public ModuleMasterGridModel FetchModuleDetails(string selRowId)
        {
            List<ModuleMasterGridModel> selModuleDetailRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@ModuleId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchModuleData", paramList, out msg);
                selModuleDetailRowData = this.__dbHelper.GetDataList<ModuleMasterGridModel>(dt);
                if (selModuleDetailRowData == null || selModuleDetailRowData.Count == 0) { return null; }
                return selModuleDetailRowData[0];
            }
            finally { dt = null; msg = null; }
        }
        public int DeleteModuleDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteModule"))
                {
                    connection.command.Parameters.AddWithValue("@ModuleId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
        public List<DropDownItem> GetModuleDrpoDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetModuleDrpoDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<MenuItemDetailsGridModel> GetMenuItemGridData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetMenuItemGridData", null, out msg);
                return this.__dbHelper.GetDataList<MenuItemDetailsGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public bool HasDuplicateUrl(int MenuItemId, string taskurl)
        {
            List<SqlParameter> spParam;
            string msg = "";
            int flag = 0;
            try
            {
                spParam = new List<SqlParameter>();
                spParam.Add(new SqlParameter("@MenuItemId", MenuItemId));
                spParam.Add(new SqlParameter("@TaskUrl", taskurl));
                flag = this.__dbHelper.ExecuteProc("IsDuplicateTaskUrl", spParam, out msg);
                return (flag > 0);
            }
            finally { spParam = null; msg = null; }
        }
        public int AddMenuItem(MenuItemDetailsModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditMenuItem"))
                {
                    connection.command.Parameters.AddWithValue("@ModuleMasterId", model.ModuleMasterId);
                    connection.command.Parameters.AddWithValue("@MenuItemName", model.MenuItemName);
                    connection.command.Parameters.AddWithValue("@TaskURL", model.TaskURL);
                    connection.command.Parameters.AddWithValue("@MenuItemDescription", model.MenuItemDescription);
                    connection.command.Parameters.AddWithValue("@MenuItemIcon", model.MenuItemIcon);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    SqlParameter outputParam = connection.command.Parameters.Add("@MenuItemId ", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            finally { }
        }
        public int EditMenuItem(MenuItemDetailsModel model)
        {
            try
            {
                if (model == null || model.MenuItemId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditMenuItem"))
                {
                    connection.command.Parameters.AddWithValue("@ModuleMasterId", model.ModuleMasterId);
                    connection.command.Parameters.AddWithValue("@MenuItemName", model.MenuItemName);
                    connection.command.Parameters.AddWithValue("@TaskURL", model.TaskURL);
                    connection.command.Parameters.AddWithValue("@MenuItemDescription", model.MenuItemDescription);
                    connection.command.Parameters.AddWithValue("@MenuItemIcon", model.MenuItemIcon);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    connection.command.Parameters.AddWithValue("@MenuItemId", model.MenuItemId);
                    connection.command.ExecuteNonQuery();
                    return model.MenuItemId;
                }
            }
            finally { }
        }
        public MenuItemDetailsModel FetchMenuItemData(string selRowId)
        {
            List<MenuItemDetailsModel> selMenuItemDetailRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@MenuItemId", selRowId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchMenuItemData", paramList, out msg);
                selMenuItemDetailRowData = this.__dbHelper.GetDataList<MenuItemDetailsModel>(dt);
                if (selMenuItemDetailRowData == null || selMenuItemDetailRowData.Count == 0) { return null; }
                return selMenuItemDetailRowData[0];
            }
            finally { dt = null; msg = null; }
        }
        public int DeleteMenuItemDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteMenuItem"))
                {
                    connection.command.Parameters.AddWithValue("@MenuItemId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
        public List<DropDownItem> GetMenuItemChkBoxData(string selectedModuleId)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@ModuleId", selectedModuleId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetMenuItemChkBoxData", paramList, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public List<MenuAccessDetailsGridModel> GetMenuAccessGridData(int userId)
        {
            DataTable dt;
            List<SqlParameter> paramList = new List<SqlParameter>();
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@AdminId", userId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetMenuAccessGridData", paramList, out msg);
                return this.__dbHelper.GetDataList<MenuAccessDetailsGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public int SaveMenuAccess(MenuAccessDetailsModel model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditMenuAccess"))
                {
                    connection.command.Parameters.AddWithValue("@AdminId", model.AdminId);
                    connection.command.Parameters.AddWithValue("@ModuleId", model.ModuleId);
                    connection.command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    DataTable menuItemType = new DataTable("MenuItemType");
                    menuItemType.Columns.Add("MenuItemId", typeof(int));
                    foreach (var item in model.SelectedMenuItems)
                    {
                        DataRow row = menuItemType.NewRow();
                        row["MenuItemId"] = item;
                        menuItemType.Rows.Add(row);
                    }
                    SqlParameter selChkBoxIds = connection.command.Parameters.AddWithValue("@MenuItemIds", menuItemType);
                    selChkBoxIds.SqlDbType = SqlDbType.Structured;
                    SqlParameter outputParam = connection.command.Parameters.Add("@StatusFlag ", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            finally { }
        }
        public List<MenuAccessDetailsModel> FetchMenuAccessData(string AdminId, string ModuleId)
        {
            List<MenuAccessDetailsModel> selMenuAccessDetailRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@AdminId", AdminId));
                paramList.Add(new SqlParameter("@ModuleId", ModuleId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("FetchMenuAccessData", paramList, out msg);
                selMenuAccessDetailRowData = this.__dbHelper.GetDataList<MenuAccessDetailsModel>(dt);
                if (selMenuAccessDetailRowData == null || selMenuAccessDetailRowData.Count == 0) { return null; }
                return selMenuAccessDetailRowData;
            }
            finally { dt = null; msg = null; }
        }
        public int DeleteMenuAccessDetail(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteMenuAccess"))
                {
                    connection.command.Parameters.AddWithValue("@MenuAccessId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
