namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IAffectedEvidenceFromQueueService
    {
        void InsertAssociationAffectedEvidenceFromQueue();

        void RemoveAssociationAffectedEvidenceFromQueue();
    }

    public class AffectedEvidenceFromQueueService : IAffectedEvidenceFromQueueService
    {
        private readonly IAffectedEvidenceFromQueueDataAccess dataAccess;

        public AffectedEvidenceFromQueueService(IAffectedEvidenceFromQueueDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void InsertAssociationAffectedEvidenceFromQueue()
        {
            var result = dataAccess.InsertAssociationAffectedEvidence();

            if (result.CustIds.Any())
            {
                UpdateBetLimitOfFlaggedCustomer(result.CustIds);
            }
        }

        public void RemoveAssociationAffectedEvidenceFromQueue()
        {
            var result = dataAccess.RemoveAssociationAffectedEvidence();

            if (result.CustIds.Any())
            {
                UpdateBetLimitOfUnflaggedCustomer(result.CustIds);
            }
        }

        private void UpdateBetLimitOfFlaggedCustomer(IEnumerable<long> custIds)
        {
            if(custIds != null && custIds.Any())
            {
                dataAccess.UpdateBetLimitOfFlaggedCustomer(string.Join(",", custIds));
            }
        }

        private void UpdateBetLimitOfUnflaggedCustomer(IEnumerable<long> custIds)
        {
            if (custIds != null && custIds.Any())
            {
                dataAccess.UpdateBetLimitOfUnflaggedCustomer(string.Join(",", custIds));
            }
        }
    }
}