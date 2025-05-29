using Microsoft.AspNetCore.Authentication.Cookies;
using MVCWebApp.Helper;
using MVCWebApp.Services;
using System.Security.Claims;

namespace MVCWebApp.Middlewares
{
    public class CookieAuthMiddleware(
        RequestDelegate next)
    {
        List<string> _whiteList = 
            ["/Login/Index",
            "/Margin/Logout",
            "/Login",
            "/Login/Login"];

        public async Task Invoke(HttpContext context)
        {
            // Detect AJAX requests to handle different response types
            bool isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (_whiteList.Contains(context.Request.Path.Value))
            {
                await next(context);
                return;
            }

            if (!context.User.Identity.IsAuthenticated) // Always false at this stage
            {
                if (context.Request.Cookies.TryGetValue("AuthToken", out var authToken))
                {
                    var username = JwtTokenHelper.DecodeJwtToken(authToken);

                    if (!string.IsNullOrEmpty(username))
                    {
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, username),
                                new Claim(ClaimTypes.Email, $"{username}@mail.com")
                            };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        context.User = principal;

                        BaseService.Username = username;
                    }

                    await next(context);
                    return;
                }

                if (isAjax)
                {
                    context.Response.StatusCode = 401; // Set status code
                    await context.Response.WriteAsync("Unauthorized"); // Send response message
                    return;
                }
                else
                {
                    context.Response.Redirect("/Login/Index");
                    return;
                }
            }

            if (isAjax)
            {
                context.Response.StatusCode = 401; // Set status code
                await context.Response.WriteAsync("Unauthorized"); // Send response message
                return;
            }
            else
            {
                context.Response.Redirect("/Login/Index");
                return;
            }
        }
    }
}
