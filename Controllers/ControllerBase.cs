using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVCWebApp.Controllers
{
    public class ControllerBase : Controller
    {
        protected string Username => User.FindFirst(ClaimTypes.Name)?.Value ?? "Guest";
        protected string Email => User.FindFirst(ClaimTypes.Email)?.Value;
    }
}
