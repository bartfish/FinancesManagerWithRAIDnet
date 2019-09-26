using FMBusiness.Classes;
using FMBusiness.Managers;
using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.IncomeVModels;
using FMBusiness.ViewModels.GraphVModels;
using FMBusiness.ViewModels.IncomeVModels;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FmWebApp.Controllers.Incomes
{
    public class IncomesController : BaseController
    {
        private ViewIncomeVModel _incomesData { get; set; }

        private List<Income> _incomes { get; set; }
        private List<Income> _incomesExported { get; set; }

        [HttpGet]
        public ActionResult _CreateIncome()
        {
            return PartialView("_CreateIncome");
        }

        [HttpPost]
        public ActionResult _CreateIncome(CreateIncomeVModel Income)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateIncome", Income);
            }

            var success = IncomesManager.SaveIncome(Income);

            if (success)
                return JavaScript("ContainerManager.ShowActionInfo('" + Resources.Resources.MessageForSuccessIncomeSaving + "');");

            return JavaScript("ContainerManager.ShowActionInfo('" + Resources.Resources.MessageForErrorIncomeSaving + "');");

        }

        [HttpGet]
        public ActionResult _GetIncomes(ViewIncomeVModel data)
        {
            _incomesData = data;
            if (_incomesData == null)
            {
                _incomesData = new ViewIncomeVModel();
            }
            _incomesData.Page = _incomesData.Page ?? 1;
            int pageIndex = _incomesData.Page.HasValue ? Convert.ToInt32(_incomesData.Page) : 1;

            _incomes = IncomesManager.GetIncomes(_incomesData.SOrderByColumn, data.SearchIncomes);
            if (_incomes == null ) return PartialView(_incomesData);
            
            _incomesData.IncomesList = _incomes.ToPagedList(pageIndex, _incomesData.PageSize);

            if (!string.IsNullOrEmpty(_incomesData.SOrderByColumn))
                if (_incomesData.SOrderByColumn.Length >= 4)
                    if (_incomesData.SOrderByColumn.Substring(_incomesData.SOrderByColumn.Length - 4) != "Desc")
                        _incomesData.SOrderByColumn = _incomesData.SOrderByColumn + "Desc";
                    else
                        _incomesData.SOrderByColumn = _incomesData.SOrderByColumn.Remove(_incomesData.SOrderByColumn.Length - 4);

            ViewBag.sOrderByColumn = _incomesData.SOrderByColumn;

            ViewBag.pageIndex = _incomesData.Page;
            ViewBag.pageSize = _incomesData.PageSize;

            return PartialView(_incomesData);
        }

        public void ExportToPdf(string sOrderByColumn)
        {
            _incomesExported = IncomesManager.GetIncomes(sOrderByColumn, _incomesData.SearchIncomes);
            PdfToDownload file = FileManager.GeneratePdf(_incomesExported);
        }

        public ActionResult ExportToCsv(string sOrderByColumn)
        {
            _incomesExported = IncomesManager.GetIncomes(sOrderByColumn, _incomesData.SearchIncomes);
            FileManager.GenerateCsv(_incomesExported).Download();
            return new EmptyResult();
        }

        public ActionResult _IncomesGraph()
        {
            if (_incomes == null)
            {
                _incomes = IncomesManager.GetIncomes("", null);
            }
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach(var income in _incomes)
            {
                int datetimeInMs = Convert.ToInt32(income.InsertTime.Value
                    .ToUniversalTime()
                    .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                    .TotalMilliseconds);

                double amount = income.Amount ?? 0;
                dataPoints.Add(new DataPoint(datetimeInMs, amount));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return PartialView();
        }

    }
}