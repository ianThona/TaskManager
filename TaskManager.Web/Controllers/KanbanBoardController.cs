using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using Npgsql;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
    public class KanbanBoardController : Controller
    {
        // Cadena de conexión (idéntica a la de BacklogController)
        private readonly string _cs =
            ConfigurationManager
              .ConnectionStrings["DefaultConnection"]
              .ConnectionString;

        // GET /Task/KanbanBoard
        [HttpGet]
        public ActionResult Index()
        {
            // 1) Carga todas las tareas
            var tareas = LoadAllTasks();
            // 2) Pásalas a la vista
            return View(tareas);
        }

        // Helper privado para leer todas las tareas
        private IEnumerable<Task> LoadAllTasks()
        {
            var list = new List<Task>();
            using (var cn = new NpgsqlConnection(_cs))
            {
                cn.Open();
                const string sql = @"
SELECT idtask, titulo, descripcion,
       asignacion, vencimiento, estado
  FROM public.task
 ORDER BY idtask;";
                using (var cmd = new NpgsqlCommand(sql, cn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new Task
                        {
                            IdTask = rd.GetInt32(0),
                            Titulo = rd.GetString(1),
                            Descripcion = rd.IsDBNull(2) ? null : rd.GetString(2),
                            Asignacion = rd.GetDateTime(3),
                            Vencimiento = rd.GetDateTime(4),
                            Estado = rd.IsDBNull(5) ? null : rd.GetString(5)
                        });
                    }
                }
            }
            return list;
        }
    }
}
