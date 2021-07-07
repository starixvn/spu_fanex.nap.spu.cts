using Fanex.Data.Repository;
using Fanex.NAP.SPU.CTS.Features.Transform.Accounts.Criteria;
using Fanex.NAP.SPU.CTS.Features.Transform.Accounts.Entities;
using Fanex.NAP.SPU.CTS.Shares;
using System.Collections.Generic;

namespace Fanex.NAP.SPU.CTS.Features.Transform.Accounts
{
    public interface IAccountDataAccess
    {
        DataResult TransformDCSAccount(string accountJson);

        IEnumerable<AccountEntity> GetRawAccount(int numOfAccount, int numOfBatch);
    }

    public class AccountDataAccess : IAccountDataAccess
    {
        readonly IDynamicRepository repository;

        public AccountDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<AccountEntity> GetRawAccount(int numOfAccount, int numOfBatch)
        {
            var criteria = new TransformToCTSAccountGetPackageCriteria
            {
                NumberOfAccount = numOfAccount,
                NumberOfBatch = numOfBatch
            };

            return repository.Fetch<AccountEntity>(criteria);
        }

        public DataResult TransformDCSAccount(string accountJson)
        {
            var criteria = new TransformDCSAccountToCTSCustomerCriteria
            {
                AccountJson = accountJson
            };

           var errorCode =  repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errorCode
            };
        }
    }
}
