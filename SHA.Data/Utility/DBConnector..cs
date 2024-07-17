using System;
using System.Data;
using System.Data.SqlClient;

namespace SHA.Data.Utility
{
    public class DBConnector : IDisposable
    {
        private SqlConnection sqlcon { get; set; }
        private string SPName { get; set; }
        public SqlCommand command { get; set; }
        public DBConnector() { OpenConnection(); }
        public DBConnector(string sPName) {
            this.SPName = sPName;
            OpenConnection();
            this.command = new SqlCommand(sPName,this.sqlcon);
            this.command.CommandType = CommandType.StoredProcedure;
        }

        private void OpenConnection()
        {
            if (this.sqlcon == null) { this.sqlcon = new SqlConnection(DataHelper.ConnectionString); this.sqlcon.Open(); }
        }
        public void Dispose()
        {
            if (this.sqlcon != null) { this.sqlcon.Close(); }
        }
    }
}
