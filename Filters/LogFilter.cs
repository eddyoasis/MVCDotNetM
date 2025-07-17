using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;

namespace MVCWebApp.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requests = string.Empty;

            context.ActionArguments.ToList().ForEach(x =>
            {
                requests += JsonConvert.SerializeObject(x.Value, Formatting.Indented);
            });

            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            Log.Information(@$"➡️ START: {controller}.{action} req: {requests}");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            Log.Information($"✅ END: {controller}.{action}");
        }
    }
}