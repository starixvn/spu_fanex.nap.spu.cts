using Fanex.NAP.SPU.CTS.Features.Accounts;
using Sentry;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.DCS.JobService.Services
{
    public interface IAccountJobService
    {
        Task<bool> Execute(int size, int numberOfBatch);

    }
    public class AccountJobService : IAccountJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IAccountServices accountServices;

        public AccountJobService(ISentryClient sentryClient, IAccountServices accountServices)
        {
            this.sentryClient = sentryClient;
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
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);            
        }

        private void JobRun(int size, int numberOfBatch)
        {
            var rawAccounts = accountServices.GetRawAccount(size, numberOfBatch);

            if (rawAccounts != null && rawAccounts.Any())
            {
                var result = accountServices.TransformAccount(rawAccounts);

                if (result.ErrorCode != 0)
                {
                    sentryClient.CaptureMessage(result.ErrorMessage, Sentry.Protocol.SentryLevel.Error);
                }
            }
        }
    }
}
