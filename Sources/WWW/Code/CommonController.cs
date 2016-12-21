using System;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using BlueBit.HR.Docs.BL.Diagnostics;

namespace BlueBit.HR.Docs.WWW.Code
{
    public class CommonController<T> : Controller
        where T : CommonController<T>
    {
        protected static readonly log4net.ILog log = Log.GetInstanceFor<T>();

        protected ActionResult _HandleAnonymousRequest(Func<ActionResult> request)
        {
            Contract.Requires(request != null);
            return log._TraceEntryWithResult(request, 1);
        }
        protected ActionResult _HandleUserRequest(Func<ActionResult> request)
        {
            Contract.Requires(request != null);
            return log._TraceEntryWithResult(() => {
                var app = MvcApplication.GetAppInstance();
                if (!app.IsUserAuthenticated()) return RedirectToAction("Logon", "Home");
                return request();
            }, 1);
        }
        protected ActionResult _HandleAdminRequest(Func<ActionResult> request)
        {
            Contract.Requires(request != null);
            return log._TraceEntryWithResult(() => {
                var app = MvcApplication.GetAppInstance();
                if (!app.IsUserAuthenticated()) return RedirectToAction("Logon", "Home");
                if (!app.IsUserAdmin()) return RedirectToAction("Index");
                return request();
            }, 1);
        }
    }
}
