namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IPlaceAffectedEvidenceFromQueue
    {
        Task<bool> Execute();
    }

    public class PlaceAffectedEvidenceFromQueue : IPlaceAffectedEvidenceFromQueue
    {
        private readonly string serviceName;
        private readonly ISentryClient sentryClient;
        private readonly IAffectedEvidenceFromQueueService affectedEvidenceFromQueueService;
        private readonly IAppSettings appSettings;

        public PlaceAffectedEvidenceFromQueue(
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
                if (appSettings.AffectedEvidenceFromQueueBatchSize > 0)
                {
                    JobRun();
                }
                else
                {
                    Utilities.LogSentryError(serviceName, "Invalid BatchSize", (appSettings.AffectedEvidenceFromQueueBatchSize), new NullReferenceException(), sentryClient);
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

        private void JobRun()
        {
            Parallel.Invoke(() =>
            {
                affectedEvidenceFromQueueService.InsertAssociationAffectedEvidenceFromQueue(appSettings.AffectedEvidenceFromQueueBatchSize);
            },
            () =>
            {
                affectedEvidenceFromQueueService.RemoveAssociationAffectedEvidenceFromQueue(appSettings.AffectedEvidenceFromQueueBatchSize);
            });
        }

        #endregion Private Methods
    }
}