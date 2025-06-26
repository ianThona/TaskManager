using System.Web.Mvc;

namespace TaskManager.Web.Controllers
{
    public class KanbanBoardController : Controller
    {
        public ActionResult Index()   // /Task/KanbanBoard
        {
            return View();            // Views/KanbanBoard/Index.cshtml
        }
    }
}
