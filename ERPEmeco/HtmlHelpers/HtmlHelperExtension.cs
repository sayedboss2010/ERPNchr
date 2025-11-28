using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ERPNchr.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper html, string action = null, string controller = null, string area = null, bool exact = false)
        {
            var routeData = html.ViewContext.RouteData;

            string routeAction = routeData.Values["action"]?.ToString();
            string routeController = routeData.Values["controller"]?.ToString();
            string routeArea = routeData.Values.ContainsKey("area") ? routeData.Values["area"]?.ToString() : "";

            bool active = true;

            if (!string.IsNullOrEmpty(area))
                active &= area.Equals(routeArea, StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(controller))
                active &= controller.Equals(routeController, StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(action))
                active &= action.Equals(routeAction, StringComparison.OrdinalIgnoreCase);

            return active ? "active open" : "";
        }
    }
}
