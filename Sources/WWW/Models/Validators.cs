using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace BlueBit.HR.Docs.WWW.Models
{
    public class HasOnlyDigitsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var s = value.ToString();
                if (s.Where(c => !char.IsDigit(c)).Any())
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}