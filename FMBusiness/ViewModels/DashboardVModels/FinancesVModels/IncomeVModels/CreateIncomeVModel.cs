using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.IncomeVModels;
using FMDataModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FMBusiness.ViewModels.IncomeVModels
{

    public partial class CreateIncomeVModel : Income
    {

        [Display(Name = "LabelForIncomeType", ResourceType = typeof(Resources.Resources))]
        public IncomeType IncomeType { get; set; }

    }
}

