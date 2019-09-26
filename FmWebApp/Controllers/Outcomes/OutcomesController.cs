using FMBusiness.Classes;
using FMBusiness.Managers;
using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.OutcomeVModels;
using FMBusiness.ViewModels.OutcomeVModels;
using PagedList;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FmWebApp.Controllers.Outcomes
{
    public class OutcomesController : BaseController
    {
        private ViewOutcomeVModel _outcomesData { get; set; }

        private List<Outcome> _outcomes { get; set; }
        private List<Outcome> _outcomesExported { get; set; }

        [HttpGet]
        public ActionResult _CreateOutcome()
        {
            return PartialView("_CreateOutcome");
        }

        [HttpPost]
        public ActionResult _CreateOutcome(CreateOutcomeVModel newOutcome)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateOutcome", newOutcome);
            }

            var success = OutcomesManager.SaveOutcome(newOutcome);

            if (success)
            {
                return JavaScript("ContainerManager.ShowActionInfo('" + Resources.Resources.MessageForSuccessOutcomeSaving + "');");
            }
            return JavaScript("ContainerManager.ShowActionInfo('" + Resources.Resources.MessageForErrorOutcomeSaving + "');");

        }

        [HttpGet]
        public ActionResult _GetOutcomes(ViewOutcomeVModel data)
        {
            _outcomesData = data;
            if (_outcomesData == null)
            {
                _outcomesData = new ViewOutcomeVModel();
            }
            _outcomesData.Page = _outcomesData.Page ?? 1;
            int pageIndex = _outcomesData.Page.HasValue ? Convert.ToInt32(_outcomesData.Page) : 1;

            _outcomes = OutcomesManager.GetOutcomes(_outcomesData.SOrderByColumn, data.SearchOutcomes);
            _outcomesData.OutcomesList = _outcomes.ToPagedList(pageIndex, _outcomesData.PageSize);

            if (!string.IsNullOrEmpty(_outcomesData.SOrderByColumn))
                if (_outcomesData.SOrderByColumn.Length >= 4)
                    if (_outcomesData.SOrderByColumn.Substring(_outcomesData.SOrderByColumn.Length - 4) != "Desc")
                        _outcomesData.SOrderByColumn = _outcomesData.SOrderByColumn + "Desc";
                    else
                        _outcomesData.SOrderByColumn = _outcomesData.SOrderByColumn.Remove(_outcomesData.SOrderByColumn.Length - 4);


            ViewBag.sOrderByColumn = _outcomesData.SOrderByColumn;

            ViewBag.pageIndex = _outcomesData.Page;
            ViewBag.pageSize = _outcomesData.PageSize;

            return PartialView(_outcomesData);
        }

        public void ExportToPdf(string sOrderByColumn)
        {
            _outcomesExported = OutcomesManager.GetOutcomes(sOrderByColumn, _outcomesData.SearchOutcomes);
            PdfToDownload file = FileManager.GeneratePdf(_outcomesExported);
        }

        public ActionResult ExportToCsv(string sOrderByColumn)
        {
            _outcomesExported = OutcomesManager.GetOutcomes(sOrderByColumn, _outcomesData.SearchOutcomes);
            FileManager.GenerateCsv(_outcomesExported).Download();
            return new EmptyResult();
        }

    }
}