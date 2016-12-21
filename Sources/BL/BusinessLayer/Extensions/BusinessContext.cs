using System;
using System.Diagnostics.Contracts;

namespace BlueBit.HR.Docs.BL.BusinessLayer.Extensions
{
    public static class BusinessContextExtensions
    {
        public static void DoInTransaction<T>(this T businessContext, Action<IBusinessTransaction> action)
            where T: IBusinessContext
        {
            Contract.Requires(businessContext != null);
            Contract.Requires(action != null);

            using (var session = businessContext.OpenDBSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    action(new BusinessTransaction(businessContext, session));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static TResult DoInTransactionWithResult<T, TResult>(this T businessContext, Func<IBusinessTransaction, TResult> action)
            where T : IBusinessContext
        {
            var result = default(TResult);
            businessContext.DoInTransaction(transaction => result = action(transaction));
            return result;
        }
    }
}
