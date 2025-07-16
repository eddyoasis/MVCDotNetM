using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Services;

namespace MVCWebApp.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        List<string> _whiteList = ["/Login/Index", "/Login/Login", "/Login"];

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                BaseService.Username = context.HttpContext.User.Identity.Name;
            }

            //if (_whiteList.Contains(context.HttpContext.Request.Path.Value))
            //{
            //    return;
            //}

            //if (!context.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    context.HttpContext.Response.StatusCode = 401; // Set HTTP status code
            //    context.Result = new RedirectToActionResult("Index", "Login", null);
            //}
        }
    }
}
