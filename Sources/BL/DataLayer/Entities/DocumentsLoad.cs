using System;

namespace BlueBit.HR.Docs.BL.DataLayer.Entities
{
    public interface IDocumentsLoad :
        Commons.IObjectInDBWithID
    {
        DateTime DateStart { get; }
        DateTime? DateStop { get; }
    }

    public class DocumentsLoad :
        IDocumentsLoad
    {
        public virtual long ID { get; set; }
        public virtual DateTime DateStart { get; set; }
        public virtual DateTime? DateStop { get; set; }
    }
}
