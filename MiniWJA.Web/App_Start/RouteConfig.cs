using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MiniWJA.Web
{
    /// <summary>
    /// Configures the routing for the ASP.NET MVC application.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the application's routes.
        /// </summary>
        /// <param name="routes">The route collection to configure.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes), "RouteCollection cannot be null.");
            }

            try
            {
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Devices", action = "Index", id = UrlParameter.Optional }
                );
            }
            catch (Exception ex)
            {
                // Log the exception as needed (e.g., using a logging framework)
                throw new InvalidOperationException("An error occurred while registering routes.", ex);
            }
        }
    }
}