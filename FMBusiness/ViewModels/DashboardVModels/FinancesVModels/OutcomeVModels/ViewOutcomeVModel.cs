using FMBusiness.Managers;
using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.OutcomeVModels;
using FMBusiness.ViewModels.IncomeVModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FMBusiness.ViewModels.OutcomeVModels
{

    public partial class ViewOutcomeVModel : Outcome
    {
        private long Id { get; set; }

        private long UserId { get; set; }

        public string UserName { get; set; }

        public IPagedList<Outcome> OutcomesList { get; set; }

        public SearchOutcomeVModel SearchOutcomes { get; set; }

        public int? Page { get; set; }
        public int PageSize { get; set; }
        public string SOrderByColumn { get; set; }

    }
}

