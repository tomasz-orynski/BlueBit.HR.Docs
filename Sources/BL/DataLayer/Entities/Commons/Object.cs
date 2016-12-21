namespace BlueBit.HR.Docs.BL.DataLayer.Entities.Commons
{
    public interface IObjectInDB
    {
    }

    public interface IObjectInDBWithID :
        IObjectInDB
    {
        long ID { get; }
    }

    public interface IObjectInDBWithID_Bindable :
        IObjectInDBWithID
    {
        long ID { get; set; }
    }
}
