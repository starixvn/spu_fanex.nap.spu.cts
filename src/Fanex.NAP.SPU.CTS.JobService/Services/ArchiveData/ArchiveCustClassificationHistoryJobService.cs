namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveCustClassificationHistoryJobService
    {
        Task<bool> ArchiveChangedCustClassHistory();

        Task<bool> CleanUpUnChangedCustClassHistory();
    }

    public class ArchiveCustClassificationHistoryJobService : IArchiveCustClassificationHistoryJobService
    {
        private readonly IArchiveCustClassificationHistoryServices archiveCustClassHistoryServices;
        private readonly ISentryClient sentryClient;
        private readonly string serviceName;

        public ArchiveCustClassificationHistoryJobService(ISentryClient sentryClient, IArchiveCustClassificationHistoryServices archiveCustClassHistoryServices)
        {
            this.sentryClient = sentryClient;
            this.archiveCustClassHistoryServices = archiveCustClassHistoryServices;
            this.serviceName = "ArchiveCustClassificationHistory";
        }

        public Task<bool> ArchiveChangedCustClassHistory()
        {
            try
            {
                ArchiveChangedCustClassificationHistory();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> CleanUpUnChangedCustClassHistory()
        {
            try
            {
                CleanUpUnChangedCustClassificationHistory();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void ArchiveChangedCustClassificationHistory()
        {
            archiveCustClassHistoryServices.ArchiveChangedCustClassificationHistory();
        }

        private void CleanUpUnChangedCustClassificationHistory()
        {
            archiveCustClassHistoryServices.CleanUpUnChangedCustClassificationHistory();
        }
    }
}