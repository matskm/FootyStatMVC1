using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FootyStatMVC1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "SelectPlayer",                                           // Route name
            //    "PlayerStat/{action}/{id}",                            // URL with parameters
            //    new { controller = "PlayerStat", action = }  // Parameter defaults
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //defaults: new { controller = "PlayerStat", action = "SelectPlayer", id = UrlParameter.Optional }
            );
        }
    }
}