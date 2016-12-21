using System;
using System.Diagnostics.Contracts;

namespace BlueBit.HR.Docs.BL.BusinessLayer.Services
{
    public interface IService<out TResult> :
        IDisposable
    {
        TResult Execute();
    }

    public abstract class ServiceBase<TServiceContext, TResult> :
        IService<TResult>
        where TServiceContext : class, IServiceContext
    {
        protected readonly TServiceContext ServiceCtx;

        protected ServiceBase(TServiceContext serviceCtx)
        {
            Contract.Requires(serviceCtx != null);
            ServiceCtx = serviceCtx;
        }

        public TResult Execute()
        {
            return OnExecute();
        }
        public void Dispose()
        {
            OnDispose();
        }

        protected abstract TResult OnExecute();
        protected virtual void OnDispose() { }
    }
}
