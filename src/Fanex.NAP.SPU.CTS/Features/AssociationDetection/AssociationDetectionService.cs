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

        private string GetAssociationBySharedMatches(string custIDList, IReadOnlyDictionary<int, int> mappingCustDictionary)
        {
            var assDetectShareMatches = DetectAssociationBySharedMatches(custIDList);

            var mappingCustIdToCTSCustId = assDetectShareMatches.Select(x =>
            {
                mappingCustDictionary.TryGetValue(x.CustID1, out var ctsCustID1);
                mappingCustDictionary.TryGetValue(x.CustID2, out var ctsCustID2);

                return new DetectAssBySharedIPMatchesModel
                {
                    CustID1 = ctsCustID1,
                    CustID2 = ctsCustID2
                };
            }).Where(x => x.CustID1 != 0 && x.CustID2 != 0);

            return JsonConvert.SerializeObject(mappingCustIdToCTSCustId);
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
                    mergeGroupInfo.AssociationByIPJson = GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIds, AssociationDetectionType.IP));
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
            if ((model?.CTSCustIds).IsNotNullOrEmpty())
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
                    if (model.Is3MatchesLast7Days && model.CustIds.IsNotNullOrEmpty() && model.MappingCustDictionary.IsNotNullOrEmpty())
                    {
                        mergeGroupInfo.AssociationBySharedMatchesJson = GetAssociationBySharedMatches(model.CustIds, model.MappingCustDictionary);
                    }
                }, () =>
                {
                    mergeGroupInfo.AssociationByAgentJson =
                        model.AgentCTSCustIds.IsNotNullOrEmpty() ? GetAssociationCustByAgent(model.AgentCTSCustIds) : "[]";
                });

                var assMergeGroup = _associationDetectionDataAccess.DetectGroupDetectAssociation(mergeGroupInfo);

                return assMergeGroup;
            }

            return Enumerable.Empty<DetectAssociationMergeGroupModel>();
        }
        #endregion
    }
}
