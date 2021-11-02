namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence.Criteria;
    using System.Collections.Generic;

    public interface IAffectedEvidenceDataAccess
    {
        IEnumerable<int> TransformAssociationAffectedEvidence();

        void UpdateBetLimitOfFlaggedCustomer(string custIds);
    }

    public class AffectedEvidenceDataAccess : IAffectedEvidenceDataAccess
    {
        private readonly IDynamicRepository repository;

        public AffectedEvidenceDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<int> TransformAssociationAffectedEvidence()
        {
            var criteria = new TransformAssociationAffectedEvidenceCriteria();

            return repository.Fetch<int>(criteria);
        }

        public void UpdateBetLimitOfFlaggedCustomer(string custIds)
        {
            var criteria = new BetLimitByEvidenceRemovingUpdateCriteria
            {
                CustIDList = custIds
            };

            repository.Execute(criteria);
        }
    }
}
