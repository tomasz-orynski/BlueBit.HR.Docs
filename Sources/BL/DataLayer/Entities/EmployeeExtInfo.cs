using System;

namespace BlueBit.HR.Docs.BL.DataLayer.Entities
{
    public interface IEmployeeExtInfo :
        Commons.IObjectInDBWithID
    {
        string PESEL { get; }
        string NIP { get; }
        DateTime? IdentCardExpireDate { get; }
        DateTime? PassportExpireDate { get; }
        string TaxOffice { get; }
        string PhoneNo { get; }
        string BankAccount { get; }
        string AddressCity { get; }
        string AddressStreet { get; }
        string AddressHouseNo { get; }
        string AddressApartNo { get; }
        string AddressRegCity { get; }
        string AddressRegStreet { get; }
        string AddressRegHouseNo { get; }
        string AddressRegApartNo { get; }
        int? LeavePrev { get; }
        int? LeaveOwing { get; }
        int? LeaveUse { get; }
        int? LeaveRemaining { get; }
        int? LeaveCare { get; }
        DateTime? ExamExpireDate { get; }
    }

    public class EmployeeExtInfo :
        IEmployeeExtInfo,
        Commons.IObjectInDBWithID_Bindable
    {
        public virtual long ID { get; set; }
        public virtual string PESEL { get; set; }
        public virtual string NIP { get; set; }
        public virtual DateTime? IdentCardExpireDate { get; set; }
        public virtual DateTime? PassportExpireDate { get; set; }
        public virtual string TaxOffice { get; set; }
        public virtual string PhoneNo { get; set; }
        public virtual string BankAccount { get; set; }
        public virtual string AddressCity { get; set; }
        public virtual string AddressStreet { get; set; }
        public virtual string AddressHouseNo { get; set; }
        public virtual string AddressApartNo { get; set; }
        public virtual string AddressRegCity { get; set; }
        public virtual string AddressRegStreet { get; set; }
        public virtual string AddressRegHouseNo { get; set; }
        public virtual string AddressRegApartNo { get; set; }
        public virtual int? LeavePrev { get; set; }
        public virtual int? LeaveOwing { get; set; }
        public virtual int? LeaveUse { get; set; }
        public virtual int? LeaveRemaining { get; set; }
        public virtual int? LeaveCare { get; set; }
        public virtual DateTime? ExamExpireDate { get; set; }
    }
}
