using System;
using System.Collections.Generic;
using System.Configuration;
using ShaApplication.Utility;
using System.IO;
using System.Web;
using System.Web.UI;

namespace ShaApplication.Utility
{
    public static class WebHelper
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SHAConnectionString"].ConnectionString;
            }
        }
        public static string WebBaseURL
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerBaseURL"] ??"";
            }
        }
        public static string GetNavigationUrl(string pageName)
        {
            return WebBaseURL + "/" + pageName;
        }
    }
}