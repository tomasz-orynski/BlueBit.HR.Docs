using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using BlueBit.HR.Docs.BL.BusinessLayer.Extensions;

namespace BlueBit.HR.Docs.WWW.Models
{
    public abstract class BusinessLogicBase
    {
        private readonly BL.BusinessLayer.IBusinessContext businessContext = WWW.MvcApplication.GetAppInstance().BusinessContext;
        protected BL.BusinessLayer.IBusinessContext BusinessContext { get { return businessContext; } }

        public void SaveEntityModel<TModel>(TModel entityModel)
            where TModel : EntityModelBase
        {
            SaveEntityModel(entityModel, (t, m) => true);
        }

        public bool SaveEntityModel<TModel>(TModel entityModel, Func<BL.BusinessLayer.IBusinessTransaction, TModel, bool> check)
            where TModel: EntityModelBase
        {
            Contract.Requires(check != null);
            return BusinessContext.DoInTransactionWithResult(transaction =>
            {
                if (check(transaction, entityModel))
                {
                    var session = transaction.DBSession;
                    var entity = entityModel.SourceEntityWithID;

                    if (entityModel.IsFromDB)
                        session.Update(entity);
                    else
                        session.Insert(entity);

                    return true;
                }
                return false;
            });
        }

        public T GetEntity<T>(Expression<Func<T, bool>> expression)
            where T : class, BL.DataLayer.Entities.Commons.IObjectInDBWithID, new()
        {
            return BusinessContext.DoInTransactionWithResult(transaction =>
            {
                var session = transaction.DBSession;
                return session.QueryOver<T>().Where(expression).SingleOrDefault();
            });
        }
    }
}