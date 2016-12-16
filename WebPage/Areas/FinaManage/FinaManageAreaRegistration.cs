using System.Web.Mvc;

namespace WebPage.Areas.FinaManage
{
    public class FinaManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FinaManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FinaManage_default",
                "Fina/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] {"WebPage.Areas.FinaManage.Controllers" }
            );
        }
    }
}