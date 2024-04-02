using Microsoft.AspNetCore.Mvc;

namespace AspnetIdentityRoleBasedTutorial.Controllers
{
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
