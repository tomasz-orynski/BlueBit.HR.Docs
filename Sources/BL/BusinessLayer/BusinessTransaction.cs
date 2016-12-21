using System;
using System.Diagnostics.Contracts;

namespace BlueBit.HR.Docs.BL.BusinessLayer
{
    public interface IBusinessTransaction
    {
        IBusinessContext BusinessCtx { get; }
        NHibernate.IStatelessSession DBSession { get; }
    }

    internal sealed class BusinessTransaction :
        IBusinessTransaction
    {
        private readonly IBusinessContext businessCtx;
        public IBusinessContext BusinessCtx { get { return businessCtx; } }

        private readonly NHibernate.IStatelessSession dbSession;
        public NHibernate.IStatelessSession DBSession { get { return dbSession; } }

        public BusinessTransaction(IBusinessContext businessCtx, NHibernate.IStatelessSession dbSession)
        {
            Contract.Requires(businessCtx != null);
            Contract.Requires(dbSession != null);
            this.businessCtx = businessCtx;
            this.dbSession = dbSession;
        }
    }
}
