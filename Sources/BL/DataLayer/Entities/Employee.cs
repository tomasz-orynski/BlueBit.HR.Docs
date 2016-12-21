namespace BlueBit.HR.Docs.BL.DataLayer.Entities
{
    public interface IEmployee :
        Commons.IObjectInDBWithID
    {
        string PESEL { get; }
        string Identifier { get; }
        string PIN { get; }
        bool IsAdministrator { get; }
        bool IsLogonMailSend { get; }
    }

    public class Employee :
        IEmployee,
        Commons.IObjectInDBWithID_Bindable
    {
        public virtual long ID { get; set; }
        public virtual string PESEL { get; set; }
        public virtual string Identifier { get; set; }
        public virtual string PIN { get; set; }
        public virtual bool IsAdministrator { get; set; }
        public virtual bool IsLogonMailSend { get; set; }
    }
}
