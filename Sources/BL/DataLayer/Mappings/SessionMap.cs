namespace BlueBit.HR.Docs.BL.DataLayer.Mappings
{
    public class SessionMap : Commons.ObjectInDBWithIDMap<Entities.Session>
    {
        public SessionMap()
        {
            this.References(x => x.Employee).Columns("EmployeeID").Not.Nullable().ForeignKey().LazyLoad();
            this.Map(x => x.DateStart).Not.Nullable();
            this.Map(x => x.DateStop).Nullable();
        }
    }
}
