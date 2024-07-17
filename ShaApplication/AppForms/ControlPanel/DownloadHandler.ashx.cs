using System;
using System.Net;
using System.Web;
using SHA.BLL.Utility;
using System.IO;

namespace ShaApplication.AppForms.ControlPanel
{
    public class DownloadHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string filePath = context.Request.QueryString["file"];
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    string fileName = Path.GetFileName(filePath);
                    string ftpUrl = $"{FileHelper.FtpServer}/{FileHelper.DownloadTargetFolder}/{fileName}";

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential(FileHelper.UserName, FileHelper.Password);

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            context.Response.ContentType = "application/octet-stream";
                            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                            responseStream.CopyTo(context.Response.OutputStream);
                        }
                        else
                        {
                            //context.Response.StatusCode = 404;
                            //context.Response.Write("File not found");
                            ShowAlert(context, "File not found");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //context.Response.StatusCode = 500;
                    //context.Response.Write("Error downloading file: " + ex.Message);
                    ShowAlert(context, "Error downloading file: " + ex.Message);
                }
                finally
                {
                    context.Response.Flush();
                    context.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                //context.Response.StatusCode = 400;
                //context.Response.Write("No file specified");
                ShowAlert(context, "No file specified");
            }
        }
        private void ShowAlert(HttpContext context, string message)
        {
            string script = $"<script>alert('{message}');</script>";
            context.Response.Write(script);
            context.Response.Flush();
            context.ApplicationInstance.CompleteRequest();
        }
        public bool IsReusable
        {
            get { return false; }
        }
    }
}
