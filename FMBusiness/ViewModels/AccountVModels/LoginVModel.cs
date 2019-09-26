using FMBusiness.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace FMBusiness.ViewModels.AccountVModels
{
    public partial class LoginVModel
    {
        [StringLength(80)]
        [Display(Name = "LabelForEmail", ResourceType = typeof(Resources.Resources))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MessageForErrorIncorrectEmail")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MessageForErrorRequiredEmail")]
        public string Email { get; set; }

        [StringLength(255)]
        [DataType(DataType.Password)]
        [Display(Name = "LabelForPassword", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MessageForErrorRequiredPassword")]
        public string Password { get; set; }

        [Display(Name = "LabelForRememberMe", ResourceType = typeof(Resources.Resources))]
        public bool RememberMe { get; set; }

        public DateTime? LastSuccessfullLogin { get; set; }

        public DateTime? LastFailedLogin { get; set; }
    }
}

