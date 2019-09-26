
using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.OutcomeVModels;
using FMBusiness.ViewModels.IncomeVModels;
using FMDataModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FMBusiness.ViewModels.OutcomeVModels
{

    public partial class CreateOutcomeVModel : Outcome
    {
        
        [Display(Name = "LabelForIncomeType", ResourceType = typeof(Resources.Resources))]
        public OutcomeType OutcomeType { get; set; }

    }
}

