namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence.Criteria;
    using Fanex.NAP.SPU.CTS.Shares.Models;
    using System;
    using System.Collections.Generic;

    public interface IAffectedEvidenceFromQueueDataAccess
    {
        ResultAffectedEvidenceFromQueueEntity InsertAssociationAffectedEvidence();

        ResultAffectedEvidenceFromQueueEntity RemoveAssociationAffectedEvidence();

        DBResult UpdateBetLimitOfFlaggedCustomer(string custIds);

        DBResult UpdateBetLimitOfUnflaggedCustomer(string custIds);
    }

    public class AffectedEvidenceFromQueueDataAccess : IAffectedEvidenceFromQueueDataAccess
    {
        private readonly IDynamicRepository repository;

        public AffectedEvidenceFromQueueDataAccess()
        {
            repository = new DynamicRepository();
        }

        public ResultAffectedEvidenceFromQueueEntity InsertAssociationAffectedEvidence()
        {
            var criteria = new AffectedEvidenceFromQueueInsertCriteria();

            var custIds = repository.Fetch<long>(criteria);

            return new ResultAffectedEvidenceFromQueueEntity
            {
                CustIds = custIds ?? new List<long>(),
                IsContinue = Convert.ToBoolean(criteria.IsDataExist)
            };
        }

        public ResultAffectedEvidenceFromQueueEntity RemoveAssociationAffectedEvidence()
        {
            var criteria = new AffectedEvidenceFromQueueRemoveCriteria();

            var custIds = repository.Fetch<long>(criteria);

            return new ResultAffectedEvidenceFromQueueEntity
            {
                CustIds = custIds ?? new List<long>(),
                IsContinue = Convert.ToBoolean(criteria.IsDataExist)
            };
        }

        public DBResult UpdateBetLimitOfFlaggedCustomer(string custIds)
        {
            var criteria = new BetLimitByEvidenceAddingUpdateCriteria
            {
                CustIDList = custIds
            };

            repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = string.IsNullOrEmpty(criteria.ErrorMessage) ? 0 : -1,
                ErrorMsg = criteria.ErrorMessage
            };
        }

        public DBResult UpdateBetLimitOfUnflaggedCustomer(string custIds)
        {
            var criteria = new BetLimitByEvidenceRemovingUpdateCriteria
            {
                CustIDList = custIds
            };

            repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = string.IsNullOrEmpty(criteria.ErrorMessage) ? 0 : -1,
                ErrorMsg = criteria.ErrorMessage
            };
        }
    }
}