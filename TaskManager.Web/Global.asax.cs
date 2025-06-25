using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TaskManager.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /* �reas (si las hubiera) */
            AreaRegistration.RegisterAllAreas();

            /* Rutas MVC */
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            /* Bundles  (CSS / JS) */
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
