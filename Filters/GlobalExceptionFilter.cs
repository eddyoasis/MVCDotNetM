using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace MVCWebApp.Filters
{
    public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> _logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            Log.Error("Execption: {Controller}.{Action} ex: {Exception}", controller, action, context.Exception);
        }
    }
}
