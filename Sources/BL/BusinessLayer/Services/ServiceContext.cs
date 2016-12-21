using System;
using System.Diagnostics.Contracts;

namespace BlueBit.HR.Docs.BL.BusinessLayer.Services
{
    public interface IServiceContext
    {
        IBusinessTransaction BusinessTransaction { get; }
    }

    public class ServiceContext :
        IServiceContext
    {
        private readonly IBusinessTransaction businessTransaction;
        public IBusinessTransaction BusinessTransaction { get { return businessTransaction; } }

        public ServiceContext(IBusinessTransaction businessTransaction)
        {
            Contract.Requires(businessTransaction != null);
            this.businessTransaction = businessTransaction;
        }
    }

}
