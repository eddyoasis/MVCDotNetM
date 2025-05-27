using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Helper
{
    public class ConverterHelper
    {
        public static List<SelectListItem> ToSelectList<TEnum>(bool useDisplayName = true) where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Text = useDisplayName ? GetDisplayName(e) : e.ToString(),
                    Value = Convert.ToInt32(e).ToString()
                })
                .ToList();
        }

        private static string GetDisplayName<TEnum>(TEnum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var displayAttribute = member?.GetCustomAttributes(typeof(DisplayAttribute), false)
                                         .FirstOrDefault() as DisplayAttribute;
            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
