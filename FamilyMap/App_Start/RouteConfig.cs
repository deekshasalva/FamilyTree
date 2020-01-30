using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FamilyMap
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
       

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "FamilyMapAdmin",
            url: "students/{action}",
            defaults: new { controller = "FamilyMap", action = "HomePageForAdmin" }
        );

            routes.MapRoute(
            name: "FamilyMapUser",
            url: "students/{action}",
            defaults: new { controller = "FamilyMap", action = "HomePageForUser" }
        );
        }
    }
}
