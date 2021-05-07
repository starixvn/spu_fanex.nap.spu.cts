using Fanex.NAP.SPU.CTS.Features.Accounts.Entities;
using Fanex.NAP.SPU.CTS.Shares;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.SPU.CTS.Features.Accounts
{
    public interface IAccountServices
    {
        DataResult TransformAccount(IEnumerable<AccountEntity> accountList);

        IEnumerable<AccountEntity> GetRawAccount(int numOfAccount, int numOfBatch);
    }

    public class AccountServices : IAccountServices
    {
        private readonly IAccountDataAccess accountDataAccess;

        public AccountServices(IAccountDataAccess accountDataAccess)
        {
            this.accountDataAccess = accountDataAccess;
        }

        public IEnumerable<AccountEntity> GetRawAccount(int numOfAccount, int numOfBatch)
        {
            return accountDataAccess.GetRawAccount(numOfAccount, numOfBatch);
        }

        public DataResult TransformAccount(IEnumerable<AccountEntity> accountList)
        {
            if (accountList != null && accountList.Any())
            {
                var accountJson = JsonConvert.SerializeObject(accountList);

                return accountDataAccess.TransformDCSAccount(accountJson);
            }

            return new DataResult
            {
                ErrorCode = -1,
                ErrorMessage = string.Empty
            };
        }
    }
}
