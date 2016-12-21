namespace BlueBit.HR.Docs.BL.DataLayer.Mappings
{
    public class SessionDocumentGetMap : Commons.ObjectInDBWithIDMap<Entities.SessionDocumentGet>
    {
        public SessionDocumentGetMap()
        {
            this.References(x => x.Session).Columns("SessionID").Not.Nullable().ForeignKey().LazyLoad();
            this.References(x => x.Document).Columns("DocumentID").Not.Nullable().ForeignKey().LazyLoad();
            this.Map(x => x.Date).Not.Nullable();
        }
    }
}
