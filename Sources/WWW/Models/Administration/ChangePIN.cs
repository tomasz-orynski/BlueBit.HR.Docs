using System.ComponentModel.DataAnnotations;

namespace BlueBit.HR.Docs.WWW.Models.Administration
{
    public class ChangePIN :
        IValidatableObject
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecny PIN:")]
        [StringLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX, MinimumLength = BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [HasOnlyDigits]
        public string ActualPIN { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nowy PIN:")]
        [StringLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX, MinimumLength = BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [HasOnlyDigits]
        public string NewPIN { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź PIN:")]
        [StringLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX, MinimumLength = BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
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