using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace MVCWebApp.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        private static readonly string[] SensitiveFields = ["Password"];

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            var requests = new StringBuilder();

            foreach (var arg in context.ActionArguments)
            {
                var obj = arg.Value;
                if (obj == null)
                    continue;

                var json = JsonConvert.SerializeObject(obj);
                var clone = JsonConvert.DeserializeObject(json, obj.GetType());

                var type = clone.GetType();

                foreach (var field in SensitiveFields)
                {
                    var prop = type.GetProperty(field);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(clone, "***MASKED***");
                    }
                }

                requests.AppendLine(JsonConvert.SerializeObject(clone, Formatting.Indented));
            }

            Log.Information("➡️ START: {Controller}.{Action} req: {Request}", controller, action, requests.ToString());
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            Log.Information($"✅ END: {controller}.{action}");
        }
    }
}