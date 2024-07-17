using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace SHA.BLL.Utility
{
    public static class FileHelper
    {
        public static string FileStorageType { get; set; }
        public static string BaseUrl { get; set; }
        public static string FtpServer { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string TargetFolder { get; set; }
        public static string FolderName { get; set; }
        public static string DownloadTargetFolder { get; set; }

        public static FileResponse Upload(FileInputParam inputParam)
        {
            FileResponse response = null;
            try
            {
                if (inputParam == null) { return new FileResponse() { Status = "F", Message = "Input Param should not be null." }; }
                if (string.IsNullOrWhiteSpace(FileStorageType)) { return new FileResponse() { Status = "F", Message = "File Storage Type should not be null." }; }
                if (string.IsNullOrWhiteSpace(BaseUrl)) { return new FileResponse() { Status = "F", Message = "Base URL should not be null." }; }
                if (string.IsNullOrWhiteSpace(FolderName)) { return new FileResponse() { Status = "F", Message = "Folder Name should not be null." }; }
                if (string.IsNullOrWhiteSpace(inputParam.FileName)) { return new FileResponse() { Status = "F", Message = "FileName should not be null." }; }
                if (string.IsNullOrWhiteSpace(inputParam.DirectoryName)) { return new FileResponse() { Status = "F", Message = "Directory Name should not be null." }; }
                if (string.IsNullOrWhiteSpace(inputParam.BaseDirectory)) { return new FileResponse() { Status = "F", Message = "Base Directory should not be null." }; }
                if (string.IsNullOrWhiteSpace(inputParam.FolderName)) { return new FileResponse() { Status = "F", Message = "Folder Name should not be null." }; }
                if (inputParam.InputStream == null) { return new FileResponse() { Status = "F", Message = "Input Stream should not be null." }; }

                switch (FileStorageType)
                {
                    case "INAPP":
                        response = UploadInApp(inputParam);
                        break;
                    case "INFTP":
                        response = UploadInFTP(inputParam);
                        break;
                    default:
                        break;
                }
                return response;
            }
            catch (Exception ex)
            {
                return new FileResponse() { Status = "E", Message = (ex.Message ?? "") + (ex.InnerException != null ? ((ex.InnerException.Message ?? "") + (ex.InnerException.StackTrace ?? "")) : (ex.StackTrace ?? "")) };
            }
            finally { response = null; }

        }
        private static FileResponse UploadInApp(FileInputParam inputParam)
        {
            try
            {
                if (!Directory.Exists(inputParam.BaseDirectory))
                {
                    Directory.CreateDirectory(inputParam.BaseDirectory);
                }
                if (!Directory.Exists(inputParam.BaseDirectory + "/" + inputParam.FolderName))
                {
                    Directory.CreateDirectory(inputParam.BaseDirectory + "/" + inputParam.FolderName);
                }
                if (!Directory.Exists(inputParam.BaseDirectory + "/" + inputParam.FolderName + "/" + inputParam.DirectoryName))
                {
                    Directory.CreateDirectory(inputParam.BaseDirectory + "/" + inputParam.FolderName + "/" + inputParam.DirectoryName);
                }
                if (File.Exists(inputParam.BaseDirectory + "/" + inputParam.FolderName + "/" + inputParam.DirectoryName + "/" + inputParam.FileName))
                {
                    return new FileResponse() { Status = "F", Message = "File Already Exist." };
                }
                using (FileStream fs = new FileStream(Path.Combine(inputParam.BaseDirectory + "/" + inputParam.FolderName + "/" + inputParam.DirectoryName, inputParam.FileName), FileMode.CreateNew, FileAccess.Write))
                {
                    inputParam.InputStream.CopyTo(fs);
                }
                return new FileResponse() { Status = "S", Message = "Uploaded Successfully." };
            }
            catch (Exception ex)
            {
                return new FileResponse() { Status = "E", Message = (ex.Message ?? "") + (ex.InnerException != null ? ((ex.InnerException.Message ?? "") + (ex.InnerException.StackTrace ?? "")) : (ex.StackTrace ?? "")) };
            }
        }
        private static FileResponse UploadInFTP(FileInputParam inputParam)
        {
            FtpWebResponse ftpWebResponse;
            FileResponse fileResponse;
            string filePath, uploadUrl, directoryUrl;
            try
            {
                if (inputParam == null) { return new FileResponse() { Status = "F", Message = "Input Param should not be null." }; }
                if (string.IsNullOrWhiteSpace(FtpServer)) { return new FileResponse() { Status = "F", Message = "FTP Server should not be null." }; }
                if (string.IsNullOrWhiteSpace(UserName)) { return new FileResponse() { Status = "F", Message = "FTP Server User Name should not be null." }; }
                if (string.IsNullOrWhiteSpace(Password)) { return new FileResponse() { Status = "F", Message = "FTP Server Password should not be null." }; }
                if (string.IsNullOrWhiteSpace(TargetFolder)) { return new FileResponse() { Status = "F", Message = "FTP Server Target Folder should not be null." }; }
                //filePath = inputParam.FolderName + "/" + inputParam.DirectoryName + "/" + inputParam.FileName;
                filePath = inputParam.FileName;
                uploadUrl = $"{FtpServer}{TargetFolder}/{filePath}";
                directoryUrl = $"{FtpServer}{TargetFolder}/{inputParam.FolderName}/{inputParam.DirectoryName}";
                //CreateDirectoryInFtp(directoryUrl);
                ftpWebResponse = UploadFileInFtp(uploadUrl, inputParam.InputStream, UserName, Password);
                fileResponse = new FileResponse();
                if (ftpWebResponse != null && ftpWebResponse.StatusDescription.Contains("226"))
                {
                    fileResponse.Status = "S";
                }
                else
                {
                    fileResponse.Message = ftpWebResponse?.StatusDescription ?? "Unknown error";
                    fileResponse.Status = "F";
                }
                return fileResponse;
            }
            catch (Exception ex)
            {
                return new FileResponse() { Status = "E", Message = (ex.Message ?? "") + (ex.InnerException != null ? ((ex.InnerException.Message ?? "") + (ex.InnerException.StackTrace ?? "")) : (ex.StackTrace ?? "")) };
            }
            finally { ftpWebResponse = null; fileResponse = null; filePath = null; uploadUrl = null; directoryUrl = null; }
        }

        private static void CreateDirectoryInFtp(string directoryUrl)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryUrl);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(UserName, Password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return;
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    throw;
                }
            }
        }
        private static FtpWebResponse UploadFileInFtp(string uploadUrl, Stream inputStream, string username, string password)
        {
            byte[] fileContents;
            FtpWebRequest request;
            try
            {
                using (StreamReader sourceStream = new StreamReader(inputStream))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }
                request = (FtpWebRequest)WebRequest.Create(uploadUrl);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(username, password);
                request.UseBinary = true;
                request.ContentLength = fileContents.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return response;
                }
            }
            finally { fileContents = null; request = null; }
        }
        public static string DownloadFromFTP(string ftpUrl)
        {
            try
            {
                return DownloadFileFromFtps(ftpUrl, UserName, Password);
            }
            finally { }
        }

        private static string DownloadFileFromFtps(string downloadUrl, string username, string password)
        {
            FtpWebRequest request;
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
                        return ReadFully(responseStream);
                    }
                }
            }
            finally { request = null; }
        }
        private static string ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public static byte[] DownloadImageFromFtp(string ftpServer)
        {
            WebClient ftpClient = new WebClient();
            try
            {
                ftpClient.Credentials = new NetworkCredential(UserName, Password);
                return ftpClient.DownloadData(ftpServer);
            }
            finally { ftpClient.Dispose(); }
        }
        public static long CheckFileIsExist(string filePath)
        {
            FtpWebResponse response;
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(filePath));
                request.Credentials = new NetworkCredential(UserName, Password);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.UseBinary = true;
                response = (FtpWebResponse)request.GetResponse();
                response.Close();
                return response != null ? response.ContentLength : 0;
            }
            finally { response = null; request = null; }
        }

    }
    public class FileInputParam
    {
        public string FileName { get; set; }
        public string BaseDirectory { get; set; }
        public string FolderName { get; set; }
        public string DirectoryName { get; set; }
        public Stream InputStream { get; set; }

    }
    public class FileResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
