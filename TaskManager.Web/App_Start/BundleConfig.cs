using System.Web.Optimization;

namespace TaskManager.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* 1 · Bundle con librerías JS comunes */
            bundles.Add(
                new ScriptBundle("~/bundles/lib")
                    .Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.bundle.min.js"
                    )
            );

            /* 2 · CSS global */
            bundles.Add(
                new StyleBundle("~/Content/bootstrap")
                    .Include("~/Content/bootstrap.min.css")
            );
            bundles.Add(
                new StyleBundle("~/Content/site")
                    .Include("~/Content/site.css")
            );

            /* 3 · Login */
            bundles.Add(
                new StyleBundle("~/Content/login-css")
                    .Include("~/Content/Login/css/login.css")
            );
            bundles.Add(
                new ScriptBundle("~/bundles/login-js")
                    .Include("~/Content/Login/js/login.js")
            );

            /* 4 · Side-menu */
            bundles.Add(
                new StyleBundle("~/Content/sideMenu-css")
                    .Include("~/Content/SideMenu/css/sideMenu.css")
            );
            bundles.Add(
                new ScriptBundle("~/bundles/sideMenu-js")
                    .Include("~/Content/SideMenu/js/sideMenu.js")
            );

            /* 5 · Backlog */
            bundles.Add(
                new StyleBundle("~/Content/backlog-css")
                    .Include("~/Content/Backlog/css/backlog.css")
            );
            bundles.Add(
                new ScriptBundle("~/bundles/backlog-js")
                    .Include("~/Content/Backlog/js/backlog.js")
            );

            /* 6 · KanbanBoard */
            bundles.Add(
                new StyleBundle("~/Content/kanbanBoard-css")
                    .Include("~/Content/KanbanBoard/css/kanbanBoard.css")
            );
            bundles.Add(
                new ScriptBundle("~/bundles/kanbanBoard-js")
                    .Include("~/Content/KanbanBoard/js/kanbanBoard.js")
            );

#if DEBUG
            BundleTable.EnableOptimizations = false;  // No minificar en Debug
#else
            BundleTable.EnableOptimizations = true;   // Sí minificar en Release
#endif
        }
    }
}
