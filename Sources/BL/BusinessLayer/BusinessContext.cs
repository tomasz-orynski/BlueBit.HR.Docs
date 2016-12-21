using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace BlueBit.HR.Docs.BL.BusinessLayer
{
    public interface IBusinessContext :
        IDisposable
    {
        NHibernate.IStatelessSession OpenDBSession();
        IEnumerable<System.DirectoryServices.DirectorySearcher> GetADSearchers();
        DataLayer.Entities.Session Session { get; }
    }

    public sealed class BusinessContext :
        IBusinessContext
    {
        private readonly NHibernate.ISessionFactory _dbSessionFactory = BL.DataLayer.Cfg.Defines.CreateSessionFactory();
        private readonly Func<IEnumerable<System.DirectoryServices.DirectorySearcher>> _adSerchersFactory = BL.DataLayer.Cfg.Defines.CreateDirectorySearchers;

        private readonly DataLayer.Entities.Session _session;
        public DataLayer.Entities.Session Session { get { return _session; } }

        public BusinessContext(Expression<Func<DataLayer.Entities.Employee, bool>> chooseEmployee)
        {
            using (var dbSession = OpenDBSession())
            using (var tran = dbSession.BeginTransaction())
                try
                {
                    var employee = dbSession.QueryOver<DataLayer.Entities.Employee>().Where(chooseEmployee).SingleOrDefault();
                    if (employee == null) LogonBusinessException.Throw();

                    _session = new DataLayer.Entities.Session() { Employee = employee, DateStart = DateTime.Now };
                    dbSession.Insert(_session);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }

        }

        public void Dispose()
        {
            using (var dbSession = OpenDBSession())
            using (var tran = dbSession.BeginTransaction())
                try
                {
                    _session.DateStop = DateTime.Now;
                    dbSession.Update(_session);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            _dbSessionFactory.Dispose();
        }

        public NHibernate.IStatelessSession OpenDBSession() { return _dbSessionFactory.OpenStatelessSession(); }
        public IEnumerable<System.DirectoryServices.DirectorySearcher> GetADSearchers() { return _adSerchersFactory(); }
    }
}
