using FluentNHibernate.Mapping;

namespace BlueBit.HR.Docs.BL.DataLayer.Mappings
{
    public class EmployeeMap : Commons.ObjectInDBWithIDMap<Entities.Employee>
    {
        public EmployeeMap()
        {
            this.Map(x => x.PESEL).Not.Nullable().Length(Cfg.Defines.FLD_LEN_PESEL);
            this.Map(x => x.Identifier).Not.Nullable().Length(Cfg.Defines.FLD_LEN_IDENTIFIER);
            this.Map(x => x.PIN).Not.Nullable().Length(Cfg.Defines.FLD_LEN_PIN);
            this.Map(x => x.IsAdministrator).Not.Nullable();
            this.Map(x => x.IsLogonMailSend).Not.Nullable();
        }
    }
}
