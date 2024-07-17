using ShaApplication.Utility;
using System;
using System.Web;

namespace ShaApplication.Master
{
    public partial class LoginLayout : System.Web.UI.MasterPage
    {
        private string redirectUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                redirectUrl = WebHelper.GetNavigationUrl("RegisterPage.aspx");
                Response.Redirect(redirectUrl, true);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            finally { redirectUrl = null; }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                redirectUrl = WebHelper.GetNavigationUrl("loginPage.aspx");
                Response.Redirect(redirectUrl, true);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            finally { redirectUrl = null; }
        }
    }
}