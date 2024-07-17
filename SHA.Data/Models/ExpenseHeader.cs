using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA.Data.Models
{
    public class ExpenseHeader
    {
        public long ExpenseId { get; set; }
        public bool IsInvoice { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceRefId { get; set; }
        public int SupplierId { get; set; }
        public string ExpenseDescription { get; set; }
        public decimal DebitOverAllAmount { get; set; }
        public DateTime DebitedDate { get; set; }
        public bool IsGST { get; set; }
        public int Header_GST { get; set; }
        public bool IsEmployee { get; set; }
        public int EmployeeId { get; set; }
        public int PayableAccTypeId { get; set; }
        public int AmountTypeId { get; set; }
        public int PaidBy { get; set; }
        public int ReceivedBy { get; set; }
        public string ExpenseFile { get; set; }
        public long CreatedBy { get; set; }
        public List<ExpenseDetails> ExpenseOtherDetails { get; set; }

    }

    public class ExpenseDetails
    {
        public long ExpenseId { get; set; }
        public long Id { get; set; }
        public int RowId { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TargetDescriptionId { get; set; }
        public string TargetDescription { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public int Gst { get; set; }
        public string ExpenseDetailsGSTName { get; set; }
        public long CreatedBy { get; set; }
    }

    public class ExpenseHeaderGridModel
    {
        public long ExpenseId { get; set; }
        public string InvoiceRefId { get; set; }
        public string CompanyName { get; set; }
        public string ExpenseDescription { get; set; }
        public string DebitOverAllAmount { get; set; }
        public string DebitedDate { get; set; }
        public string PayableAccTypeName { get; set; }
        public string AmountTypeName { get; set; }
    }
    public class ExpenseDetailsGridModel
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public int DetailsRowId { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string TargetDescription { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string GST { get; set; }
    }
}
