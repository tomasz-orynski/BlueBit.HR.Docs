using System.ComponentModel.DataAnnotations;

namespace BlueBit.HR.Docs.WWW.Models.Administration
{
    public class UserData :
        EntityModelBase<BL.DataLayer.Entities.Employee>
    {
        [Required]
        [Display(Name = "Identyfikator:")]
        [StringLength(BL.DataLayer.Cfg.Defines.FLD_LEN_IDENTIFIER, MinimumLength = 1)]
        public string Identifier { get { return SourceEnity.Identifier; } set { SourceEnity.Identifier = value; } }

        //[Required]
        [Display(Name = "Nazwisko, imię:")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "PESEL:")]
        [StringLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PESEL, MinimumLength = BL.DataLayer.Cfg.Defines.FLD_LEN_PESEL)]
        [HasOnlyDigits]
        public string PESEL { get { return SourceEnity.PESEL; } set { SourceEnity.PESEL = value; } }

        [Required]
        [Display(Name = "Kod PIN:")]
        [StringLength(BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MAX, MinimumLength = BL.DataLayer.Cfg.Defines.FLD_LEN_PIN_MIN)]
        [HasOnlyDigits]
        public string PIN { get { return SourceEnity.PIN; } set { SourceEnity.PIN = value; } }

        [Required]
        [Display(Name = "Admin:")]
        public bool IsAdministrator { get { return SourceEnity.IsAdministrator; } set { SourceEnity.IsAdministrator = value; } }

        public UserData() { }
        public UserData(BL.DataLayer.Entities.Employee employee) : base(employee) { }
        public UserData(BL.BusinessLayer.Services.Enviroment.IUserInfo userInfo)
        {
            Identifier = userInfo.Identifier;
            FullName = userInfo.FullName;
        }
        public UserData(BL.BusinessLayer.Services.Enviroment.IUserInfo userInfo, BL.DataLayer.Entities.Employee employee)
            : base(employee) 
        {
            FullName = userInfo.FullName;
        }

        public override System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}