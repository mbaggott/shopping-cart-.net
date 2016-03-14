using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShoppingCart.Attributes
{
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Authorisation check
            return httpContext.Session["Username"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirect to login controller
            filterContext.Result = new RedirectToRouteResult(
                                        new RouteValueDictionary(
                                            new { controller = "Home", action = "Login" })); 
        }
    }
}