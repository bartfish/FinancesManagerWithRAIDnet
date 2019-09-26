using FMBusiness.Managers;
using FMBusiness.ViewModels.DashboardVModels;
using FMDataModel.Enums;
using System.Web.Mvc;
using static FMBusiness.CustomAttributes.ViewAttributes.CustomActionFilterAttributes;

namespace FmWebApp.Controllers
{
    public class DashboardController : BaseController
    {
        private DashboardVModel _dashboardModel;
            
        [UserNotLoggedProtection]
        public ActionResult Dashboard(DashboardVModel dashboardModel)
        {
            _dashboardModel = dashboardModel ?? new DashboardVModel();
            LayoutBaseVModel layoutBaseVModel = new LayoutBaseVModel(_dashboardModel.User.DefaultCurrency);

            return View(layoutBaseVModel);
        }
        
        [UserNotLoggedProtection] // for ajax reloading of the main view
        public ActionResult DashboardRefresh()
        {
            return RedirectToAction("Dashboard", _dashboardModel);
        }

        [UserNotLoggedProtection]
        public ActionResult _DashboardContent(DashboardVModel dashboardModel)
        {
            _dashboardModel = dashboardModel ?? new DashboardVModel();
            return PartialView(_dashboardModel);
        }

        [HttpGet]
        public ActionResult _Adder()
        {
            return PartialView();
        }
        
        [HttpGet]
        public ActionResult _Settings()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult SwitchCurrencyForDashboard(CurrencyType currency)
        {
            UserManager.SwitchCurrency(currency);
            return View("Dashboard", _dashboardModel);
        }

        [HttpGet]
        public ActionResult DashboardTranslate(string culture)
        { 
            return SetCulture(culture, "~/Views/Dashboard/Dashboard.cshtml");
        }
    }
}