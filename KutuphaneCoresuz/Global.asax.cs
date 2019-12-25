using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KutuphaneCoresuz
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        ////
        /////// SOOOOOORRR!!!!!!
        ///
        //public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //{
        //    filters.Add(new HandleErrorAttribute());

        //}
        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{ resource}.axd/{*pathInfo}");
        //    routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Security", action = "anasayfa", id = UrlParameter.Optional });
        //}

    }

}