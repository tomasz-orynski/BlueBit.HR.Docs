using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BlueBit.HR.Docs.BL.Diagnostics;

namespace BlueBit.HR.Docs.WWW
{
    public class MvcAppException : Exception
    {
    }

    public class LogonMvcAppException : MvcAppException
    {
        public string UserIdentifier { get; private set; }
        public static void Throw(string userIdentifier) { throw new LogonMvcAppException() { UserIdentifier = userIdentifier }; }
    }


    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log;
        private const string _SESSION_INI_ = "SESSION_INI";
        private const string _SESSION_CTX_ = "SESSION_CTX";

        static MvcApplication()
        {
            Log.Configure("WebLog.config");
            Log.AddPropertyInfo("USER", () => {
                var user = HttpContext.Current.User;
                return user == null ? string.Empty : user.Identity.Name;
            });
            Log.AddPropertyInfo("SESSION", () =>
            {
                var app = MvcApplication.GetAppInstance();
                return app == null ? string.Empty : app.SessionID;
            });
            log = Log.GetInstanceFor<MvcApplication>();
        }

        protected void Application_Start()
        {
            log._TraceEntry(() => {
                AreaRegistration.RegisterAllAreas();

                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
            });
        }

        protected void Session_Start()
        {
            log._TraceEntry(() => {});
            Session[_SESSION_INI_] = Guid.NewGuid();
        }

        protected void Session_Stop()
        {
            log._TraceEntry(() => {
                BusinessContext.Dispose();
            });
        }

        public static MvcApplication GetAppInstance()
        {
            var ctx = HttpContext.Current;
            return ctx == null ? null : (MvcApplication)ctx.ApplicationInstance;
        }

        public string SessionID 
        { 
            get 
            { 
                var ctx = HttpContext.Current;
                if (ctx == null) return string.Empty;
                var app = ctx.ApplicationInstance;
                if (app == null) return string.Empty;
                var session = app.Session;
                if (session == null) return string.Empty;
                return session.SessionID;
            } 
        }

        public BL.BusinessLayer.IBusinessContext BusinessContext { 
            get { return (BL.BusinessLayer.IBusinessContext)Session[_SESSION_CTX_]; }
            set { Session[_SESSION_CTX_] = value; }
        }

        public bool IsUserAuthenticated()
        {
            return BusinessContext != null;
        }

        public bool IsUserAdmin()
        {
            var businessCtx = BusinessContext;
            return businessCtx == null
                ? false
                : businessCtx.Session.Employee.IsAdministrator;
        }

        public bool IsUserCurrent(long id)
        {
            var businessCtx = BusinessContext;
            return businessCtx == null
                ? false
                : businessCtx.Session.Employee.ID == id;
        }

        public Tuple<string, bool?, DateTime?> GetUserInfo()
        {
            var identifier = HttpContext.Current.User.Identity.Name;

            var businessContext = BusinessContext;
            if (businessContext != null)
            {
                var session = businessContext.Session;
                return Tuple.Create<string, bool?, DateTime?>(identifier, session.Employee.IsAdministrator, session.DateStart);
            }
            return Tuple.Create<string, bool?, DateTime?>(identifier, null, null);
        }

        public void UserLogout()
        {
            var businessContext = BusinessContext;
            if (businessContext != null)
            {
                BusinessContext = null;
                businessContext.Dispose();
            }
        }

        public BL.DataLayer.Entities.Employee UserLogon(string PIN)
        {
            UserLogout();
            var identifier = HttpContext.Current.User.Identity.Name;
            try
            {
                BusinessContext = new BL.BusinessLayer.BusinessContext(e => e.Identifier == identifier && e.PIN == PIN);
            }
            catch (BL.BusinessLayer.LogonBusinessException)
            {
                LogonMvcAppException.Throw(identifier);
            }
            return BusinessContext.Session.Employee;
        }
    }
}