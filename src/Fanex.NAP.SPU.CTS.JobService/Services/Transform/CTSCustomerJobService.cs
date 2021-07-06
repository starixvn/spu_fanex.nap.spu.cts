namespace Fanex.NAP.SPU.CTS.JobService.Services.Transform
{
    using Fanex.NAP.SPU.CTS.Features.CTSCustomer;
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICTSCustomerJobService
    {
        Task<bool> Execute(int batchSize, int numOfBatch);
    }

    public class CTSCustomerJobService : ICTSCustomerJobService
    {
        private readonly ICTSCustomerService ctscustomerService;
        private readonly ISentryClient sentryClient;
        private readonly IAppSettings appSettings;

        public CTSCustomerJobService(ISentryClient sentryClient, ICTSCustomerService ctscustomerService, IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.ctscustomerService = ctscustomerService;
            this.appSettings = appSettings;
        }

        public Task<bool> Execute(int batchSize, int numOfBatch)
        {
            try
            {
                var lastScanned = ctscustomerService.GetLastInsertCust();
                var customers = ctscustomerService.GetCustomerFromMainDB(batchSize, lastScanned.LastCustID);
                var custSubs = ctscustomerService.GetCustSubFromMainDB(batchSize, lastScanned.LastCustSubID).ToList();
                var custsToCTS = ctscustomerService.PrepareData(customers, custSubs);

                var result = ctscustomerService.InsertNewCustomer(custsToCTS, true, 100);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return Task.FromResult(false);
            }
        }
    }
}
