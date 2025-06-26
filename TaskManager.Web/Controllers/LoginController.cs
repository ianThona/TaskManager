using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Npgsql;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
    public class LoginController : Controller
    {
        /*────────────────────────  CADENA DE CONEXIÓN  ───────────────────────*/
        private readonly string _cs =
            ConfigurationManager.ConnectionStrings["DefaultConnection"]
                                 .ConnectionString;

        /*──────────────────────────────  GET  /Login  ─────────────────────────*/
        [HttpGet]
        public ActionResult Login()
        {
            var users = LoadUserOptions().ToList();

            var vm = new LoginViewModel
            {
                UserOptions = users,
                // Autocompleta el primer usuario (opcional)
                Username = users.FirstOrDefault()?.Name,
                Password = users.FirstOrDefault()?.Password
            };

            // Envía al cliente un diccionario { usuario : contraseña } (opcional)
            ViewBag.UserPassJson = System.Web.Helpers.Json.Encode(
                                        users.ToDictionary(u => u.Name, u => u.Password));

            // Si había una sesión anterior, la cerramos
            Session.Abandon();

            return View(vm);
        }

        /*──────────────────────────────  POST /Login  ─────────────────────────*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.UserOptions = LoadUserOptions();
                return View(model);
            }

            var storedPassword = GetPassword(model.Username);
            var ok = storedPassword != null && model.Password == storedPassword;

            if (ok)
            {
                // Guarda datos mínimos en sesión
                Session["Username"] = model.Username;
                Session["LoginTime"] = DateTime.UtcNow;

                // Redirige al primer módulo de la aplicación
                return RedirectToAction("Backlog", "Task");
            }

            // Credenciales inválidas
            model.ErrorMessage = "Usuario o contraseña incorrectos";
            model.UserOptions = LoadUserOptions();
            return View(model);
        }

        /*───────────────────────────────  GET /Logout  ────────────────────────*/
        public ActionResult Logout()
        {
            Session.Abandon();                 // destruye la sesión
            return RedirectToAction("Login");  // vuelve al formulario de login
        }

        /*─────────────────────  Helper opcional para AJAX  ───────────────────*/
        /// <summary>Devuelve true/false si hay usuario logueado.</summary>
        public JsonResult IsAuthenticated() =>
            Json(Session["Username"] != null, JsonRequestBehavior.AllowGet);

        /*───────────────────────────────  Helpers DB  ─────────────────────────*/
        private IEnumerable<UserOption> LoadUserOptions()
        {
            var list = new List<UserOption>();

            using (var conn = new NpgsqlConnection(_cs))
            {
                conn.Open();
                const string sql = "SELECT name, password FROM public.users ORDER BY name";

                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new UserOption
                        {
                            Name = rdr.GetString(0),
                            Password = rdr.GetString(1)
                        });
                    }
                }
            }
            return list;
        }

        private string GetPassword(string username)
        {
            using (var conn = new NpgsqlConnection(_cs))
            {
                conn.Open();
                const string sql = "SELECT password FROM public.users WHERE name = @u";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("u", username);
                    return cmd.ExecuteScalar() as string;  // null si no existe
                }
            }
        }
    }
}
