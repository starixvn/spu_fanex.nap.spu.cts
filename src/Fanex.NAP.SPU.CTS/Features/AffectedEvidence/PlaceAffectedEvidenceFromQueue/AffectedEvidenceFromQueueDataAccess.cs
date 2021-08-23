namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence.Criteria;

    public interface IAffectedEvidenceFromQueueDataAccess
    {
        bool InsertAssociationAffectedEvidence(int batchSize);

        bool RemoveAssociationAffectedEvidence(int batchSize);
    }

    public class AffectedEvidenceFromQueueDataAccess : IAffectedEvidenceFromQueueDataAccess
    {
        private readonly IDynamicRepository repository;

        public AffectedEvidenceFromQueueDataAccess()
        {
            repository = new DynamicRepository();
        }

        public bool InsertAssociationAffectedEvidence(int batchSize)
        {
            var criteria = new AffectedEvidenceFromQueueInsertCriteria
            {
                BatchSize = batchSize
            };

            repository.Execute(criteria);

            return criteria.IsExist;
        }

        public bool RemoveAssociationAffectedEvidence(int batchSize)
        {
            var criteria = new AffectedEvidenceFromQueueRemoveCriteria
            {
                BatchSize = batchSize
            };

            repository.Execute(criteria);

            return criteria.IsExist;
        }
    }
}