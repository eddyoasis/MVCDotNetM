using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;
using MVCWebApp.Services;
using System.DirectoryServices.AccountManagement;

namespace MVCWebApp.Controllers
{
    public class LoginController(
        IAuthService authService,
        ILogger<LoginController> logger) : Controller
    {
        private static string _domain = "60.250.169.233";

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            logger.LogInformation("Login page visited at {Time}", DateTime.UtcNow);

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginReq)
        {
            var token = await authService.Login(loginReq.Username);

            // Set cookie on successful login
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true in production (HTTPS required)
                Expires = DateTime.UtcNow.AddHours(1) // Cookie expires in 1 hour
            });

            return RedirectToAction("Index", "MarginCall");
        }

        public bool ValidateUser(string domain, string username, string password)
        {
            using var context = new PrincipalContext(ContextType.Domain, domain);
            return context.ValidateCredentials(username, password);
        }

        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
