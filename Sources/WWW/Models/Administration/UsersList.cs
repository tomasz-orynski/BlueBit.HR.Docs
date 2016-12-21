using PagedList;

namespace BlueBit.HR.Docs.WWW.Models.Administration
{
    public class UsersList
    {
        public IPagedList<UserData> List { get; set; }

        public string Filter { get; set; }
    }
}