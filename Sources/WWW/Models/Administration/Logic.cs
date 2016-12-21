using System;
using System.Collections.Generic;
using System.Linq;
using BlueBit.HR.Docs.BL.BusinessLayer.Extensions;
using BlueBit.HR.Docs.BL.Extensions;

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
                    var app = MvcApplication.GetAppInstance();
                    var adUsers = service.Execute().ToDictionary(_ => _.Identifier);
                    var dbUsers = transaction.DBSession.QueryOver<BL.DataLayer.Entities.Employee>().List().ToDictionary(_ => _.Identifier);
                    var identifiers = adUsers.Keys.Union(dbUsers.Keys).Distinct().ToList();
                    var dict = new Dictionary<Tuple<bool, bool>, Func<Tuple<BL.BusinessLayer.Services.Enviroment.IUserInfo, BL.DataLayer.Entities.Employee>, UserData>>()
                    {
                        [Tuple.Create(true, true)] = _ => new UserData(_.Item1, _.Item2),
                        [Tuple.Create(false, true)] = _ => new UserData(_.Item2),
                        [Tuple.Create(true, false)] = _ => new UserData(_.Item1),
                    };
                    return identifiers
                        .Select(_ => Tuple.Create(adUsers.GetOptValue(_), dbUsers.GetOptValue(_)))
                        .Select(_ => dict[Tuple.Create(_.Item1!=null, _.Item2!=null)](_))
                        .Select(_ =>
                        {
                            _.CanDelete = !app.IsUserCurrent(_.ID);
                            return _;
                        })
                        .ToList();
                }
            });
        }

        public bool DeleteUser(long id)
        {
            return BusinessContext.DoInTransactionWithResult(transaction =>
            {
                var session = transaction.DBSession;
                var entity = session.Get<BL.DataLayer.Entities.Employee>(id);
                session.QueryOver<BL.DataLayer.Entities.Session>()
                    .Where(_ => _.Employee.ID == entity.ID)
                    .List()
                    .ForAll(s =>
                    {
                        session.QueryOver<BL.DataLayer.Entities.SessionDocumentGet>()
                            .Where(_ => _.Session.ID == s.ID)
                            .List()
                            .ForAll(session.Delete);
                        session.Delete(s);
                    });
                session.QueryOver<BL.DataLayer.Entities.DocumentWithData>()
                    .Where(_ => _.PESEL == entity.PESEL)
                    .List()
                    .ForAll(session.Delete);

                session.Delete(entity);
                return true;
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