namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.RealtimeClassification;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IRealtimeClassificationJobService
    {
        Task<bool> Execute(int batchSize);
    }

    public class RealtimeClassificationJobService : IRealtimeClassificationJobService
    {
        private readonly string serviceName;
        private readonly ISentryClient sentryClient;
        private readonly IRealtimeClassificationServices realtimeClassificationServices;

        public RealtimeClassificationJobService(
            ISentryClient sentryClient,
            IRealtimeClassificationServices realtimeClassificationServices)
        {
            this.sentryClient = sentryClient;
            this.realtimeClassificationServices = realtimeClassificationServices;
            this.serviceName = "DailyClassification";
        }

        public Task<bool> Execute(int batchSize)
        {
            try
            {
                if (batchSize > 0)
                {
                    JobRun(batchSize);
                }
                else
                {
                    Utilities.LogSentryError(serviceName, "InValid Batch size", (batchSize), new NullReferenceException(), sentryClient);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        #region Private Methods

        private void JobRun(int batchSize)
        {
            const string categoryIds = "201, 202, 205, 231, 232, 233, 222, 221, 223, 212, 211, 213"; // Hard code for 'New member' & 'Main Normal Account' & 'Main VIP'  & special categories

            realtimeClassificationServices
                .GetCustomerGeneralCategoryHasChangeForRunRealtime()
                .ScanningRealtimeClassification(batchSize, categoryIds)
                .RealtimeUpdateLastScanTime(true);
        }
    }

    #endregion Private Methods
}