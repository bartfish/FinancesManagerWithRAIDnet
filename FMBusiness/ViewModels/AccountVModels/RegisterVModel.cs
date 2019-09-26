using System.ComponentModel.DataAnnotations;
using static FMBusiness.CustomAttributes.ModelValidationAttributes.UserAttributes;

namespace FMBusiness.ViewModels.AccountVModels
{

    public partial class RegisterVModel
    {
        [StringLength(150)]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MessageForErrorRequiredName")]
        public string Name { get; set; }

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

        [StringLength(255)]
        [DataType(DataType.Password)]
        [Display(Name = "LabelForRepeatPassword", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MessageForErrorMatchingPasswords")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MessageForErrorMatchingPasswords")]
        public string PasswordRepeated { get; set; }
        
        [Display(Name = "LabelForTermsAndConditions", ResourceType = typeof(Resources.Resources))]
        [TermsAccepted(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MessageForErrorRequiredTerms")]
        public bool Terms { get; set; }
    }
}

