using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class PatientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
