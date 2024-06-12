namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IAssociationDetectionService
    {
        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList);

        IEnumerable<DetectAssociationMergeGroupModel> DetectAssociationRuleHedging(string ctsCustIds, string agentCTSCustIds);

        IEnumerable<DetectAssociationMergeGroupModel> DetectAssociation(DetectAssociationParamModel model);
    }

    public class AssociationDetectionService : IAssociationDetectionService
    {
        private readonly IAssociationDetectionDataAccess _associationDetectionDataAccess;

        public AssociationDetectionService(IAssociationDetectionDataAccess associationDetectionDataAccess)
        {
            _associationDetectionDataAccess = associationDetectionDataAccess;
        }

        #region 
        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList)
        {
            if (string.IsNullOrWhiteSpace(custIDList))
            {
                return new List<DetectAssBySharedIPMatchesModel>();
            }

            return _associationDetectionDataAccess.DetectAssociationBySharedMatches(custIDList);
        }

        private string GetAssociationBySharedMatches(string custIDList)
        {
            var result = DetectAssociationBySharedMatches(custIDList);

            return JsonConvert.SerializeObject(result);
        }
        #endregion

        #region New core Associated Detection

        private string GetAssociationCustByAgent(string ctsCustIDs)
        {
            var assByAgent = _associationDetectionDataAccess.DetectAssociationByAgent(ctsCustIDs);

            var input = assByAgent.Select(c => new
            {
                O = c.OrigID,
                D = c.DestID,
                A = c.AssociationType
            });

            return input.Any() ? Utilities.RenderExcessiveJson(input, 900000) : "[]";
        }

        private string GetAssociationCustByPrimary(DetectAssociationByPrimaryFilterModel custAssType)
        {
            var assByPrimary = _associationDetectionDataAccess.DetectAssociationByPrimary(custAssType);

            var input = assByPrimary.Select(c => new
            {
                O = c.OrigID,
                D = c.DestID,
                A = c.AssociationType
            });

            return input.Any() ? Utilities.RenderExcessiveJson(input, 900000) : "[]";
        }

        #endregion

        #region MM Hedging

        public IEnumerable<DetectAssociationMergeGroupModel> DetectAssociationRuleHedging(string ctsCustIds, string agentCTSCustIds)
        {
            if (!string.IsNullOrEmpty(ctsCustIds))
            {
                var mergeGroupInfo = new MergeGroupAssociationFilterModel { CTSCustIDs = ctsCustIds, IsGetLogTransInfo = false };

                Parallel.Invoke(() =>
                {
                    mergeGroupInfo.AssociationByDeviceJson = GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIds, AssociationDetectionType.Device));
                }, () =>
                {

                    mergeGroupInfo.AssociationByAIJson = GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIds, AssociationDetectionType.AI));
                }, () =>
                {
                    //mergeGroupInfo.AssociationByIPJson = GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIds, AssociationDetectionType.IP));
                    mergeGroupInfo.AssociationByIPJson = "[]";
                }, () =>
                {

                    mergeGroupInfo.AssociationByAgentJson = GetAssociationCustByAgent(agentCTSCustIds);
                });

                var assMergeGroup = _associationDetectionDataAccess.DetectGroupDetectAssociation(mergeGroupInfo);

                return assMergeGroup;
            }

            return Enumerable.Empty<DetectAssociationMergeGroupModel>();
        }

        #endregion

        #region MM GroupBetting
        public IEnumerable<DetectAssociationMergeGroupModel> DetectAssociation(DetectAssociationParamModel model)
        {
            if (!string.IsNullOrEmpty(model?.CTSCustIds))
            {
                var mergeGroupInfo = new MergeGroupAssociationFilterModel { CTSCustIDs = model.CTSCustIds, IsGetLogTransInfo = false };

                Parallel.Invoke(() =>
                {
                    mergeGroupInfo.AssociationByDeviceJson =
                        model.IsDevice ? GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(model.CTSCustIds, AssociationDetectionType.Device)) : "[]";
                }, () =>
                {

                    mergeGroupInfo.AssociationByAIJson =
                        model.IsAI ? GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(model.CTSCustIds, AssociationDetectionType.AI)) : "[]";
                }, () =>
                {
                    mergeGroupInfo.AssociationByIPJson =
                        model.IsIP ? GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(model.CTSCustIds, AssociationDetectionType.IP)) : "[]";
                }, () =>
                {
                    mergeGroupInfo.AssociationBySharedMatchesJson =
                        model.Is3MatchesLast7Days ? GetAssociationBySharedMatches(model.CTSCustIds) : "[]";
                });

                var assMergeGroup = _associationDetectionDataAccess.DetectGroupDetectAssociation(mergeGroupInfo);

                return assMergeGroup;
            }

            return Enumerable.Empty<DetectAssociationMergeGroupModel>();
        }
        #endregion
    }
}
