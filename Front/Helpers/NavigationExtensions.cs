using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Front.Helpers
{
    public static class NavigationExtensions
    {
        public static void NavigateTo<T>(this NavigationManager Nav, bool forceLoad = false) where T : IComponent 
        {
            Type component = typeof(T);
            RouteAttribute routeAttribute = component.GetCustomAttribute<RouteAttribute>() ?? throw new TypeAccessException("The component has no routes");

            Nav.NavigateTo(routeAttribute.Template, forceLoad);
        }
    }
}
