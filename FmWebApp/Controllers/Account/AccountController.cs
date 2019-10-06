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

    public class AccountController : BaseController
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [AllowAnonymous]
        [UserLoggedProtection]
        public ActionResult Login()
        {
            HostCommunication.Managers.DbManager.InitializeBackup();

            Session["USER_ID"] = null;
            LoginVModel UserLogin = new LoginVModel();

            if (Request.Cookies["XBVCBXVZVCVXBVCXBXC"] != null)
            {
                UserLogin.Email = Request.Cookies["XBVCBXVZVCVXBVCXBXC"].Values["werewqrwerqwerqqrwqr23"];
                UserLogin.Email = SecurityManager.Decrypt(UserLogin.Email);

                UserLogin.Password = Request.Cookies["XBVCBXVZVCVXBVCXBXC"].Values["SAafdasfregdRFfdsgasfas"];
                UserLogin.RememberMe = true;
            }

            HttpCookie cookie = Request.Cookies["_culture"];

            // pobierz język przeglądarki uzytkownika (pierwsze wywołanie funkcji, stąd jako argument przesłany null)
            var culture = CultureHelper.GetImplementedCulture(null);

            // sprawdz czy wczesniej zostało utworzone ciasteczko
            if (cookie == null)
            {
                cookie = new HttpCookie("_culture")
                {
                    Value = culture,
                    Expires = DateTime.Now.AddYears(1)
                };

                Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = culture;
            }

            return View(UserLogin);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVModel UserLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(UserLogin);
            }

            var userSigning = AccountManager.UserSignIn(UserLogin);
            if (userSigning == null) {

                ModelState.AddModelError("", Resources.Resources.MessageForInvalidLogin);
                return View(UserLogin);

            } else
            {
                FormsAuthentication.SetAuthCookie(userSigning.Email, userSigning.RememberMe);
                 
                HttpCookie cookie = new HttpCookie("XBVCBXVZVCVXBVCXBXC"); // random strings for safety purposes, login
                string mail = SecurityManager.Encrypt(userSigning.Email);
                cookie.Values.Add("werewqrwerqwerqqrwqr23", mail);
                cookie.Values.Add("SAafdasfregdRFfdsgasfas", userSigning.Password);

                cookie.Expires = DateTime.Now.AddDays(15);
                Response.Cookies.Add(cookie);

                HttpCookie cookieCulture = Request.Cookies["_culture"];
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookieCulture.Value);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                return RedirectToAction("Dashboard", "Dashboard");
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [UserLoggedProtection]
        public ActionResult Register()
        {
            Session["USER_ID"] = null;
            RegisterVModel UserRegister = new RegisterVModel();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVModel UserRegister)
        {
            if (!ModelState.IsValid) return View(UserRegister);

            if (!UserManager.InsertUser(UserRegister))
            {
                ModelState.AddModelError("", Resources.Resources.MessageForErrorRegister);
                return View(UserRegister);
            }

            return RedirectToAction("AccountActivation");
        }


        public ActionResult Logout()
        {
            UserManager.ResetUserOnLogout();
            return RedirectToAction("Login");
        }
    }
}