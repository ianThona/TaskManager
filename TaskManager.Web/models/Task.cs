using System;

namespace TaskManager.Web.Models
{
    public class Task
    {
        public int IdTask { get; set; }   // idtask
        public string Titulo { get; set; }   // titulo
        public string Descripcion { get; set; }   // descripcion
        public DateTime Asignacion { get; set; }   // asignacion
        public DateTime Vencimiento { get; set; }   // vencimiento
        public string Estado { get; set; }   // estado
    }
}
