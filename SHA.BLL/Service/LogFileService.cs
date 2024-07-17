using SHA.Data.Utility;
using System;
using System.IO;

namespace SHA.BLL.Service
{
    public interface ILogFileService
    {
        void LogError(string msg);
        void LogError(long userId, string activity, string url, Exception ex, string modelJson);
    }
    public class LogFileService : ILogFileService
    {
        public void LogError(long userId,string activity,string url, Exception ex, string modelJson)
        {
            string errorLineNo, errorMsg, extype, msg;
            try
            {
                errorLineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                errorMsg = (ex.GetType().Name ?? "").ToString();
                extype = ex.GetType().ToString();
                msg = (ex.Message ?? "") + (ex.InnerException != null ? (ex.InnerException.Message ?? "" + " " + (ex.InnerException.StackTrace ?? "")) : (ex.StackTrace ?? ""));
                using (DBConnector connection = new DBConnector("LogError"))
                {
                    connection.command.Parameters.AddWithValue("@UserId", userId);
                    connection.command.Parameters.AddWithValue("@Activity", activity);
                    connection.command.Parameters.AddWithValue("@ActURL", url);
                    connection.command.Parameters.AddWithValue("@ProcedureName", "");
                    connection.command.Parameters.AddWithValue("@ActData", modelJson);
                    connection.command.Parameters.AddWithValue("@ErrorType", extype);
                    connection.command.Parameters.AddWithValue("@ErrorMessage", msg);
                    connection.command.Parameters.AddWithValue("@ErrorNo", "0");
                    connection.command.Parameters.AddWithValue("@ErrorLine", errorLineNo);
                    connection.command.ExecuteNonQuery();
                }
            }
            catch (Exception e) { LogError(e); }
            finally {  errorLineNo = null; errorMsg = null; extype = null; msg = null; }
        }
        private  void LogError(Exception ex)
        {
            string line, errorLineNo, errorMsg, extype, msg, filepath;
            try
            {
                filepath = DataHelper.LogFilePath;  //Text File Path
                if (!Directory.Exists(filepath)) { Directory.CreateDirectory(filepath); }
                filepath = filepath + DateTime.Now.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath)) { File.Create(filepath).Dispose(); }
                line = Environment.NewLine;
                errorLineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                errorMsg = (ex.GetType().Name ?? "").ToString();
                extype = ex.GetType().ToString();
                msg = (ex.Message ?? "") + (ex.InnerException != null ? (ex.InnerException.Message ?? "" + " " + (ex.InnerException.StackTrace ?? "")) : (ex.StackTrace ?? ""));
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + errorLineNo + line + "Error Message:" + " " + errorMsg + line + "Exception Type:" + " " + extype + line + "StackTrace:" + " " + msg;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            finally { line = null; errorLineNo = null; errorMsg = null; extype = null; msg = null; filepath = null; }
        }
        public void LogError(string msg)
        {
            string line, filepath;
            try
            {
                filepath = DataHelper.LogFilePath;  //Text File Path
                if (!Directory.Exists(filepath)) { Directory.CreateDirectory(filepath); }
                filepath = filepath + DateTime.Now.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath)) { File.Create(filepath).Dispose(); }
                line = Environment.NewLine;
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line +  "StackTrace:" + " " + msg;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e) { e.ToString(); }
            finally { line = null;  filepath = null; }
        }
    }
}
