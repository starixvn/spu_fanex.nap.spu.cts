using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.CTS.Features.Accounts.Criteria;
using Fanex.NAP.SPU.CTS.Features.Accounts.Entities;
using Fanex.NAP.SPU.CTS.Shares;
using System.Collections.Generic;

namespace Fanex.NAP.SPU.CTS.Features.Accounts
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

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<AccountEntity>(criteria);
            }
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
