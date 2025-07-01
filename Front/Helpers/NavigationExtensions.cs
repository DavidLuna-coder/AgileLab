using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Front.Helpers
{
    public static class NavigationExtensions
    {
        public static void NavigateTo<T>(this NavigationManager Nav, Dictionary<string, object>? parameters = null, bool forceLoad = false) where T : IComponent
        {
            string url = GetUrl<T>(Nav, parameters);
            Nav.NavigateTo(url, forceLoad);
        }
        public static string GetUrl<T>(this NavigationManager Nav, Dictionary<string, object>? parameters = null) where T : IComponent
        {
            Type component = typeof(T);
            RouteAttribute routeAttribute = component.GetCustomAttribute<RouteAttribute>() ?? throw new TypeAccessException("The component has no routes");

            string url = routeAttribute.Template;

            if (parameters != null)
            {
                // Regex to match {ParamName:constraint} or {ParamName}
                var regex = new Regex(@"{(?<name>\w+)(:(?<constraint>\w+))?}");
                url = regex.Replace(url, match => {
                    var name = match.Groups["name"].Value;
                    if (parameters.TryGetValue(name, out var value))
                    {
                        return value?.ToString() ?? string.Empty;
                    }
                    // If not provided, leave the placeholder as is
                    return match.Value;
                });
            }

            return url;
        }
    }
}
