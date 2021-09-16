namespace Fanex.NAP.SPU.CTS.JobService.Services.ArchiveData
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveCustomerJobService
    {
        Task<bool> ArchiveCTSCustomer(int batchSize);
    }

    public class ArchiveCustomerJobService : IArchiveCustomerJobService
    {
        private readonly IArchiveCustomerService archiveCustomerService;
        private readonly ISentryClient sentryClient;

        public ArchiveCustomerJobService(IArchiveCustomerService archiveCustomerService, ISentryClient sentryClient)
        {
            this.archiveCustomerService = archiveCustomerService;
            this.sentryClient = sentryClient;
        }

        public Task<bool> ArchiveCTSCustomer(int batchSize)
        {
            try
            {
                archiveCustomerService.ArchiveCTSCustomer(batchSize);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                      .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                      .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                      .ToString();

                sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);
                return Task.FromResult(false);
            }
        }

    }
}
