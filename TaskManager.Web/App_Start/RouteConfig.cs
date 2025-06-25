using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Ignorar los .axd (trazas, web resources, etc.)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ruta por defecto: ahora va a /Login/Login
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Login",
                    action = "Login",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}
