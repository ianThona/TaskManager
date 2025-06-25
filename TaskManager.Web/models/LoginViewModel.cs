// Models/LoginViewModel.cs
using System.Collections.Generic;
using System.Linq;          // ← para Enumerable.Empty

namespace TaskManager.Web.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Lista de usuarios que se mostrará en el <select>.
        /// IEnumerable es suficiente para recorrer y usar LINQ,
        /// y evita el error de conversión al asignar desde LoadUserOptions().
        /// </summary>
        public IEnumerable<UserOption> UserOptions { get; set; } = Enumerable.Empty<UserOption>();
    }
}
