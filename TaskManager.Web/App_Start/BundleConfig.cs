/*  App_Start/BundleConfig.cs  */

using System.Web.Optimization;

namespace TaskManager.Web
{
    public static class BundleConfig
    {
        /// <summary>
        /// Registra todos los bundles de hojas de estilo y JavaScript.
        /// Llamar desde Global.asax → Application_Start().
        /// </summary>
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*------------------------------------------------------------
             *  1 · Librerías JS compartidas
             *-----------------------------------------------------------*/
            bundles.Add(new ScriptBundle("~/bundles/lib")
                .Include("~/Scripts/jquery-3.7.1.js",
                         "~/Scripts/bootstrap.bundle.js"));   // incluye Popper

            /*------------------------------------------------------------
             *  2 · Estilos globales (Bootstrap + site.css)
             *-----------------------------------------------------------*/
            bundles.Add(new StyleBundle("~/Content/bootstrap")
                .Include("~/Content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/site")
                .Include("~/Content/site.css"));

            /*------------------------------------------------------------
             *  3 · Página de Login  (CSS y JS propios)
             *-----------------------------------------------------------*/
            bundles.Add(new StyleBundle("~/Content/login-css")
                .Include("~/Content/Login/css/login.css"));

            bundles.Add(new ScriptBundle("~/bundles/login-js")
                .Include("~/Content/Login/js/login.js"));

            /*------------------------------------------------------------
             *  4 · Side-menu (Backlog / Kanban board)
             *-----------------------------------------------------------*/
            bundles.Add(new StyleBundle("~/Content/sideMenu-css")
                .Include("~/Content/SideMenu/css/sideMenu.css"));

            bundles.Add(new ScriptBundle("~/bundles/sideMenu-js")
                .Include("~/Content/SideMenu/js/sideMenu.js"));

            /*------------------------------------------------------------
             *  5 · Optimización siempre activa
             *-----------------------------------------------------------*/
            BundleTable.EnableOptimizations = true;
        }
    }
}
