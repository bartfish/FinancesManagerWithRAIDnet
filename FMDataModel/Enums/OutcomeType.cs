using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDataModel.Enums
{
    public enum OutcomeType
    {
        [Display(Name = "Tax")]
        Tax = 1,

        [Display(Name = "Fine")]
        Fine = 2,

        [Display(Name = "Fun")]
        Fun = 3,

        [Display(Name = "Food")]
        Food = 4,

        [Display(Name = "Communication")]
        Communication = 5,

        [Display(Name = "Saving")]
        School = 6,

        [Display(Name = "Repairments")]
        Repairments = 7,

        [Display(Name = "Home")]
        Home = 8,

        [Display(Name = "Utilities")]
        Utilities = 9
    }
}
