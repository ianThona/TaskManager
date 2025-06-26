using System.Web.Mvc;

namespace TaskManager.Web.Controllers
{
    public class BacklogController : Controller
    {
        public ActionResult Index()   // /Task/Backlog
        {
            return View();            // Views/Backlog/Index.cshtml
        }
    }
}
