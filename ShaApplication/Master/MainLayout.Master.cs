using Newtonsoft.Json;
using SHA.BLL.Service;
using SHA.Data.Models;
using ShaApplication.AppForms.ControlPanel;
using ShaApplication.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;

namespace ShaApplication.Master
{
    public partial class MainLayout : System.Web.UI.MasterPage
    {
        private ILogFileService logFileService
        {
            get
            {
                if (__logFileService == null) { __logFileService = new LogFileService(); }
                return __logFileService;
            }
        }
        private ILogFileService __logFileService = null;
        private IUserService userService = null;
        private string redirectUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            List<MenuDetails> menuList;
            string jsonMenu, urlPath, pageName;
            bool flag = false;
            try
            {
                if (SessionManager.UserId <= 0)
                {
                    //FormsAuthentication.RedirectToLoginPage();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Login');", true);
                    redirectUrl = WebHelper.GetNavigationUrl("loginPage.aspx");
                    //redirectUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/AppForms/ControlPanel/loginPage.aspx";
                    Response.Redirect(redirectUrl, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                else
                {
                    jsonMenu = SessionManager.MenuData;
                    if (!IsPostBack || string.IsNullOrWhiteSpace(jsonMenu))
                    {
                        menuList = GetMenuData();
                        urlPath = Request.Url.AbsolutePath;
                        FileInfo fileInfo = new FileInfo(urlPath);
                        pageName = fileInfo.Name;
                        foreach (var item in menuList) { if (item.TaskURL == "ExpenseDetailsMaster.aspx") { flag = true; } }
                        if (!IsValidUser(menuList, pageName) && pageName != "welcomePage.aspx" && !flag)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert(You Are Not Access For this Page');", true);
                            redirectUrl = WebHelper.GetNavigationUrl("loginPage.aspx");
                            Response.Redirect(redirectUrl, false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            return;
                        }
                        jsonMenu = JsonConvert.SerializeObject(menuList);
                        SessionManager.MenuData = jsonMenu;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnGenerateListBody", $"fnGenerateListBody('{jsonMenu}');", true);
                }
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "MAIN LAYOUT", "MainLayout.Master.cs", ex, "");
                ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert", "alert('An error occurred. Please try again later.');", true);
            }
            finally { menuList = null; jsonMenu = null; }
        }

        //[WebMethod]
        public List<MenuDetails> GetMenuData()
        {
            userService = new UserService();
            try
            {
                return userService.GetMenuData(SessionManager.UserId);
            }
            catch (Exception ex)
            {
                this.logFileService.LogError(SessionManager.UserId, "MAIN LAYOUT", "MainLayout.Master.cs", ex, "");
                return new List<MenuDetails>();
            }
            finally
            {
                userService = null;
            }
        }
        public bool IsValidUser(List<MenuDetails> menuList, string pageName)
        {
            foreach (var menuItem in menuList)
            {
                if (menuItem.TaskURL.Equals(pageName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            redirectUrl = WebHelper.GetNavigationUrl("loginPage.aspx");
            Response.Redirect(redirectUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}