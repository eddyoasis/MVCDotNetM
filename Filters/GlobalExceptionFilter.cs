using Microsoft.AspNetCore.Mvc.Filters;

namespace MVCWebApp.Filters
{
    public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> _logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled Exception");
        }
    }
}
