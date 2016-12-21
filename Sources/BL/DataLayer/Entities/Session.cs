using System;

namespace BlueBit.HR.Docs.BL.DataLayer.Entities
{
    public interface ISession :
        Commons.IObjectInDBWithID
    {
        IEmployee Employee { get; }
        DateTime DateStart { get; }
        DateTime? DateStop { get; }
    }

    public class Session :
        ISession
    {
        public virtual long ID { get; set; }
        public virtual Employee Employee { get; set; }
        IEmployee ISession.Employee { get { return Employee; } }
        public virtual DateTime DateStart { get; set; }
        public virtual DateTime? DateStop { get; set; }
    }
}
