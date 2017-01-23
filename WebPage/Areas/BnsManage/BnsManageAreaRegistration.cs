using System.Web.Mvc;

namespace WebPage.Areas.BnsManage
{
    public class BnsManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BnsManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BnsManage_default",
                "Bns/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] {"WebPage.Areas.BnsManage.Controllers" }
            );
        }
    }
}