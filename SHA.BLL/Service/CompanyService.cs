using SHA.Data.Models;
using SHA.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SHA.BLL.Service
{
    public interface ICompanyService
    {
        List<DropDownItem> GetCompanyTypeDrpoDownData();
        long AddCompany(companyDetails model);
        List<companyGridModel> GetCompanyGridData();
        companyDetails FetchCompanyDetails(string selRowId);
        long EditCompany(companyDetails model);
        long DeleteCompany(string selRowId);
    }
    public class CompanyService : ICompanyService
    {
        private IDBHelper __dbHelper = null;
        public CompanyService()
        {
            __dbHelper = new DBHelper();
        }
        public List<DropDownItem> GetCompanyTypeDrpoDownData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetCompanyTypeDropDownData", null, out msg);
                return this.__dbHelper.GetDataList<DropDownItem>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public long AddCompany(companyDetails model)
        {
            try
            {
                if (model == null) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditCompany"))
                {
                    connection.command.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    connection.command.Parameters.AddWithValue("@CompanyTypeId", model.CompanyTypeId);
                    connection.command.Parameters.AddWithValue("@CompanyUen", model.CompanyUen);
                    connection.command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNo);
                    connection.command.Parameters.AddWithValue("@Address", model.Address);
                    connection.command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    connection.command.Parameters.AddWithValue("@PostalCode", model.PostalCode);
                    SqlParameter outputParam = connection.command.Parameters.Add("@CompanyId ", SqlDbType.BigInt);
                    outputParam.Direction = ParameterDirection.Output;
                    connection.command.ExecuteNonQuery();
                    return Convert.ToInt64(outputParam.Value);
                }
            }
            finally { }
        }
        public List<companyGridModel> GetCompanyGridData()
        {
            DataTable dt;
            string msg = "";
            try
            {
                dt = this.__dbHelper.ExecuteProcWithAdapter("GetCompaniesGridData", null, out msg);
                return this.__dbHelper.GetDataList<companyGridModel>(dt);
            }
            finally { dt = null; msg = null; }
        }
        public companyDetails FetchCompanyDetails(string selRowId)
        {
            List<companyDetails> selCompanyRowData;
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt;
            string msg = "";
            try
            {
                    paramList.Add(new SqlParameter("@CompanyId", long.Parse(selRowId)));
                    dt = this.__dbHelper.ExecuteProcWithAdapter("FetchCompanyData", paramList, out msg);
                    selCompanyRowData = this.__dbHelper.GetDataList<companyDetails>(dt);
                    if (selCompanyRowData == null || selCompanyRowData.Count == 0) { return null; }
                    return selCompanyRowData[0];
            }
            finally { dt = null; msg = null; }
        }
        public long EditCompany(companyDetails model)
        {
            try
            {
                if (model == null || model.CompanyId == 0) { return 0; }
                using (DBConnector connection = new DBConnector("AddEditCompany"))
                {
                    connection.command.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                    connection.command.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    connection.command.Parameters.AddWithValue("@CompanyTypeId", model.CompanyTypeId);
                    connection.command.Parameters.AddWithValue("@CompanyUen", model.CompanyUen);
                    connection.command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNo);
                    connection.command.Parameters.AddWithValue("@Address", model.Address);
                    connection.command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    connection.command.Parameters.AddWithValue("@PostalCode", model.PostalCode);
                    connection.command.ExecuteNonQuery();
                    return model.CompanyId;
                }
            }
            finally { }
        }
        public long DeleteCompany(string selRowId)
        {
            try
            {
                using (DBConnector connection = new DBConnector("DeleteCompany"))
                {
                    connection.command.Parameters.AddWithValue("@CompanyId", selRowId);
                    return connection.command.ExecuteNonQuery();
                }
            }
            finally { }
        }
    }
}
