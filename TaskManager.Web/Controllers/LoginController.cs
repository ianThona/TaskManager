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
        private readonly string _cs =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /*------------------------  GET  ------------------------*/
        [HttpGet]
        public ActionResult Login()
        {
            var users = LoadUserOptions().ToList();

            var vm = new LoginViewModel
            {
                UserOptions = users,
                Username = users.FirstOrDefault()?.Name,
                Password = users.FirstOrDefault()?.Password
            };

            ViewBag.UserPassJson = System.Web.Helpers.Json.Encode(
                                        users.ToDictionary(u => u.Name, u => u.Password));

            return View(vm);
        }

        /*------------------------  POST  -----------------------*/
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

            if (storedPassword != null && model.Password == storedPassword)
                return RedirectToAction(nameof(LoginSuccess));

            model.ErrorMessage = "Usuario o contraseña incorrectos";
            model.UserOptions = LoadUserOptions();
            return View(model);
        }

        public ActionResult LoginSuccess() =>
            Content("¡Has iniciado sesión correctamente!");

        /*--------------------  Helpers  ------------------------*/

        private IEnumerable<UserOption> LoadUserOptions()
        {
            var list = new List<UserOption>();

            using (var conn = new NpgsqlConnection(_cs))
            {
                conn.Open();

                const string sql =
                    "SELECT name, password FROM public.users ORDER BY name";

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
            return list;   // IEnumerable<UserOption>
        }

        private string GetPassword(string username)
        {
            using (var conn = new NpgsqlConnection(_cs))
            {
                conn.Open();

                const string sql =
                    "SELECT password FROM public.users WHERE name = @u";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("u", username);
                    return cmd.ExecuteScalar() as string; // null si no existe
                }
            }
        }
    }
}
