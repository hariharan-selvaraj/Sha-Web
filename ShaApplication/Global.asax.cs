using SHA.BLL.Utility;
using SHA.Data.Utility;
using ShaApplication.Utility;
using System;
using System.Configuration;
using System.Net;
using System.Web;

namespace ShaApplication
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            DataHelper.ConnectionString = WebHelper.ConnectionString;
            DataHelper.LogFilePath = Server.MapPath("~/Logs/");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            FileHelper.BaseUrl = ConfigurationManager.AppSettings["FileBaseURL"];
            FileHelper.FileStorageType = ConfigurationManager.AppSettings["FileStorageType"];
            FileHelper.FtpServer = ConfigurationManager.AppSettings["FtpServer"];
            FileHelper.UserName = ConfigurationManager.AppSettings["UserName"];
            FileHelper.Password = ConfigurationManager.AppSettings["Password"];
            FileHelper.TargetFolder = ConfigurationManager.AppSettings["TargetFolder"];
            FileHelper.FolderName = ConfigurationManager.AppSettings["FolderPath"];
            FileHelper.DownloadTargetFolder = ConfigurationManager.AppSettings["DownloadTargetFolder"];
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (SessionManager.UserId <= 0)
            {
                string redirectUrl = WebHelper.GetNavigationUrl("loginPage.aspx");
                Response.Redirect(redirectUrl, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}