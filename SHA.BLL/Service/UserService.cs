using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SHA.BLL.Service
{
    public interface IUserService
    {
        bool HasValidLogin(string userName, string password);
        long RegisterUser(UserDetails model);
        LoginDetails GetLoginUserIdByICNo(string loginId);
        List<MenuDetails> GetMenuData(long adminId);
    }
    public class UserService : IUserService
    {
        private IDBHelper __dbHelper = null;
        public UserService()
        {
            __dbHelper = new DBHelper();
        }

        /// <summary>
        /// Method used to validate whether the login is valid or not based on user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool HasValidLogin(string userName, string password)
        {
            List<SqlParameter> spParam;
            string msg = "";
            int flag = 0;
            try
            {
                spParam = new List<SqlParameter>();
                spParam.Add(new SqlParameter("@Username", userName));
                spParam.Add(new SqlParameter("@Password", password));
                flag = this.__dbHelper.ExecuteProc("ValidateLogin", spParam, out msg);
                return (flag > 0);
            }
            finally { spParam = null; msg = null; }
        }
        /// <summary>
        /// Used to create Users into our system
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long RegisterUser(UserDetails model)
        {
            try
            {
                if (model == null) { return 0;}
                using (DBConnector connection = new DBConnector("RegisterAdmin"))
                {
                    connection.command.Parameters.AddWithValue("@CompanyId", model.Company);
                    connection.command.Parameters.AddWithValue("@AdminName", model.AdminName);
                    connection.command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                    connection.command.Parameters.AddWithValue("@Address", model.Address);
                    connection.command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    connection.command.Parameters.AddWithValue("@PassportNumber", model.PassportNumber);
                    connection.command.Parameters.AddWithValue("@IcNumber", model.IcNumber);
                    connection.command.Parameters.AddWithValue("@IsAdmin", model.IsAdmin ? 1 : 0);
                    connection.command.Parameters.AddWithValue("@Password", model.Password);
                    connection.command.Parameters.AddWithValue("@RoleId", model.RoleId);
                    SqlParameter outputParam = connection.command.Parameters.Add("@AdminId", SqlDbType.BigInt);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt64(outputParam.Value);
                }
            }
            finally { }
        }

        public LoginDetails GetLoginUserIdByICNo(string loginId)
        {
            List<LoginDetails> userDetails;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                paramList.Add(new SqlParameter("@IcNo", loginId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetLoginDetailsByICNo", paramList, out msg);
                userDetails = this.__dbHelper.GetDataList<LoginDetails>(dt);
                if (userDetails == null || userDetails.Count == 0) { return null; }
                return userDetails[0];
            }
            finally { userDetails = null; paramList = null; dt = null; msg = null; }
        }
        public List<MenuDetails> GetMenuData(long adminId)
        {
            List<MenuDetails> menuList;
            List<SqlParameter> paramList;
            DataTable dt;
            string msg = "";
            try
            {
                paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@AdminId", adminId));
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetMenuData", paramList, out msg);
                menuList= this.__dbHelper.GetDataList<MenuDetails>(dt);
                return menuList??new List<MenuDetails>();
            }
            finally
            {
                paramList = null; dt = null; menuList = null; msg = null;
            }
        }
    }
}
