using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDataModel.Enums
{
    public enum CurrencyType
    {
        [Display(Name = "zł")]
        PLN = 1,

        [Display(Name = "$")]
        DOLLAR = 2,

        [Display(Name = "€")]
        EURO = 3
    }
}
