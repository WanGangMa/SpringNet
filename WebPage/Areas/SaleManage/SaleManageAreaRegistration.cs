using System.Web.Mvc;

namespace WebPage.Areas.BnsManage
{
    public class SaleManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SaleManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SaleManage_default",
                "Sale/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] {"WebPage.Areas.SaleManage.Controllers" }
            );
        }
    }
}