using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMBusiness.ModelValidation;
using FMBusiness.Managers;
using System.ComponentModel.DataAnnotations;

namespace FMBusiness.CustomAttributes.ModelValidationAttributes
{
    public class UserAttributes 
    {
        public class EmailAvailable : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return UserManager.IsEmailAddressAvailable((string)value);
            }
        }

        public class TermsAccepted : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return value is bool && (bool) value;
            }
        }
    }
}
