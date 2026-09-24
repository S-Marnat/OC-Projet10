using Microsoft.AspNetCore.Mvc;

namespace Risques.Controllers
{
    public class RisqueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
