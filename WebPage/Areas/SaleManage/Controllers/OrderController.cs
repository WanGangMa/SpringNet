using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPage.Areas.SaleManage.Controllers
{
    public class OrderController : Controller
    {
        // GET: SaleManage/Order
        public ActionResult Index()
        {
            return View();
        }
    }
}