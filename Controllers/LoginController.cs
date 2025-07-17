using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Helper;
using MVCWebApp.Services;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Controllers
{
    public class LoginController(
        IAuthService authService,
        ILogger<LoginController> logger) : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            logger.LogInformation("Login page visited at {Time}", DateTime.UtcNow);

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginReq)
        {
            var token = await authService.AuthenticateAndGetUser(loginReq.Username, loginReq.Password);
            if (token.IsNotNullOrEmpty())
            {
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production (HTTPS required)
                    Expires = DateTime.UtcNow.AddHours(1) // Cookie expires in 1 hour
                });

                return Json(new { isSuccess = true });
            }

            return Json(new { isSuccess = false });
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Index", "Login");
        }
    }
}
