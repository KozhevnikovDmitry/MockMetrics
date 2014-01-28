using System;
using Common.DA.DAException;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.DomainContext
{
    public abstract class TransactionWrapper
    {
        protected void Transaction(IDomainDbManager dbManager, Action<IDomainDbManager> action)
        {
            try
            {
                dbManager.BeginDomainTransaction();

                action(dbManager);

                dbManager.CommitDomainTransaction();
            }
            catch (BLLException)
            {
                throw;
            }
            catch (TransactionControlException ex)
            {
                throw new BLLException(ex);
            }
            catch (Exception ex)
            {
                dbManager.RollbackDomainTransaction();
                throw new BLLException(ex);
            }
        }
    }
}
