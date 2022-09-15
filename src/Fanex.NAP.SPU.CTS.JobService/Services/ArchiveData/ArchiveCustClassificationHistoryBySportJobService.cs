namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveHistoryCCBySportJobService
    {
        Task<bool> ArchiveHistoryCCBySport();
    }

    public class ArchiveHistoryCCBySportJobService : IArchiveHistoryCCBySportJobService
    {
        private readonly IArchiveHistoryCCBySportServices archiveHistoryCCBySportServices;
        private readonly ISentryClient sentryClient;
        private readonly string serviceName;

        public ArchiveHistoryCCBySportJobService(ISentryClient sentryClient, IArchiveHistoryCCBySportServices archiveHistoryCCBySportServices)
        {
            this.sentryClient = sentryClient;
            this.archiveHistoryCCBySportServices = archiveHistoryCCBySportServices;
            this.serviceName = "ArchiveCustClassificationHistoryBySportJobService";
        }

        public Task<bool> ArchiveHistoryCCBySport()
        {
            try
            {
                archiveHistoryCCBySportServices.ArchiveHistoryCCBySport();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}