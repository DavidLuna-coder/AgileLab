using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Front.Helpers
{
    public static class NavigationExtensions
    {
		public static void NavigateTo<T>(this NavigationManager Nav, Dictionary<string, string>? parameters = null, bool forceLoad = false) where T : IComponent
		{
			string url = GetUrl<T>(Nav, parameters);
			Nav.NavigateTo(url, forceLoad);
		}
		public static string GetUrl<T>(this NavigationManager Nav, Dictionary<string, string>? parameters = null) where T : IComponent
		{
			Type component = typeof(T);
			RouteAttribute routeAttribute = component.GetCustomAttribute<RouteAttribute>() ?? throw new TypeAccessException("The component has no routes");

			string url = routeAttribute.Template;

			if (parameters != null)
			{
				foreach (var parameter in parameters)
				{
					string placeholder = $"{{{parameter.Key}}}";
					if (url.Contains(placeholder))
					{
						url = url.Replace(placeholder, parameter.Value);
					}
				}
			}

			return url;
		}
	}
}
