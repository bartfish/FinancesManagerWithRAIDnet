using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMBusiness.ViewModels
{
    public partial class FGeneralVModel
    {
        [StringLength(255)]
        [Required]
        [Display(Name = "LabelForComeName", ResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }

        [Column(TypeName = "numeric")]
        [Required]
        [Display(Name = "LabelForAmount", ResourceType = typeof(Resources.Resources))]
        public double? Amount { get; set; }

        [Display(Name="Currency")]
        public string Currency { get; set; }
        
        public DateTime? InsertTime { get; set; }
    }
}

