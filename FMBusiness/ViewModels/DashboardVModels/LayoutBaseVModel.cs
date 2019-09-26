using FMBusiness.Managers;
using FMBusiness.StorageModels;
using FMBusiness.ViewModels.DashboardVModels.UserVModels;
using FMDataModel.Enums;
using FMBusiness.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FMBusiness.ViewModels.DashboardVModels
{
    public partial class LayoutBaseVModel
    {
        public CurrencyType CurrencyType { get; set; }

        public LanguageType LanguageType { get; set; }

        public LayoutBaseVModel(CurrencyType CurrencyType)
        {
            this.CurrencyType = CurrencyType;
        }
    }
}
