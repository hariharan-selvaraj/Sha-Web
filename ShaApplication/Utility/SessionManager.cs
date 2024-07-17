using System;
using System.Web;

namespace ShaApplication.Utility
{
    public static class SessionManager
    {
        #region Common Session Variables
        public static string __LoginId = "LOGINID";
        public static string __UserName = "USERNAME";
        public static string __ClientId = "CLIENTID";
        public static string __UserId = "USERID";
        public static string __AdminId = "ADMINID";
        public static string __RoleId = "ROLEID";
        public static string __MenuData = "MENUDATA";
        public static string __ExpenseUploadEDFilePath = "EXPENSEUPLOADEDFILEPATH";
        #endregion

        #region Accessing Session Data using Static Variable
        public static string LoginId {
            get { return GetSessionStringValueByKey(__LoginId); }
            set { SetSessionValue(__LoginId,value); }
        }
        public static string UserName
        {
            get { return GetSessionStringValueByKey(__UserName); }
            set { SetSessionValue(__UserName, value); }
        }
        public static int ClientId
        {
            get { return GetSessionIntValueByKey(__ClientId); }
            set { SetSessionValue(__ClientId, value); }
        }
        public static int UserId
        {
            get { return GetSessionIntValueByKey(__UserId); }
            set { SetSessionValue(__UserId, value); }
        }
        public static int AdminId
        {
            get { return GetSessionIntValueByKey(__AdminId); }
            set { SetSessionValue(__UserId, value); }
        }
        public static int RoleId
        {
            get { return GetSessionIntValueByKey(__RoleId); }
            set { SetSessionValue(__RoleId, value); }
        }
        public static string MenuData
        {
            get { return GetSessionStringValueByKey(__MenuData); }
            set { SetSessionValue(__MenuData, value); }
        }
        public static string ExpenseUploadEDFilePath
        {
            get { return GetSessionStringValueByKey(__ExpenseUploadEDFilePath); }
            set { SetSessionValue(__ExpenseUploadEDFilePath, value); }
        }
        #endregion

        #region Common Method
        private static string GetSessionStringValueByKey(string key)
        {
            object val;
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null) { return null; }
                val= HttpContext.Current.Session[key];
                return (val == null ? null : val.ToString());
            }
            finally { val = null; }
        }
        private static int GetSessionIntValueByKey(string key)
        {
            object val;
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null) { return 0; }
                val = HttpContext.Current.Session[key];
                return (val == null ? 0 : Convert.ToInt32(val));
            }
            finally { val = null; }
        }
        private static long GetSessionLongValueByKey(string key)
        {
            object val;
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null) { return 0; }
                val = HttpContext.Current.Session[key];
                return (val == null ? 0 : Convert.ToInt64(val));
            }
            finally { val = null; }
        }
        private static void SetSessionValue(string key,object value)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null) { return; }
            HttpContext.Current.Session[key] = value;
        }
        #endregion
    }
}