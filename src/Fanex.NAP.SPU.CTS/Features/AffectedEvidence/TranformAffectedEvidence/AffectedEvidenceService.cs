namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    using Sentry;
    using System.Collections.Generic;
    using System.Linq;

    public interface IAffectedEvidenceService
    {
        IEnumerable<int> TransformAssociationAffectedEvidence(int batchSize);

        void UpdateBetLimitOfFlaggedCustomer(IEnumerable<int> custIds);
    }

    public class AffectedEvidenceService : IAffectedEvidenceService
    {
        private readonly IAffectedEvidenceDataAccess dataAccess;

        public AffectedEvidenceService(IAffectedEvidenceDataAccess dataAccess, ISentryClient sentryClient)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<int> TransformAssociationAffectedEvidence(int batchSize)
        {
            return dataAccess.TransformAssociationAffectedEvidence(batchSize);
        }

        public void UpdateBetLimitOfFlaggedCustomer(IEnumerable<int> custIds)
        {
            if(custIds != null && custIds.Any())
            {
                dataAccess.UpdateBetLimitOfFlaggedCustomer(string.Join(",", custIds));
            }
        }
    }
}