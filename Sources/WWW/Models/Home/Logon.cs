using System.ComponentModel.DataAnnotations;

namespace BlueBit.HR.Docs.WWW.Models.Home
{
    public class LogOn :
        IValidatableObject
    {
        [Required]
        [Display(Name = "Kod PIN:")]
        [StringLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX, MinimumLength = BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [HasOnlyDigits]
        public string PIN { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}