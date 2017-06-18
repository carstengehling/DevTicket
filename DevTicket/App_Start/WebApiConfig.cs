using DevTicket.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DevTicket
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var ioc = new UnityContainer();
            ioc.RegisterType<ITicketRepository, TicketRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(ioc);


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional, action = "DefaultAction" }
            );
        }
    }
}
