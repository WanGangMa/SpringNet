using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.SysManage.Controllers
{
    public class ChatController : BaseController
    {
        public ActionResult Index()
        {
            return base.View();
        }
    }
}
