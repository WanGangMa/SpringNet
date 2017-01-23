using System.Web.Mvc;

namespace WebPage.Areas.FinaManage.Controllers
{
    public class StatementController : WebPage.Controllers.BaseController
    {
        // GET: FinaManage/Statement
        public ActionResult Index()
        {
            return View();
        }
    }
}