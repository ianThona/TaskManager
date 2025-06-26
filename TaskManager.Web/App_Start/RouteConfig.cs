// File: App_Start/RouteConfig.cs

using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*───────────────────────────────────────────────────────────────
             * Ignorar trazas y web-resources (*.axd)
             *───────────────────────────────────────────────────────────────*/
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*───────────────────────────────────────────────────────────────
             * 1 · Rutas específicas de “Task / Backlog”
             *    (el orden importa: primero las más concretas)
             *───────────────────────────────────────────────────────────────*/

            // POST /Task/Backlog/Update → BacklogController.Update
            routes.MapRoute(
                name: "TaskBacklogUpdate",
                url: "Task/Backlog/Update",
                defaults: new { controller = "Backlog", action = "Update" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
            );

            // POST /Task/Backlog/Delete → BacklogController.Delete
            routes.MapRoute(
                name: "TaskBacklogDelete",
                url: "Task/Backlog/Delete",
                defaults: new { controller = "Backlog", action = "Delete" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
            );

            // GET /Task/Backlog → BacklogController.Index
            routes.MapRoute(
                name: "TaskBacklog",
                url: "Task/Backlog",
                defaults: new { controller = "Backlog", action = "Index" }
            );

            // GET /Task/KanbanBoard → KanbanBoardController.Index
            routes.MapRoute(
                name: "TaskKanbanBoard",
                url: "Task/KanbanBoard",
                defaults: new { controller = "KanbanBoard", action = "Index" }
            );

            /*───────────────────────────────────────────────────────────────
             * 2 · Ruta por defecto (login)
             *───────────────────────────────────────────────────────────────*/
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
