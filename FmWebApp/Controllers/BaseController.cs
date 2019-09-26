using FMBusiness.Classes;
using FMBusiness.Managers;
using FMBusiness.ViewModels.AccountVModels;
using log4net;
using System;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static FMBusiness.CustomAttributes.ViewAttributes.CustomActionFilterAttributes;

namespace FmWebApp.Controllers
{
    // TODO: Translate all static texts in the app

    public class BaseController : Controller
    {
        public string lang
        {
            get
            {
                string language;
                if (Session["APP_LANGUAGE"] != null)
                {
                    language = Session["APP_LANGUAGE"].ToString();
                    return language;
                }
                language = "PL";
                return language;
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = HttpContext.Request.UserLanguages != null && HttpContext.Request.UserLanguages.Length > 0 ?
                        HttpContext.Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages 
                        null;
            }
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        public ActionResult SetCulture(string culture, string returnUrl = "Login")
        {
            // pobierz język
            culture = CultureHelper.GetImplementedCulture(culture);

            // dodaj/zaktualizuj cookiesa
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
            {
                Response.Cookies.Remove("_culture");

                cookie = new HttpCookie("_culture")
                {
                    Value = culture,
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Add(cookie);
            }
            else
            {
                cookie = new HttpCookie("_culture")
                {
                    Value = culture,
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Add(cookie);
            }
            HttpContext.Session["APP_LANGUAGE"] = culture;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return View(returnUrl);
        }

    }
}