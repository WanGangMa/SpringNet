using System;
using System.Web.Mvc;

namespace WebPage.Areas.MailManage
{
    public class MailManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MailManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("MailManage_default", "Mail/{controller}/{action}/{id}", new
            {
                action = "Index",
                id = UrlParameter.Optional
            }, new string[]
            {
                "WebPage.Areas.MailManage.Controllers"
            });
        }
    }
}
