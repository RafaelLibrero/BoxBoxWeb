using Microsoft.AspNetCore.Mvc;

namespace BoxBoxClient.Helpers
{
    public class HelperTools
    {
        public static RedirectToRouteResult GetRoute
            (string controller, string action)
        {
            RouteValueDictionary ruta =
                new RouteValueDictionary(
                    new { controller = controller, action = action });
            RedirectToRouteResult result =
                new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
