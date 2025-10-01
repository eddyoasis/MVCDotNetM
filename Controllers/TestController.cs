using Microsoft.AspNetCore.Mvc;

namespace MVCWebApp.Controllers
{
    public class TestController() : Controller
    {
        [HttpPost]
        public async Task<IActionResult> TestPOST()
        {
            return Json(new { isSuccess = true });
        }
    }
}
