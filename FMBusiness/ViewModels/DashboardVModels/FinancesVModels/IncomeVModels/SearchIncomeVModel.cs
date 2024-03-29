﻿using FMBusiness.Managers;
using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.IncomeVModels;
using FMDataModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FMBusiness.ViewModels.IncomeVModels
{

    public partial class SearchIncomeVModel
    {
        public string Name { get; set; }

        public IncomeType IncomeType { get; set; }
       
        public decimal? Amount { get; set; }

        public string Currency { get; set; }

        public DateTime? InsertTime { get; set; }
    }
}

