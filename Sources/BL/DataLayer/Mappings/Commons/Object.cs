using FluentNHibernate.Mapping;

namespace BlueBit.HR.Docs.BL.DataLayer.Mappings.Commons
{
    public abstract class ObjectInDBWithIDMap<T> :
        ClassMap<T>
        where T: DataLayer.Entities.Commons.IObjectInDBWithID
    {
        protected ObjectInDBWithIDMap()
        {
            this.Table(Cfg.Defines.GetTableName<T>());
            this.Id(x => x.ID);
        }
    }
}
