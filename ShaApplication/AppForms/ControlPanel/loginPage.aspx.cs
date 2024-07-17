using SHA.BLL.Service;
using SHA.Data.Models;
using ShaApplication.Utility;
using System;
using System.Web;
using Newtonsoft.Json;

namespace ShaApplication.AppForms.ControlPanel
{
    public partial class loginPage : System.Web.UI.Page
    {
        #region Common Variables
        private IUserService userService = null;
        #endregion
        private ILogFileService logFileService
        {
            get
            {
                if (__logFileService == null) { __logFileService = new LogFileService(); }
                return __logFileService;
            }
        }
        private ILogFileService __logFileService = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string loginName;
            string password;
            string msg;
            string modelJson;
            string redirectUrl;
            LoginDetails Model =null;
            try
            {
                loginName = txtUserName.Text.Trim();
                password = txtPassword.Text.Trim();
                msg = ValidateInputFields(loginName, password);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    viewMessage.Text = msg;
                    viewMessage.Visible = true;
                    return;
                }
                this.userService = new UserService();
                if (this.userService.HasValidLogin(loginName, password))
                {
                    Model = new LoginDetails();
                    Model = this.userService.GetLoginUserIdByICNo(loginName);
                    if (Model == null)
                    {
                        viewMessage.Text = "Login Details Not Found.";
                        viewMessage.Visible = true;
                        return;
                    }
                    SessionManager.UserId = Model.UserId;
                    SessionManager.AdminId = Model.AdminId;
                    SessionManager.UserName = Model.AdminName;
                    SessionManager.LoginId = Model.IcNo;
                    SessionManager.RoleId = Model.RoleId;
                    redirectUrl = WebHelper.GetNavigationUrl("welcomePage.aspx");
                    Response.Redirect(redirectUrl, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    viewMessage.Text = string.IsNullOrEmpty(msg) ? "Invalid username or password" : msg;
                }
                viewMessage.Visible = !string.IsNullOrEmpty(viewMessage.Text);
            }
            catch (Exception ex)
            {
                modelJson = JsonConvert.SerializeObject(Model);
                this.logFileService.LogError(SessionManager.UserId,"LOGIN", "loginPage.aspx.cs",ex, modelJson);
            }
            finally { loginName = null; password = null; msg = null; userService = null;}
        }

        private string ValidateInputFields(string loginname, string password)
        {
            if (string.IsNullOrWhiteSpace(loginname)) { return "Please enter a valid username"; }
            if (string.IsNullOrWhiteSpace(password)) { return "Please enter a valid password"; }
            return "";
        }
    }
}