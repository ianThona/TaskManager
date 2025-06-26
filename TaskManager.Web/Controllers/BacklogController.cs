using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using Npgsql;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
    public class BacklogController : Controller
    {
        // Cadena de conexión
        private readonly string _cs =
            ConfigurationManager.ConnectionStrings["DefaultConnection"]
                                 .ConnectionString;

        // GET /Task/Backlog
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Tasks = LoadAllTasks();
            return View();   // Views/Backlog/Index.cshtml
        }

        // POST /Task/Backlog/Create
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Create(Task model)
        {
            // validar + parsear fecha
            if (!DateTime.TryParseExact(Request["Vencimiento"], "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var vto))
                return Json(new { ok = false, msg = "Fecha inválida" });

            if (string.IsNullOrWhiteSpace(model.Titulo))
                return Json(new { ok = false, msg = "El título es obligatorio" });

            if (vto <= DateTime.Today)
                return Json(new { ok = false, msg = "La fecha de vencimiento debe ser mayor a hoy" });

            // insertar
            int newId;
            using (var cn = new NpgsqlConnection(_cs))
            {
                cn.Open();
                const string sql = @"
INSERT INTO public.task (titulo, descripcion, asignacion, vencimiento, estado)
VALUES (@t, @d, CURRENT_DATE, @v, @e)
RETURNING idtask;";
                using (var cmd = new NpgsqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("t", model.Titulo);
                    cmd.Parameters.AddWithValue("d", string.IsNullOrWhiteSpace(model.Descripcion)
                                                       ? (object)DBNull.Value
                                                       : model.Descripcion);
                    cmd.Parameters.AddWithValue("v", vto);
                    cmd.Parameters.AddWithValue("e", string.IsNullOrWhiteSpace(model.Estado)
                                                       ? "Pendiente"
                                                       : model.Estado);
                    newId = (int)cmd.ExecuteScalar();
                }
            }

            // devolver JSON
            return Json(new
            {
                ok = true,
                task = new
                {
                    id = newId,
                    titulo = model.Titulo,
                    descripcion = model.Descripcion,
                    asignacion = DateTime.Today.ToString("yyyy-MM-dd"),
                    vencimiento = vto.ToString("yyyy-MM-dd"),
                    estado = string.IsNullOrWhiteSpace(model.Estado) ? "Pendiente" : model.Estado
                }
            });
        }

        // POST /Task/Backlog/Update
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(Task model)
        {
            // validar
            if (model.IdTask <= 0)
                return Json(new { ok = false, msg = "Id inválido" });

            if (string.IsNullOrWhiteSpace(model.Titulo))
                return Json(new { ok = false, msg = "El título es obligatorio" });

            // parsear fecha
            if (!DateTime.TryParseExact(Request["Vencimiento"], "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var vto))
                return Json(new { ok = false, msg = "Fecha inválida" });

            if (vto <= DateTime.Today)
                return Json(new { ok = false, msg = "La fecha de vencimiento debe ser mayor a hoy" });

            // actualizar
            int rows;
            using (var cn = new NpgsqlConnection(_cs))
            {
                cn.Open();
                const string sql = @"
UPDATE public.task
   SET titulo      = @t,
       descripcion = @d,
       vencimiento = @v,
       estado      = @e
 WHERE idtask      = @id;";
                using (var cmd = new NpgsqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("id", model.IdTask);
                    cmd.Parameters.AddWithValue("t", model.Titulo);
                    cmd.Parameters.AddWithValue("d", string.IsNullOrWhiteSpace(model.Descripcion)
                                                       ? (object)DBNull.Value
                                                       : model.Descripcion);
                    cmd.Parameters.AddWithValue("v", vto);
                    cmd.Parameters.AddWithValue("e", string.IsNullOrWhiteSpace(model.Estado)
                                                       ? "Pendiente"
                                                       : model.Estado);
                    rows = cmd.ExecuteNonQuery();
                }
            }

            if (rows == 0)
                return Json(new { ok = false, msg = "No se encontró la tarea" });

            // devolver JSON con los datos actualizados
            return Json(new
            {
                ok = true,
                task = new
                {
                    id = model.IdTask,
                    titulo = model.Titulo,
                    descripcion = model.Descripcion,
                    asignacion = model.Asignacion.ToString("yyyy-MM-dd"),
                    vencimiento = vto.ToString("yyyy-MM-dd"),
                    estado = string.IsNullOrWhiteSpace(model.Estado) ? "Pendiente" : model.Estado
                }
            });
        }

        // POST /Task/Backlog/Delete
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            if (id <= 0)
                return Json(new { ok = false, msg = "Id inválido" });

            int rows;
            using (var cn = new NpgsqlConnection(_cs))
            {
                cn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM public.task WHERE idtask = @id;", cn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    rows = cmd.ExecuteNonQuery();
                }
            }
            return Json(new { ok = rows > 0, msg = rows > 0 ? "Eliminado" : "No se encontró la tarea", id });
        }

        // Helper: leer todas las tareas
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
