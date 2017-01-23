using System.Web.Mvc;
using System.Web.Routing;

namespace WebPage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                //routes.MapRoute(
                //    name: "Default2",
                //    url: "{name}/{year}/{month}/{title}",
                //    defaults: new { controller = "Customer", action = "Index2", id = UrlParameter.Optional }
                //    ).DataTokens.Add("Area", "SaleManage"
                //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
                ).DataTokens.Add("Area", "SysManage"
            );

        }
    }
}
