using System;
using System.Net;
using System.Web;
using SHA.BLL.Utility;
using System.IO;

namespace ShaApplication.AppForms.ControlPanel
{
    public class GetImage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //string filePath = "context.Request.QueryString["file"]";
            string fileName = "wallpaperflare.com_wallpaper (2).jpg";
            string ftpUrl = $"{FileHelper.FtpServer}/{FileHelper.TargetFolder}/{fileName}";

            try
            {
                // Download the image from FTP server
                //byte[] imageBytes = DownloadImageFromFTP(ftpUrl);

                //if (imageBytes != null)
                //{
                //    context.Response.ContentType = "image/jpeg";
                //    context.Response.OutputStream.Write(imageBytes, 0, imageBytes.Length);
                //    context.Response.Flush();
                //    context.Response.SuppressContent = true;
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //}
                //else
                //{
                //    context.Response.StatusCode = 404;
                //    context.Response.StatusDescription = "Image not found";
                //}
                WebClient ftpClient = new WebClient();
                ftpClient.Credentials = new NetworkCredential(FileHelper.UserName, FileHelper.Password);

                    byte[] imageBytes = ftpClient.DownloadData(ftpUrl);

                    context.Response.ContentType = "image/jpeg"; // Set appropriate content type
                    context.Response.BinaryWrite(imageBytes);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server error: " + ex.Message;
            }
        }

        private byte[] DownloadImageFromFTP(string ftpUrl)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(FileHelper.UserName, FileHelper.Password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception)
            {
                // Handle the exception
                return null;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
