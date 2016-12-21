using System;

namespace BlueBit.HR.Docs.BL.DataLayer.Entities
{
    public interface ISessionDocumentGet :
        Commons.IObjectInDBWithID
    {
        ISession Session { get; }
        IDocument Document { get; }
        DateTime Date { get; }
    }

    public class SessionDocumentGet :
        ISessionDocumentGet
    {
        public virtual long ID { get; set; }
        public virtual Session Session { get; set; }
        ISession ISessionDocumentGet.Session { get { return Session; } }
        public virtual DocumentWithData Document { get; set; }
        IDocument ISessionDocumentGet.Document { get { return Document; } }
        public virtual DateTime Date { get; set; }
    }
}
