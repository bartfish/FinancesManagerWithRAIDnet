namespace FMDataModel.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class fm_Incomes
    {
        [Key]
        public long Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public double? Amount { get; set; }

        public int Type { get; set; }

        public DateTime? InsertTime { get; set; }

        public long UserId { get; set; }
    }
}
