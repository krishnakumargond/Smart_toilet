using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Smart_toilet.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionPersiter.oUserInfo == null && SkipAuthorization(filterContext) == false)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Account", Action = "Login" }));
            }
            //else
            //{
            //    if (!this.AuthorizeCore(filterContext.HttpContext))
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "AccessDenied", Action = "Index", Areas = "" }));
            //    }
            //}

        }

        private static bool SkipAuthorization(AuthorizationContext filterContext)
        {
            Contract.Assert(filterContext != null);

            return filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                   || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool flg = true;
            var controller = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
            //string UserRole = SessionPersiter.oUserInfo.Roles;

            //if (UserRole == "Admin")
            //    flg = true;
            return flg;
        }
    }
}