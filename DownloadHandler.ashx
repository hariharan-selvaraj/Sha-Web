using System;
using System.IO;
using System.Net;
using System.Web;

namespace ShaApplication.AppForms.ControlPanel
{
    public class DownloadHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string filePath = context.Request.QueryString["filePath"];
            if (string.IsNullOrEmpty(filePath))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Invalid file path.");
                return;
            }

            string fileName = Path.GetFileName(filePath);
            string ftpUrl = $"{FileHelper.FtpServer}/{FileHelper.DownloadTargetFolder}/{fileName}";

            try
            {
                byte[] fileData = DownloadFileFromFtp(ftpUrl, FileHelper.UserName, FileHelper.Password);

                if (fileData != null)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                    context.Response.BinaryWrite(fileData);
                    context.Response.End();
                }
                else
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write("File download failed.");
                }
            }
            catch (WebException ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write((ex.Response as FtpWebResponse).StatusDescription);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private static byte[] DownloadFileFromFtp(string downloadUrl, string username, string password)
        {
            FtpWebRequest request;
            byte[] fileData;
            try
            {
                request = (FtpWebRequest)WebRequest.Create(downloadUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(username, password);
                request.UseBinary = true;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            responseStream.CopyTo(ms);
                            fileData = ms.ToArray();
                        }
                    }
                    return fileData;
                }
            }
            finally { request = null; }
        }
    }
}
