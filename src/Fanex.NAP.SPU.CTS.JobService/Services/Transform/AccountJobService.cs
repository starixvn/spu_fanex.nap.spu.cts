using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.CTS.Features.Transform.Accounts;
using Fanex.NAP.SPU.CTS.Shares;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    public interface IAccountJobService
    {
        Task<bool> Execute(int size, int numberOfBatch);

    }
    public class AccountJobService : IAccountJobService
    {
        private readonly IAccountServices accountServices;

        public AccountJobService(IAccountServices accountServices)
        {
            this.accountServices = accountServices;
        }

        public Task<bool> Execute(int size, int numberOfBatch)
        {
            try
            {
                if(size > 0 && numberOfBatch > 0)
                {
                    JobRun(size, numberOfBatch);
                }                                
            }
            catch (System.Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                      .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                      .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                      .ToString();

                logMessage.LogError();

                return Task.FromResult(false);
            }

            return Task.FromResult(true);            
        }

        private void JobRun(int size, int numberOfBatch)
        {
            var rawAccounts = accountServices.GetRawAccount(size, numberOfBatch);
            if (rawAccounts.Any())
            {
                var groupAccounts = rawAccounts.GroupBy(x => x.BatchId);

                Parallel.ForEach(groupAccounts, accounts =>
                {
                    var result = accountServices.TransformAccount(accounts);

                    if (result.ErrorCode != 0)
                    {
                        result.ErrorMessage.LogError();
                    }
                });
            }
        }
    }
}
