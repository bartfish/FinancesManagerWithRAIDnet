namespace FMDataModel.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class fm_Users
    {
        [Key]
        public long Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(80)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string ForgottenPasswordToken { get; set; }

        public DateTime? LastSuccessfullLogin { get; set; }

        public DateTime? LastFailedLogin { get; set; }

        public DateTime? InsertTime { get; set; }

        public int AccountStatus { get; set; }

        public bool? AccountSleeps { get; set; }

        public int? DefaultCurrency { get; set; }

        [Required]
        [MaxLength(50)]
        public byte[] Salt { get; set; }

        public int IsOnline { get; set; }

        public int AppThemeId { get; set; }
    }
}
