using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDataModel.Enums
{
    public enum IncomeType
    {
        [Display(Name = "LabelForSalaryType", ResourceType = typeof(Resources.Resources))]
        Salary = 1,

        [Display(Name = "LabelForGiftType", ResourceType = typeof(Resources.Resources))]
        Gift = 2,

        [Display(Name = "LabelForSavingType", ResourceType = typeof(Resources.Resources))]
        Saving = 3
    }
}
