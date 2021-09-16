namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IAffectedEvidenceFromQueueJobService
    {
        Task<bool> Execute();
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
            this.serviceName = "PlaceAffectedEvidenceFromQueue";
        }

        public Task<bool> Execute()
        {
            try
            {
                JobRun();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        #region Private Methods

        private void JobRun()
        {
            Parallel.Invoke(() =>
            {
                affectedEvidenceFromQueueService.InsertAssociationAffectedEvidenceFromQueue();
            },
            () =>
            {
                affectedEvidenceFromQueueService.RemoveAssociationAffectedEvidenceFromQueue();
            });
        }

        #endregion Private Methods
    }
}