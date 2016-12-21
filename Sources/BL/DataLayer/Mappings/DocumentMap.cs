using System;

namespace BlueBit.HR.Docs.BL.DataLayer.Mappings
{
    public class DocumentBaseMap<T> : Commons.ObjectInDBWithIDMap<T>
        where T: Entities.DocumentBase
    {
        public DocumentBaseMap()
        {
            this.References(x => x.DocumentsLoad).Columns("DocumentsLoadID").Not.Nullable().ForeignKey().LazyLoad();
            this.Map(x => x.PESEL).Not.Nullable().Length(Cfg.Defines.FLD_LEN_PESEL);
            this.Map(x => x.DateYear).Not.Nullable();
            this.Map(x => x.DateMonth).Not.Nullable();
        }
    }

    public class DocumentWithDataMap : DocumentBaseMap<Entities.DocumentWithData>
    {
        public DocumentWithDataMap()
        {
            this.Map(x => x.Data).Not.Nullable().Length(Int32.MaxValue);
        }
    }

    public class DocumentWithoutDataAndLastVerMap : DocumentBaseMap<Entities.DocumentWithoutDataAndLastVer>
    {
        public DocumentWithoutDataAndLastVerMap()
        {
        }
    }
}
