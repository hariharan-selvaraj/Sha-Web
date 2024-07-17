using SHA.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SHA.Data.Utility
{
    public interface IDBHelper
    {
        int ExecuteProc(string storeProcName, List<SqlParameter> paramList, out string msg);
        DataTable ExecuteProcWithAdapter(string storeProcName, List<SqlParameter> paramList, out string msg);
        List<T> GetDataList<T>(DataTable dt);

        string GetScalarStringValue(string query, List<Param_Key_Value_Object> paramList);
        int GetScalarIntValue(string query, List<Param_Key_Value_Object> paramList);
        int ExecuteNonQuery(string query, List<Param_Key_Value_Object> paramList, out string msg);
        UserDetails GetUserDetailsForSession(string query, List<Param_Key_Value_Object> paramList, out string msg);
        List<DropDownItem> GetCompanyDropDownData(string query);
    }
    public class DBHelper : IDBHelper
    {
        public DBHelper() {}

        public int ExecuteProc(string storeProcName, List<SqlParameter> paramList, out string msg)
        {
            try
            {
                msg = "";
                if (string.IsNullOrWhiteSpace(storeProcName)) { msg = "StoredProcedure Name Should not be empty."; return -1; }
                using (SqlConnection connection = new SqlConnection(DataHelper.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(storeProcName, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (paramList != null && paramList.Count > 0)
                    {
                        command.Parameters.AddRange(paramList.ToArray());
                    }
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                msg = (e.Message ?? "") + (e.InnerException != null ? ((e.InnerException.Message ?? "") + " " + (e.InnerException.StackTrace ?? "")) : (e.StackTrace ?? ""));
                return -1;
            }
        }
        public DataTable ExecuteProcWithAdapter(string query, List<SqlParameter> paramList, out string msg)
        {
            DataTable dt = new DataTable();
            try
            {
                msg = "";
                if (string.IsNullOrWhiteSpace(query)) { msg = "Query should not be empty."; return null; }
                using (SqlConnection connection = new SqlConnection(DataHelper.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(query,connection))
                    {
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        if (paramList != null && paramList.Count > 0)
                        {
                            da.SelectCommand.Parameters.AddRange(paramList.ToArray());
                        }
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                msg = (e.Message ?? "") + (e.InnerException != null ? ((e.InnerException.Message ?? "") + " " + (e.InnerException.StackTrace ?? "")) : (e.StackTrace ?? ""));
                return null;
            }
            finally
            {
                dt = null;
            }
        }
        public List<T> GetDataList<T>(DataTable dt)
        {
            PropertyInfo[] filedList;
            List<T> resList;
            try
            {
                resList = new List<T>();
                if (dt==null || dt.Rows==null || dt.Rows.Count == 0) { return resList; }
                filedList = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                foreach (DataRow dr in dt.Rows)
                {
                    // Create the object of T
                    T obj = Activator.CreateInstance<T>();
                    foreach (var fieldInfo in filedList)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (fieldInfo.Name == dc.ColumnName)
                            {
                                if (dr[dc.ColumnName] != DBNull.Value){
                                    fieldInfo.SetValue(obj, dr[dc.ColumnName]);
                                }
                                break;
                            }
                        }
                    }
                    resList.Add(obj);
                }
                return resList;
            }
            finally
            {
               // filedList = null;
                resList = null;
            }
        }
        public string GetScalarStringValue(string query, List<Param_Key_Value_Object> paramList)
        {
            object val;
            try
            {
                val = GetSclalarValue(query, paramList);
                return (val != null ? val.ToString() : null);
            }
            finally { val = null; }
        }
        public int GetScalarIntValue(string query, List<Param_Key_Value_Object> paramList)
        {
            object val;
            try
            {
                val = GetSclalarValue(query, paramList);
                return (val != null ? Convert.ToInt32(val) : 0);
            }
            finally { val = null; }
        }

        private object GetSclalarValue(string query, List<Param_Key_Value_Object> paramList)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query)) { return null; }
                using (SqlConnection connection = new SqlConnection(DataHelper.ConnectionString))
                {

                    SqlCommand command = new SqlCommand(query, connection);
                    if (paramList != null && paramList.Count > 0)
                    {
                        for (int i = 0; i < paramList.Count; i++) { command.Parameters.AddWithValue(paramList[i].Key, paramList[i].Value); }
                    }
                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                string m = e.Message ?? "";
                return null;
            }
        }

        public int ExecuteNonQuery(string query, List<Param_Key_Value_Object> paramList, out string msg)
        {
            try
            {
                msg = "";
                if (string.IsNullOrWhiteSpace(query)) { msg = "Query should not be empty."; return -1; }
                using (SqlConnection connection = new SqlConnection(DataHelper.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    if (paramList != null && paramList.Count > 0)
                    {
                        for (int i = 0; i < paramList.Count; i++) { command.Parameters.AddWithValue(paramList[i].Key, paramList[i].Value); }
                    }
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                msg = (e.Message ?? "") + (e.InnerException != null ? ((e.InnerException.Message ?? "") + " " + (e.InnerException.StackTrace ?? "")) : (e.StackTrace ?? ""));
                return -1;
            }
        }
        

        public UserDetails GetUserDetailsForSession(string query, List<Param_Key_Value_Object> paramList, out string msg)
        {
            UserDetails model = null;
            try
            {
                msg = "";
                if (string.IsNullOrWhiteSpace(query)) { msg = "Query should not be empty."; return new UserDetails(); }
                using (SqlConnection connection = new SqlConnection(DataHelper.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    if (paramList != null && paramList.Count > 0)
                    {
                        for (int i = 0; i < paramList.Count; i++) { command.Parameters.AddWithValue(paramList[i].Key, paramList[i].Value); }
                    }
                    connection.Open();
                    using (var data = command.ExecuteReader())
                    {
                        if (data.HasRows && data.Read())
                        {
                            model = new UserDetails
                            {
                                IcNumber = data["IcNo"] != DBNull.Value ? data["IcNo"].ToString() : null,
                                AdminName = data["AdminName"] != DBNull.Value ? data["AdminName"].ToString() : null,
                                UserId = data["AdminId"] != DBNull.Value ? Convert.ToInt64(data["AdminId"]) : 0
                            };
                        }
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                msg = (e.Message ?? "") + (e.InnerException != null ? ((e.InnerException.Message ?? "") + " " + (e.InnerException.StackTrace ?? "")) : (e.StackTrace ?? ""));
                return new UserDetails();
            }
            finally
            {
                model = null;
            }
        }

        public List<DropDownItem> GetCompanyDropDownData(string query)
        {
            List<DropDownItem> companyData = new List<DropDownItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(DataHelper.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            long companyId = (long)reader["CompanyId"];
                            string companyName = reader["CompanyName"].ToString();

                            companyData.Add(new DropDownItem { Key = companyId, Value = companyName });
                        }

                        reader.Close();
                    }
                }
                return companyData;
            }
            catch (Exception ex)
            {
                string m = ex.Message ?? "";
                return new List<DropDownItem>();
            }
        }

        
    }
}
