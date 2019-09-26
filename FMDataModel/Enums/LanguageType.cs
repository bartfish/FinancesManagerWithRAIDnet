using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDataModel.Enums
{
    public enum LanguageType
    {
        [Display(Name = "PL")]
        PL = 1,

        [Display(Name = "EN")]
        EN = 2
    }
}
