namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    using Fanex.NAP.SPU.CTS.Shares;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

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
            var result = new ResultAffectedEvidenceFromQueueEntity { CustIds = new List<long>(), IsContinue = true };

            while (result.IsContinue)
            {
                result = dataAccess.InsertAssociationAffectedEvidence();

                if (result.CustIds.Any())
                {
                    dataAccess.UpdateBetLimitOfFlaggedCustomer(string.Join(",", result.CustIds));
                }

                Thread.Sleep(Consts.AffectedEvidence_SleepTimes);
            }
        }

        public void RemoveAssociationAffectedEvidenceFromQueue()
        {
            var result = new ResultAffectedEvidenceFromQueueEntity { CustIds = new List<long>(), IsContinue = true };

            while (result.IsContinue)
            {
                result = dataAccess.RemoveAssociationAffectedEvidence();

                if (result.CustIds.Any())
                {
                    dataAccess.UpdateBetLimitOfUnflaggedCustomer(string.Join(",", result.CustIds));
                }

                Thread.Sleep(Consts.AffectedEvidence_SleepTimes);
            }
        }
    }
}