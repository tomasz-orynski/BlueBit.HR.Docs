using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueBit.HR.Docs.BL.BusinessLayer.Services.Enviroment
{
    public interface IGetUsersServiceContext : IServiceContext
    {
    }

    public class GetUsersServiceContext :
        ServiceContext,
        IGetUsersServiceContext
    {
        public GetUsersServiceContext(IBusinessTransaction businessTransaction)
            : base(businessTransaction)
        {
        }
    }

    public interface IUserInfo
    {
        string Identifier { get; }
        string FullName { get; }
        string EMail { get; }
    }

    internal class UserInfo : 
        IUserInfo
    {
        public string Identifier { get; set;}
        public string FullName { get; set;}
        public string EMail { get; set;}
    }

    internal static class Ext
    {
        public static IEnumerable<System.DirectoryServices.SearchResult> Concat(this IEnumerable<System.DirectoryServices.SearchResultCollection> source)
        {
            foreach (var s in source)
                foreach (var i in s)
                    yield return (System.DirectoryServices.SearchResult)i;
        }
    }

    internal sealed class GetUsersService
    {
        private static DateTime lastGet = DateTime.Now;
        private static List<IUserInfo> userInfos = null;
        private static object synch = new object();

        public static IEnumerable<IUserInfo> GetUsers(Func<IEnumerable<System.DirectoryServices.DirectorySearcher>> getADSearchers)
        {
            var ts = DateTime.Now - lastGet;
            if (userInfos == null || ts.TotalMinutes >= 10)
            lock (synch)
            {
                if (userInfos == null)
                {
                    userInfos = getADSearchers().Select((s)=>{ s.Filter = "(objectClass=user)"; return s; })
                        .Select((s) => s.FindAll())
                        .Concat()
                        .Select(sr => sr.GetDirectoryEntry())
                        .Select(de => new UserInfo()
                        {
                            Identifier = GetUserIdentifier(de),
                            FullName = (string)de.Properties["name"].Value,
                            EMail = (string)de.Properties["Mail"].Value,
                        })
                        .Cast<IUserInfo>()
                        .ToList();
                    lastGet = DateTime.Now;
                }
            }
            return userInfos;
        }

        private static string GetUserIdentifier(System.DirectoryServices.DirectoryEntry de)
        {
            var nameTypeNT = (int)ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_NT4;
            var nameTypeDN = (int)ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_1779;
            var nameTypeUserPrincipalName = (int)ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_USER_PRINCIPAL_NAME;

            var trans = new ActiveDs.NameTranslate();
            try
            {
                var name = (string)de.Properties["distinguishedname"].Value;
                trans.Set(nameTypeDN, name);
                return trans.Get(nameTypeNT);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(trans);
            }
        }
    }

    public sealed class GetUsersService<TServiceContext> : ServiceBase<TServiceContext, IEnumerable<IUserInfo>>
        where TServiceContext : class, IGetUsersServiceContext
    {
        public GetUsersService(TServiceContext context) : base(context) { }

        protected override IEnumerable<IUserInfo> OnExecute()
        {
            return GetUsersService.GetUsers(ServiceCtx.BusinessTransaction.BusinessCtx.GetADSearchers);
        }
    }
}
