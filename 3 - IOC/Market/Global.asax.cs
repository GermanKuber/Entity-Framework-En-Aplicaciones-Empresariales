﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Market
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //TODO : 1 - Instalo Install-Package structuremap
        //Install-Package StructureMap.MVC5
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}