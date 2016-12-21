namespace BlueBit.HR.Docs.BL.DataLayer.Mappings
{
    public class DocumentsLoadMap : Commons.ObjectInDBWithIDMap<Entities.DocumentsLoad>
    {
        public DocumentsLoadMap()
        {
            this.Map(x => x.DateStart).Not.Nullable();
            this.Map(x => x.DateStop).Nullable();
        }
    }
}
