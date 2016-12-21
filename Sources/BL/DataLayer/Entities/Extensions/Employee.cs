using System;
using System.Diagnostics.Contracts;

namespace BlueBit.HR.Docs.BL.DataLayer.Entities.Extensions
{
    public static class EmployeeExtensions
    {
        public static string GetFirstName<T>(this T employee)
            where T : class, IEmployee
        {
            Contract.Requires(employee != null);
            return string.Format("TODO-FirstName-{0}", employee.Identifier);
        }

        public static string GetLastName<T>(this T employee)
            where T : class, IEmployee
        {
            Contract.Requires(employee != null);
            return string.Format("TODO-LastName-{0}", employee.Identifier);
        }
    }
}
