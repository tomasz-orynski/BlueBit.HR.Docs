using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using BL = BlueBit.HR.Docs.BL;
using WWW = BlueBit.HR.Docs.WWW;

namespace BlueBit.HR.Docs.WWW.Models.Administration
{
    public class ChangePIN :
        IValidatableObject
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecny PIN:")]
        [MinLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [MaxLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX)]
        [HasOnlyDigits]
        public string ActualPIN { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nowy PIN:")]
        [MinLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [MaxLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX)]
        [HasOnlyDigits]
        public string NewPIN { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź PIN:")]
        [MinLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [MaxLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX)]
        [HasOnlyDigits]
        public string ConfirmPIN { get; set; }

        [Required]
        [Display(Name = "Czy wysyłać mail o zalogowaniu:")]
        public bool IsLogonMailSend { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ActualPIN != WWW.MvcApplication.GetAppInstance().BusinessContext.Session.Employee.PIN)
                yield return new ValidationResult("Obecny kod PIN niezgodny.", new string[] { "ActualPIN" });
            if (NewPIN != ConfirmPIN)
                yield return new ValidationResult("Niezgodność nowego kodu PIN z wartością potwierdzoną.", new string[] { "NewPIN", "ConfirmPIN" });
        }
    }
}