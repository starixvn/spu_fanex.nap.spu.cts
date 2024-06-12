namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Criteria;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IAssociationDetectionDataAccess
    {
        IEnumerable<DetectAssBySharedIPMatchesEntity> DetectAssociationBySharedMatches(string custIDList);

        #region New core Associated Detection

        IEnumerable<DetectAssociationByPrimaryEntity> DetectAssociationByPrimary(DetectAssociationByPrimaryFilterModel filter);

        IEnumerable<DetectAssociationByPrimaryEntity> DetectAssociationByPrimaryLv1(DetectAssociationByPrimaryFilterModel filter);

        IEnumerable<DetectAssociationByPrimaryEntity> DetectAssociationByAgent(string ctsCustIDs);

        IEnumerable<DetectAssociationMergeGroupEntity> DetectGroupDetectAssociation(MergeGroupAssociationFilterModel model);

        IEnumerable<DetectAssociationMergeGroupLv1Entity> DetectGroupDetectAssociationLv1(MergeGroupAssociationFilterLv1Model model);
        #endregion
    }

    public class AssociationDetectionDataAccess : IAssociationDetectionDataAccess
    {
        private readonly IDynamicRepository _repository;

        public AssociationDetectionDataAccess()
        {
            _repository = new DynamicRepository();
        }

        #region 
        public IEnumerable<DetectAssBySharedIPMatchesEntity> DetectAssociationBySharedMatches(string custIDList)
        {
            var criteria = new DetectAssociationBySharedMatchesCriteria
            {
                CustIDList = custIDList,
                QueryType = (int)DetectAssociation_QueryType.DetectAss
            };

            return _repository.Fetch<DetectAssBySharedIPMatchesEntity>(criteria);
        }
        #endregion

        #region New core Associated Detection
        public IEnumerable<DetectAssociationByPrimaryEntity> DetectAssociationByPrimary(DetectAssociationByPrimaryFilterModel filter)
        {
            var criteria = new DetectAssociationByPrimaryCriteria
            {
                CTSCustIDs = filter.CTSCustIDs,
                IsAI = filter.IsAI,
                IsDevice = filter.IsDevice,
                IsIP = filter.IsIP
            };

            return _repository.Fetch<DetectAssociationByPrimaryEntity>(criteria);
        }

        public IEnumerable<DetectAssociationByPrimaryEntity> DetectAssociationByAgent(string ctsCustIDs)
        {
            var criteria = new DetectAssociationByAgentCriteria
            {
                CTSCustIDs = ctsCustIDs
            };

            return _repository.Fetch<DetectAssociationByPrimaryEntity>(criteria);
        }

        public IEnumerable<DetectAssociationMergeGroupEntity> DetectGroupDetectAssociation(MergeGroupAssociationFilterModel model)
        {
            var criteria = new DetectAssociationMergeGroupCriteria
            {
                CTSCustIDs = model.CTSCustIDs,
                DeviceJson = model.AssociationByDeviceJson,
                AIJson = model.AssociationByAIJson,
                IPJson = model.AssociationByIPJson,
                AgentJson = model.AssociationByAgentJson,
                SharedIPJson = model.AssociationBySharedIPJson,
                SharedMatchesJson = model.AssociationBySharedMatchesJson,
                IsGetLogTransInfo = model.IsGetLogTransInfo
            };

            return _repository.Fetch<DetectAssociationMergeGroupEntity>(criteria);
        }

        public IEnumerable<DetectAssociationByPrimaryEntity> DetectAssociationByPrimaryLv1(DetectAssociationByPrimaryFilterModel filter)
        {
            var criteria = new DetectAssociationByPrimaryLv1Criteria
            {
                CTSCustIDs = filter.CTSCustIDs,
                IsAI = filter.IsAI,
                IsDevice = filter.IsDevice,
                IsIP = filter.IsIP
            };

            return _repository.Fetch<DetectAssociationByPrimaryEntity>(criteria);
        }

        public IEnumerable<DetectAssociationMergeGroupLv1Entity> DetectGroupDetectAssociationLv1(MergeGroupAssociationFilterLv1Model model)
        {
            var criteria = new DetectAssociationMergeGroupLv1Criteria
            {
                DeviceJson = model.AssociationByDeviceJson,
                AIJson = model.AssociationByAIJson,
                IPJson = model.AssociationByIPJson,
                SharedMatchesJson = model.AssociationBySharedMatchesJson,
            };

            return _repository.Fetch<DetectAssociationMergeGroupLv1Entity>(criteria);
        }
        #endregion
    }
}