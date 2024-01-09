namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Criteria;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IAssociationDetectionDataAccess
    {
        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList);

        #region New core Associated Detection

        public IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByPrimary(DetectAssociationByPrimaryFilterModel filter);

        public IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByAgent(string ctsCustIDs);

        IEnumerable<DetectAssociationMergeGroupModel> DetectGroupDetectAssociation(MergeGroupAssociationFilterModel model);

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
        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList)
        {
            var criteria = new DetectAssociationBySharedMatchesCriteria
            {
                CustIDList = custIDList,
                QueryType = (int)DetectAssociation_QueryType.DetectAss
            };

            return _repository.Fetch<DetectAssBySharedIPMatchesModel>(criteria);
        }
        #endregion

        #region New core Associated Detection
        public IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByPrimary(DetectAssociationByPrimaryFilterModel filter)
        {
            var criteria = new DetectAssociationByPrimaryCriteria
            {
                CTSCustIDs = filter.CTSCustIDs,
                IsAI = filter.IsAI,
                IsDevice = filter.IsDevice,
                IsIP = filter.IsIP
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<DetectAssociationByPrimaryModel>(criteria);
            }
        }

        public IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByAgent(string ctsCustIDs)
        {
            var criteria = new DetectAssociationByAgentCriteria
            {
                CTSCustIDs = ctsCustIDs
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<DetectAssociationByPrimaryModel>(criteria);
            }
        }

        public IEnumerable<DetectAssociationMergeGroupModel> DetectGroupDetectAssociation(MergeGroupAssociationFilterModel model)
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

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<DetectAssociationMergeGroupModel>(criteria);
            }
        }


        #endregion
    }
}
