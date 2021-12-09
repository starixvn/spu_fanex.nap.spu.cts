namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IAffectedEvidenceFromQueueJobService
    {
        Task<bool> InsertAffectedEvidences();

        Task<bool> RemoveAffectedEvidences();
    }

    public class AffectedEvidenceFromQueueJobService : IAffectedEvidenceFromQueueJobService
    {
        private readonly string serviceName;
        private readonly ISentryClient sentryClient;
        private readonly IAffectedEvidenceFromQueueService affectedEvidenceFromQueueService;
        private readonly IAppSettings appSettings;

        public AffectedEvidenceFromQueueJobService(
            ISentryClient sentryClient,
            IAffectedEvidenceFromQueueService affectedEvidenceFromQueueService,
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.affectedEvidenceFromQueueService = affectedEvidenceFromQueueService;
            this.appSettings = appSettings;
            this.serviceName = "AffectedEvidenceFromQueue";
        }

        public Task<bool> InsertAffectedEvidences()
        {
            try
            {
                affectedEvidenceFromQueueService.InsertAssociationAffectedEvidenceFromQueue();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> RemoveAffectedEvidences()
        {
            try
            {
                affectedEvidenceFromQueueService.RemoveAssociationAffectedEvidenceFromQueue();
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