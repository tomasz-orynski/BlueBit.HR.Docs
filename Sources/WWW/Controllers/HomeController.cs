using System.Linq;
using System.Web.Mvc;
using BlueBit.HR.Docs.BL.Diagnostics;

namespace BlueBit.HR.Docs.WWW.Controllers
{
    public class HomeController : Code.CommonController<HomeController>
    {
        [HttpGet]
        [Authorize]
        public ActionResult LogOn()
        {
            return _HandleAnonymousRequest(() => {
                    var model = new Models.Home.LogOn();
                    return View(model);
                });
        }

        [HttpPost]
        [Authorize]
        public ActionResult LogOn(Models.Home.LogOn model)
        {
            return _HandleAnonymousRequest(() => {
                if (!ModelState.Values.SelectMany(v => v.Errors).Any())
                    {
                        try
                        {
                            var e = MvcApplication.GetAppInstance().UserLogon(model.PIN);
                            if (e.IsLogonMailSend)
                                SendMail(e.Identifier, string.Format("Zalogowano się w HRDOCS na twoje konto."));
                            return RedirectToAction("Index");
                        }
                        catch (LogonMvcAppException e)
                        {
                            ModelState.AddModelError("", "Nie masz prawa dostępu do aplikacji lub podałeś nie właściwy PIN.");
                            SendMail(e.UserIdentifier, string.Format("Próbowano bez skutku zalogować się w HRDOCS na twoje konto."));
                        }
                    }
                    return View(model);
                });
        }

        [Authorize]
        public ActionResult LogOut()
        {
            return _HandleAnonymousRequest(() => {
                MvcApplication.GetAppInstance().BusinessContext = null;
                return RedirectToAction("LogOn", "Home");
            });
        }

        [Authorize]
        public ActionResult Index()
        {
            return _HandleUserRequest(() => {
                return RedirectToAction("Index", "Documents");
            });
        }

        private void SendMail(string userIdentifier, string body)
        {
            log._TraceEntry(() => {
                var server = Properties.Settings.Default.SMTPServer;
                var from = Properties.Settings.Default.SMTPFrom;
                var to = GetUserEMail(userIdentifier);

                var klient = new System.Net.Mail.SmtpClient(server);
                klient.Send(from, to, "Powiadomienie z HRDOCS", body);
            });
        }

        private static string GetUserEMail(string userIdentifier)
        {
            var nameTypeNT = (int)ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_NT4;
            var nameTypeUserPrincipalName = (int)ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_USER_PRINCIPAL_NAME;

            var trans = new ActiveDs.NameTranslate();
            try
            {
                trans.Set(nameTypeNT, userIdentifier);
                return trans.Get(nameTypeUserPrincipalName);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(trans);
            }
        }
    }
}
