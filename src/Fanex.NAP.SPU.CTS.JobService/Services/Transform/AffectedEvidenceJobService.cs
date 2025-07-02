namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
    using Fanex.NAP.SPU.CTS.Shares;
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

        public AffectedEvidenceJobService(IAffectedEvidenceService affectedEvidenceService)
        {
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

                logMessage.LogError();
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
