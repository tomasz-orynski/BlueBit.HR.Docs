using System;
using System.Collections.Generic;
using System.Linq;
using BL = BlueBit.HR.Docs.BL;
using BlueBit.HR.Docs.BL.BusinessLayer.Extensions;

namespace BlueBit.HR.Docs.WWW.Models.Administration
{
    public class Logic : 
        BusinessLogicBase
    {
        public void ChangePIN(string newPIN, bool isLogonMailSend)
        {
            BusinessContext.DoInTransaction(transaction =>
            {
                var employee = BusinessContext.Session.Employee;
                employee.PIN = newPIN;
                employee.IsLogonMailSend = isLogonMailSend;
                transaction.DBSession.Update(employee);
                employee.IsLogonMailSend = isLogonMailSend;
            });
        }

        public IList<UserData> GetUsers()
        {
            return BusinessContext.DoInTransactionWithResult(transaction =>
            {
                using (var service =
                    new BL.BusinessLayer.Services.Enviroment.GetUsersService<BL.BusinessLayer.Services.Enviroment.GetUsersServiceContext>(
                        new BL.BusinessLayer.Services.Enviroment.GetUsersServiceContext(transaction)
                    ))
                {
                    var dbUsers = transaction.DBSession.QueryOver<BL.DataLayer.Entities.Employee>().List().ToDictionary(u=>u.Identifier);
                    var adUsers = service.Execute();

                    return adUsers
                        .Select(u => dbUsers.ContainsKey(u.Identifier) ? new UserData(u, dbUsers[u.Identifier]) : new UserData(u))
                        .ToList();
                }
            });
        }

        public IEnumerable<Tuple<string, string>> CheckUser(BL.BusinessLayer.IBusinessTransaction transaction, UserData user)
        {
            var dbSession = transaction.DBSession;
            var dbUserByIdentifier = dbSession.QueryOver<BL.DataLayer.Entities.Employee>().Where(u => u.ID != user.ID && u.Identifier == user.Identifier).SingleOrDefault();
            var dbUserByPESEL = dbSession.QueryOver<BL.DataLayer.Entities.Employee>().Where(u => u.ID != user.ID && u.PESEL == user.PESEL).SingleOrDefault();

            if (dbUserByIdentifier != null)
                yield return Tuple.Create("Identyfikator", "Jest już użytkownik o takim identyfikatorze.");

            if (dbUserByPESEL != null)
                yield return Tuple.Create("PESEL", "Jest już użytkownik o takim PESEL.");
        }


        public void LoadData(System.IO.Stream stream)
        {
            BusinessContext.DoInTransaction(transaction =>
            {
                using (var service =
                    new BL.BusinessLayer.Services.DataLoad.LoadZipService<BL.BusinessLayer.Services.DataLoad.LoadZipServiceContext>(
                        new BL.BusinessLayer.Services.DataLoad.LoadZipServiceContext(transaction, stream)
                    ))
                {
                    service.Execute();
                }
            });
        }
    }
}