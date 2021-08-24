using Fanex.NAP.SPU.CTS.Shares;
using System.Threading;

namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    public interface IAffectedEvidenceFromQueueService
    {
        void InsertAssociationAffectedEvidenceFromQueue(int batchSize);

        void RemoveAssociationAffectedEvidenceFromQueue(int batchSize);
    }

    public class AffectedEvidenceFromQueueService : IAffectedEvidenceFromQueueService
    {
        private readonly IAffectedEvidenceFromQueueDataAccess dataAccess;

        public AffectedEvidenceFromQueueService(IAffectedEvidenceFromQueueDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void InsertAssociationAffectedEvidenceFromQueue(int batchSize)
        {
            var isContinue = true;

            while (isContinue)
            {
                isContinue = dataAccess.InsertAssociationAffectedEvidence(batchSize);

                Thread.Sleep(Consts.AffectedEvidence_SleepTimes);
            }
        }

        public void RemoveAssociationAffectedEvidenceFromQueue(int batchSize)
        {
            var isContinue = true;

            while (isContinue)
            {
                isContinue = dataAccess.RemoveAssociationAffectedEvidence(batchSize);

                Thread.Sleep(Consts.AffectedEvidence_SleepTimes);
            }
        }
    }
}