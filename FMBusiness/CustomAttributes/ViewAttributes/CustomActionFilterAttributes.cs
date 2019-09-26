using System.Web.Mvc;

namespace FMBusiness.CustomAttributes.ViewAttributes
{
    public class CustomActionFilterAttributes
    {
        /* CONTROLLER PROTECTION ATTRIBUTES */
        public class UserNotLoggedProtection : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var user = filterContext.HttpContext.Session["USER_ID"];
                if (user == null)
                {
                    filterContext.Result = new RedirectResult("~/Accounts/Login");
                } 
            }
        }

        public class UserLoggedProtection : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var user = filterContext.HttpContext.Session["USER_ID"];
                if (user != null)
                {
                    filterContext.Result = new RedirectResult("~/Dashboard/Dashboard");
                }
            }
        }

    }
}
