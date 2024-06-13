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
        IEnumerable<DetectAssBySharedIPMatchesEntity> DetectAssociationBySharedMatches(string custIDList);

        IEnumerable<DetectAssociationMergeGroupEntity> DetectAssociationRuleHedging(string ctsCustIds, string agentCTSCustIds);

        IEnumerable<DetectAssociationMergeGroupEntity> DetectAssociation(DetectAssociationParamModel model);

        IEnumerable<DetectAssociationMergeGroupLv1Entity> DetectAssociationLv1(DetectAssociationParamModel model);
    }

    public class AssociationDetectionService : IAssociationDetectionService
    {
        private readonly IAssociationDetectionDataAccess _associationDetectionDataAccess;

        public AssociationDetectionService(IAssociationDetectionDataAccess associationDetectionDataAccess)
        {
            _associationDetectionDataAccess = associationDetectionDataAccess;
        }

        #region New core Associated Detection
        public IEnumerable<DetectAssBySharedIPMatchesEntity> DetectAssociationBySharedMatches(string custIDList)
        {
            if (string.IsNullOrWhiteSpace(custIDList))
            {
                return new List<DetectAssBySharedIPMatchesEntity>();
            }

            return _associationDetectionDataAccess.DetectAssociationBySharedMatches(custIDList);
        }

        private string GetAssociationCustByAgent(string ctsCustIDs)
        {
            var assByAgent = _associationDetectionDataAccess.DetectAssociationByAgent(ctsCustIDs);
            var mapperAssByAgent = MapperInputFromAssociationCust(assByAgent);

            return RenderExcessiveJsonFromIEnumerable(mapperAssByAgent);
        }

        private string GetAssociationCustByPrimary(DetectAssociationByPrimaryFilterModel custAssType)
        {
            var assByPrimary = _associationDetectionDataAccess.DetectAssociationByPrimary(custAssType);
            var mapperAssByPrimary = MapperInputFromAssociationCust(assByPrimary);

            return RenderExcessiveJsonFromIEnumerable(mapperAssByPrimary);
        }

        private string GetAssociationCustByPrimaryLv1(string ctsCustIds, AssociationDetectionType associationType)
        {
            var requestModel = new DetectAssociationByPrimaryFilterModel(ctsCustIds, associationType);
            var assByPrimary = _associationDetectionDataAccess.DetectAssociationByPrimaryLv1(requestModel);

            return RenderExcessiveJsonFromIEnumerable(assByPrimary);
        }

        private string GetAssociationBySharedMatches(string custIDList, IReadOnlyDictionary<int, int> mappingCustDictionary)
        {
            var assDetectShareMatches = _associationDetectionDataAccess.DetectAssociationBySharedMatches(custIDList);

            var mappingCustIdToCTSCustId = assDetectShareMatches.Select(x =>
            {
                mappingCustDictionary.TryGetValue(x.CustID1, out var ctsCustID1);
                mappingCustDictionary.TryGetValue(x.CustID2, out var ctsCustID2);

                return new DetectAssBySharedIPMatchesEntity
                {
                    CustID1 = ctsCustID1,
                    CustID2 = ctsCustID2
                };
            }).Where(x => x.CustID1 != 0 && x.CustID2 != 0);

            return JsonConvert.SerializeObject(mappingCustIdToCTSCustId);
        }

        private IEnumerable<object> MapperInputFromAssociationCust(IEnumerable<DetectAssociationByPrimaryEntity> assList)
        {
            return assList.Select(c => new
            {
                O = c.OrigID,
                D = c.DestID,
                A = c.AssociationType
            });
        }

        private string RenderExcessiveJsonFromIEnumerable<T>(IEnumerable<T> input) where T : class
        {
            return input.Any() ? Utilities.RenderExcessiveJson(input, 900000) : "[]";
        }
        #endregion

        #region MM Hedging

        public IEnumerable<DetectAssociationMergeGroupEntity> DetectAssociationRuleHedging(string ctsCustIds, string agentCTSCustIds)
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

            return Enumerable.Empty<DetectAssociationMergeGroupEntity>();
        }

        #endregion

        #region Detect Association
        public IEnumerable<DetectAssociationMergeGroupEntity> DetectAssociation(DetectAssociationParamModel model)
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

            return Enumerable.Empty<DetectAssociationMergeGroupEntity>();
        }

        public IEnumerable<DetectAssociationMergeGroupLv1Entity> DetectAssociationLv1(DetectAssociationParamModel model)
        {
            if (!string.IsNullOrEmpty(model?.CTSCustIds))
            {
                var mergeGroupInfo = new MergeGroupAssociationFilterLv1Model();

                Parallel.Invoke(() =>
                {
                    mergeGroupInfo.AssociationByDeviceJson =
                        model.IsDevice ? GetAssociationCustByPrimaryLv1(model.CTSCustIds, AssociationDetectionType.Device) : "[]";
                }, () =>
                {
                    mergeGroupInfo.AssociationByAIJson =
                        model.IsAI ? GetAssociationCustByPrimaryLv1(model.CTSCustIds, AssociationDetectionType.AI) : "[]";
                }, () =>
                {
                    mergeGroupInfo.AssociationByIPJson =
                        model.IsIP ? GetAssociationCustByPrimaryLv1(model.CTSCustIds, AssociationDetectionType.IP) : "[]";
                }, () =>
                {
                    if (model.Is3MatchesLast7Days && model.CustIds.IsNotNullOrEmpty() && model.MappingCustDictionary.IsNotNullOrEmpty())
                    {
                        mergeGroupInfo.AssociationBySharedMatchesJson = GetAssociationBySharedMatches(model.CustIds, model.MappingCustDictionary);
                    }
                });

                var assMergeGroup = _associationDetectionDataAccess.DetectGroupDetectAssociationLv1(mergeGroupInfo);

                return assMergeGroup;
            }

            return Enumerable.Empty<DetectAssociationMergeGroupLv1Entity>();
        }
        #endregion
    }
}
