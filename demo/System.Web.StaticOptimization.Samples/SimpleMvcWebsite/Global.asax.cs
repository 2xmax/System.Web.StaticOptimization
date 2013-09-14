using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleMvcWebsite
{
    using System.Web.StaticOptimization.Mvc;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
#if DEBUG
            BundleTable.Init(System.Web.HttpContext.Current.Server.MapPath("~/bundles.config"));
#endif
        }
    }
}