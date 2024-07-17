using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA.Data.Models
{
    public class UserDetails
    {
        public long UserId { get; set; }
        public long Company { get; set; }
        public string AdminName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PassportNumber { get; set; }
        public string IcNumber { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }
    }
    public class LoginDetails
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string IcNo { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
    public class ModuleMasterGridModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public string ModuleIcon { get; set; }
        public long CreatedBy { get; set; }
    }
    public class MenuItemDetailsGridModel
    {
        public int MenuItemId { get; set; }
        public string ModuleName { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemDescription { get; set; }
        public string TaskURL { get; set; }
        public string MenuItemIcon { get; set; }
    }
    public class MenuItemDetailsModel
    {
        public int MenuItemId { get; set; }
        public int ModuleMasterId { get; set; }
        public string ModuleName { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemDescription { get; set; }
        public string TaskURL { get; set; }
        public string MenuItemIcon { get; set; }
        public long CreatedBy { get; set; }
    }
    public class MenuAccessDetailsGridModel
    {
        public int MenuAccessId { get; set; }
        public string AdminName { get; set; }
        public string ModuleName { get; set; }
        public string MenuItemName { get; set; }
    }
    public class MenuAccessDetailsModel
    {
        public int MenuAccessId { get; set; }
        public int ModuleId { get; set; }
        public int MenuItemId { get; set; }
        public int AdminId { get; set; }
        public long CreatedBy { get; set; }
        public List<string> SelectedMenuItems { get; set; }
    }
    public class MenuDetails
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string TaskURL { get; set; }
        public string MenuItemIcon { get; set; }
        public int ParentId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleIcon { get; set; }

    }
    public class companyDetails
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CompanyTypeId { get; set; }
        public string CompanyUen { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PostalCode { get; set; }
    }
    public class companyGridModel
    {
        public long CompanyId { get; set; }
        public string CompanyRefId { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNo { get; set; }
        public string EmailAddress { get; set; }
    }

    public class IncomeDetails
    {
        public long IncomeId { get; set; }
        public bool IsInvoice { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceRefId { get; set; }
        public int CompanyId { get; set; }
        public bool HasProject { get; set; }
        public bool IsEmployee { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int ReceivableAccountTypeId { get; set; }
        public string IncomeDescription { get; set; }
        public decimal CreditAmount { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreditedDate { get; set; }
        public bool IsGST { get; set; }
        public int GstRate { get; set; }
        public int AmountTypeId { get; set; }
        public int AdminId { get; set; }
        public int TargetDescriptionId { get; set; }
    }

    public class IncomeDetailsGridModel
    {
        public long IncomeId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceRefId { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string RecivableAccTypeName { get; set; }
        public string IncomeDescription { get; set; }
        public string CreditAmount { get; set; }
        public string CreditedDate { get; set; }
        public string AmountTypeName { get; set; }
        public string GstRate { get; set; }
        public string UserName { get; set; }
        public string PaymentTargetType { get; set; }
    }

    
    public class RecievableAccTypeModel
    {
        public int RecivableAccTypeId { get; set; }
        public string RecivableAccTypeName { get; set; }
        public string RecivableAccTypeDescription { get; set; }
        public long CreatedBy { get; set; }
    }
    public class RecievableAccTypeGridModel
    {
        public int RecivableAccTypeId { get; set; }
        public string RecivableAccTypeName { get; set; }
        public string RecivableAccTypeDescription { get; set; }
    }
    public class PayableAccTypeModel
    {
        public int PayableAccTypeId { get; set; }
        public string PayableAccTypeName { get; set; }
        public string PayableAccTypeDescription { get; set; }
        public long CreatedBy { get; set; }
    }
    public class PayableAccTypeGridModel
    {
        public int PayableAccTypeId { get; set; }
        public string PayableAccTypeName { get; set; }
        public string PayableAccTypeDescription { get; set; }
    }
    public class PaymentTargetMasterGridModel
    {
        public int PaymentTargetId { get; set; }
        public string ProjectName { get; set; }
        public string PaymentTargetType { get; set; }
        public string PaymentTargetFromDate { get; set; }
        public string PaymentTargetToDate { get; set; }
        public string Amount { get; set; }
        public string PaymentTargetDescription { get; set; }
    }
    public class PaymentTargetMasterModel
    {
        public int PaymentTargetId { get; set; }
        public int ProjectId { get; set; }
        public string PaymentTargetType { get; set; }
        public DateTime PaymentTargetFromDate { get; set; }
        public DateTime PaymentTargetToDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentTargetDescription { get; set; }
        public long CreatedBy { get; set; }
    }
    public class TimeSheetGridModel
    {
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string MonthlyWorkedHrs { get; set; }
        public string HolidayType { get; set; }
        public string WorkedDays { get; set; }
        public string UserName { get; set; }
        public string ProjectType { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string WorkedOfficeHrs { get; set; }
        public string InTimeImgPath { get; set; }
        public string OutTimeImgPath { get; set; }
    }
    public enum TeamName
    {
        SHA = 1,
        SUPPLY = 2
    }
    public class TeamSupplyMasterGridModel
    {
        public int Team_Supply_Id { get; set; }
        public short Team_Id { get; set; }
        public string Team_Head_Name { get; set; }
        public string TeamName => ((TeamName)Team_Id).ToString();
    }
    public class TeamSupplyMasterModel
    {
        public int TeamSupplyId { get; set; }
        public short TeamId { get; set; }
        public string TeamHeadName { get; set; }
        public long CreatedBy { get; set; }
    }
    public class WorkPlanMasterGridModel
    {
        public long Work_Plan_Id { get; set; }
        public string ProjectName { get; set; }
        public string PlannedDate { get; set; }
        public string WorkDescription { get; set; }
        public string Phase { get; set; }
        public string TeamSupplyName { get; set; }
        public int WorkersCount { get; set; }
    }
    public class WorkPlanMasterModel
    {
        public long WorkPlanId { get; set; }
        public int ProjectId { get; set; }
        public DateTime PlannedDate { get; set; }
        public string WorkDescription { get; set; }
        public string Phase { get; set; }
        public int Team_Supply_Id { get; set; }
        public List<string> SelectedWorkers { get; set; }
        public int WorkersCount { get; set; }
        public long CreatedBy { get; set; }
    }
}
