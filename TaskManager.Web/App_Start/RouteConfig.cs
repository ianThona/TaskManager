/*  App_Start/RouteConfig.cs  */

using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*------------------------------------------------------------
             *  Ignorar trazas y web-resources (*.axd)
             *-----------------------------------------------------------*/
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*------------------------------------------------------------
             *  1 · Rutas específicas de Task
             *-----------------------------------------------------------*/

            //  /Task/Backlog            → BacklogController.Index
            routes.MapRoute(
                name: "TaskBacklog",
                url: "Task/Backlog",
                defaults: new { controller = "Backlog", action = "Index" }
            );

            //  /Task/KanbanBoard        → KanbanBoardController.Index
            routes.MapRoute(
                name: "TaskKanbanBoard",
                url: "Task/KanbanBoard",
                defaults: new { controller = "KanbanBoard", action = "Index" }
            );

            /*------------------------------------------------------------
             *  2 · Ruta por defecto (login)
             *-----------------------------------------------------------*/
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
