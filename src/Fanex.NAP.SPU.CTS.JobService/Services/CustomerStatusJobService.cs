using Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus;
using Fanex.NAP.SPU.CTS.Shares;
using Sentry;
using System;
using System.Threading.Tasks;
using static Fanex.NAP.SPU.CTS.Shares.Consts;

namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    public interface ICustomerStatusJobService
    {
        Task<bool> Execute();
    }

    public class CustomerStatusJobService : ICustomerStatusJobService
    {
        private readonly ICustomerStatusServices customerStatusServices;
        private readonly ISentryClient sentryClient;
        private readonly IAppSettings appSettings;
        private readonly string serviceName;

        public CustomerStatusJobService(ISentryClient sentryClient, ICustomerStatusServices customerStatusServices, IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.customerStatusServices = customerStatusServices;
            this.appSettings = appSettings;
            this.serviceName = "CustomerStatus";
        }


        public Task<bool> Execute()
        {
            try
            {
                if (appSettings.CustomerStatusBatchSize > 0)
                {
                    JobRun();
                }
                else
                {
                    Utilities.LogSentryError(serviceName, "Invalid BatchSize", appSettings.CustomerStatusBatchSize, new NullReferenceException(), sentryClient);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun()
        {
            customerStatusServices
                .Start()
                .GetCustIds(appSettings.CustomerStatusBatchSize)
                .GetCustomerFromMainDBForUpdate(UpdateCustomer.FromProductStatus)
                .UpdateCustomerStatus();
        }
    }
}
