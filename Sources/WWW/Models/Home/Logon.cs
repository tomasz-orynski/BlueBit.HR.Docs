using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BL = BlueBit.HR.Docs.BL;
using WWW = BlueBit.HR.Docs.WWW;

namespace BlueBit.HR.Docs.WWW.Models.Home
{
    public class LogOn :
        IValidatableObject
    {
        [Required]
        [Display(Name = "Kod PIN:")]
        [MinLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [MaxLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX)]
        [HasOnlyDigits]
        public string PIN { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}