namespace PortalAgentesThona.Models.Backlog    // ← coloca el namespace que uses
{
    /// <summary>
    /// Representa una fila en la tabla de tareas del Backlog.
    /// </summary>
    public class BacklogTaskModel
    {
        /// <summary>
        /// Número consecutivo mostrado en la primera columna.
        /// </summary>
        public int NumeroRegistro { get; set; }

        /// <summary>
        /// Título o resumen de la tarea.
        /// </summary>
        public string Titulo { get; set; }
    }
}
