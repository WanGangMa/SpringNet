using Autofac;
using Autofac.Integration.Mvc;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Service;

namespace WebPage
{
    public class MvcApplication : /*Spring.Web.Mvc.SpringMvcApplication // */System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //autofac自动注入
            var builder = new ContainerBuilder();
            //builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            //var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(e => typeof(IAutofac).IsAssignableFrom(e) && e != typeof(IAutofac))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
