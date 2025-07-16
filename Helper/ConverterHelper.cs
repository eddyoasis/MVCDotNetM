using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Helper
{
    public class ConverterHelper
    {
        public static bool MatchesSearchLowerString(string source, string target)
        {
            return string.IsNullOrEmpty(source) || target?.ToLower().Contains(source, StringComparison.CurrentCultureIgnoreCase) == true;
        }

        public static List<SelectListItem> ToSelectList(IEnumerable<string> listString)
        {
            return 
                new[] { "All" }
                .Concat(listString ?? Enumerable.Empty<string>())
                .Select(item => new SelectListItem
                {
                    Text = item,
                    Value = item
                })
                .ToList();
        }

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
