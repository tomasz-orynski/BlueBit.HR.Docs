using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;

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

        public bool CanDelete { get; set; }

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

    public static class UserDataExt
    {
        public static IEnumerable<UserData> Sort<T>(
            this IEnumerable<UserData> @this,
            string sortOrder, string currentSortOrder,
            Func<UserData, T> selector)
            => (sortOrder == currentSortOrder)
                ? @this.OrderByDescending(selector)
                : @this.OrderBy(selector);

        public static IEnumerable<UserData> SortAndFilter(
            this IEnumerable<UserData> @this,
            string nameFilter, string sortOrder, string currentSortOrder
            )
        {
            Contract.Assert(@this != null);

            if (!string.IsNullOrWhiteSpace(nameFilter))
                @this = @this.Where(_ => _.Identifier?.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0 || _.FullName?.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0);

            switch (sortOrder)
            {
                default:
                case nameof(UserData.Identifier):
                    return @this.Sort(sortOrder, currentSortOrder, _ => _.Identifier);

                case nameof(UserData.FullName):
                    return @this.Sort(sortOrder, currentSortOrder, _ => _.FullName);

                case nameof(UserData.PESEL):
                    return @this.Sort(sortOrder, currentSortOrder, _ => _.PESEL);

                case nameof(UserData.IsAdministrator):
                    return @this.Sort(sortOrder, currentSortOrder, _ => _.IsAdministrator);
            }
        }
    }
}
