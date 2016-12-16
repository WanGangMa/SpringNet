using System;
using System.Web.Mvc;

namespace WebPage.Areas.ProManage
{
    public class ProManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ProManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("ProManage_default", "Pro/{controller}/{action}/{id}", new
            {
                action = "Index",
                id = UrlParameter.Optional
            }, new string[]
            {
                "WebPage.Areas.ProManage.Controllers"
            });
        }
    }
}
