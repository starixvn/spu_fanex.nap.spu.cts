namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IAssociationDetectionService
    {
        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList);

        IEnumerable<DetectAssociationMergeGroupModel> DetectAssociationRuleHedging(IEnumerable<long> ctsCustIds, string agentCustIds);
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

            return input.Any() ? JsonConvert.SerializeObject(input) : "[]";
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

            return input.Any() ? JsonConvert.SerializeObject(input) : "[]";
        }

        #endregion

        #region MM Hedging

        public IEnumerable<DetectAssociationMergeGroupModel> DetectAssociationRuleHedging(IEnumerable<long> ctsCustIds, string agentCustIds)
        {
            if (ctsCustIds.Any())
            {
                var ctsCustIDs = string.Join(",", ctsCustIds.Select(c => c).Distinct());

                var mergeGroupInfo = new MergeGroupAssociationFilterModel { CTSCustIDs = ctsCustIDs, IsGetLogTransInfo = false };

                Parallel.Invoke(() =>
                {
                    mergeGroupInfo.AssociationByDeviceJson = GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIDs, AssociationDetectionType.Device));
                }, () =>
                {

                    mergeGroupInfo.AssociationByAIJson = GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIDs, AssociationDetectionType.AI));
                }, () =>
                {

                    mergeGroupInfo.AssociationByIPJson = GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIDs, AssociationDetectionType.IP));
                }, () =>
                {

                    mergeGroupInfo.AssociationByAgentJson = GetAssociationCustByAgent(agentCustIds);
                });

                var assMergeGroup = _associationDetectionDataAccess.DetectGroupDetectAssociation(mergeGroupInfo);

                return assMergeGroup;
            }

            return Enumerable.Empty<DetectAssociationMergeGroupModel>();
        }

        #endregion
    }
}
