using System.Web.Mvc;

namespace BlueBit.HR.Docs.WWW.Controllers
{
    public class ExtInfoController : Code.CommonController<ExtInfoController>
    {
        [Authorize]
        public ActionResult Index()
        {
            return _HandleUserRequest(() => {
                var logic = new Models.Documents.Logic();
                return View(logic.GetEmployeeExtInfo());
            });
        }
    }
}
