namespace BlueBit.HR.Docs.BL.DataLayer.Mappings
{
    public class EmployeeExtInfoMap : Commons.ObjectInDBWithIDMap<Entities.EmployeeExtInfo>
    {
        public EmployeeExtInfoMap()
        {
            this.Map(x => x.PESEL).Not.Nullable().Length(Cfg.Defines.FLD_LEN_PESEL);
            this.Map(x => x.NIP);
            this.Map(x => x.IdentCardExpireDate);
            this.Map(x => x.PassportExpireDate);
            this.Map(x => x.TaxOffice);
            this.Map(x => x.PhoneNo);
            this.Map(x => x.BankAccount);
            this.Map(x => x.AddressCity);
            this.Map(x => x.AddressStreet);
            this.Map(x => x.AddressHouseNo);
            this.Map(x => x.AddressApartNo);
            this.Map(x => x.AddressRegCity);
            this.Map(x => x.AddressRegStreet);
            this.Map(x => x.AddressRegHouseNo);
            this.Map(x => x.AddressRegApartNo);
            this.Map(x => x.LeavePrev);
            this.Map(x => x.LeaveOwing);
            this.Map(x => x.LeaveUse);
            this.Map(x => x.LeaveRemaining);
            this.Map(x => x.LeaveCare);
            this.Map(x => x.ExamExpireDate);
        }
    }
}
