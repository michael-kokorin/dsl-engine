namespace Modules.UI.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public sealed class HomeController : Controller
    {
        public ActionResult Index() => RedirectToAction("Index", "Project");

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}