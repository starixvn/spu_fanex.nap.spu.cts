namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
    using Sentry;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAffectedEvidenceJobService
    {
        Task<bool> Execute();
    }

    public class AffectedEvidenceJobService : IAffectedEvidenceJobService
    {
        private readonly IAffectedEvidenceService affectedEvidenceService;
        private readonly ISentryClient sentryClient;

        public AffectedEvidenceJobService(ISentryClient sentryClient, IAffectedEvidenceService affectedEvidenceService)
        {
            this.sentryClient = sentryClient;
            this.affectedEvidenceService = affectedEvidenceService;
        }

        public Task<bool> Execute()
        {
            try
            {
                var custIds = affectedEvidenceService.TransformAssociationAffectedEvidence();

                if (custIds.Any())
                {
                    affectedEvidenceService.UpdateBetLimitOfFlaggedCustomer(custIds);
                }                
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

            return Task.FromResult(true);
        }
    }
}
