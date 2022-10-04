namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveHistoryClassificationBySportJobService
    {
        Task<bool> ArchiveHistoryClassificationBySport();
    }

    public class ArchiveHistoryClassificationBySportJobService : IArchiveHistoryClassificationBySportJobService
    {
        private readonly IArchiveHistoryClassificationBySportServices archiveCCHistoryBySportServices;
        private readonly ISentryClient sentryClient;
        private readonly string serviceName;

        public ArchiveHistoryClassificationBySportJobService(ISentryClient sentryClient, IArchiveHistoryClassificationBySportServices archiveHistoryClassificationBySportServices)
        {
            this.sentryClient = sentryClient;
            this.archiveCCHistoryBySportServices = archiveHistoryClassificationBySportServices;
            this.serviceName = "ArchiveHistoryClassificationBySportJobService";
        }

        public Task<bool> ArchiveHistoryClassificationBySport()
        {
            try
            {
                archiveCCHistoryBySportServices.ArchiveHistoryClassificationBySport();
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