using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspnetIdentityRoleBasedTutorial.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        public IActionResult GetAll()
        {
            return View();
        }
    }
}
