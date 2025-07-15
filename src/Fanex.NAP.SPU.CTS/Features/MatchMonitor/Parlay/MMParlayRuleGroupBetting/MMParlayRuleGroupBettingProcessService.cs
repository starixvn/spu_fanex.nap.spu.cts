namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class MMParlayRuleGroupBettingProcessService : IMMParlayRuleProcessService<MMParlayStagingRuleGroupBettingGroupModel>
    {
        private readonly IMMParlayRuleGroupBettingDataAccess _dataAccess;
        private readonly IAssociationDetectionService _associationDetection;
        private const string ServiceName = "Match Monitor Parlay Process Rule Group Betting Service";

        private MMParlayStagingRuleGroupBettingGroupModel MatchStaging;
        private readonly List<MMParlayProcessRuleGroupBettingModel> MatchProcesses;
        private bool IsLive;

        public MMParlayRuleGroupBettingProcessService(IAssociationDetectionService associationDetection, IMMParlayRuleGroupBettingDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _associationDetection = associationDetection;

            MatchProcesses = new List<MMParlayProcessRuleGroupBettingModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "<Pending>")]
        public IMMParlayRuleProcessService PrepareProcess(MMParlayStagingRuleGroupBettingGroupModel matchStaging, 
            bool isLive,
            IReadOnlyDictionary<int, int> mappingCustDictionary)
        {
            MatchStaging = matchStaging;
            IsLive = isLive;

            return this;
        }

        public IMMParlayRuleProcessService Complete(ulong maxSequenceID)
        {
            var mergeMatchComplete = MatchProcesses?
                                            .Where(x => x.CompleteMatches.IsNotNullOrEmpty())
                                            .SelectMany(x => x.CompleteMatches)
                                            .GroupBy(item => item.TransIDList)
                                            .Select((group, index) =>
                                            {
                                                var result = group.First();
                                                result.GroupId = index + 1;
                                                return result;
                                            });

            if (mergeMatchComplete.IsNotNullOrEmpty())
            {
                var para = new MMParlayCompleteRuleGroupBettingParameterModel
                {
                    IsLive = IsLive,
                    MatchInfo = MatchStaging,
                    MaxSequenceID = maxSequenceID,
                    TransGroupJson = JsonConvert.SerializeObject(mergeMatchComplete)
                };

                _dataAccess.CompleteTransRuleGroupBetting(para);
            }

            return this;
        }

        public IMMParlayRuleProcessService ProcessRound1()
        {
            if (MatchStaging != null && MatchStaging.MatchGroupStagingList.IsNotNullOrEmpty())
            {
                var matchProcessesRound1 = MatchStaging.MatchGroupStagingList.Select(matchInfo => ProcessWithAssociationRound1(matchInfo, IsLive))
                    .Where(matchInfo => matchInfo != null).ToList();

                MatchProcesses.AddRange(matchProcessesRound1);
            }

            return this;
        }
       
        public IMMParlayRuleProcessService ProcessRound2()
        {
            if (MatchProcesses.IsNotNullOrEmpty())
            {
                var matchProcessesRound2 = MatchProcesses
                            .SelectMany(matchProcessRound1 => matchProcessRound1.ReprocessMatches
                                                .Select(reprocessInfoRound2 => ProcessWithAssociationRound2(matchProcessRound1.MatchInfo, reprocessInfoRound2, IsLive))
                            ).Where(matchInfo => matchInfo != null)
                            .ToList();

                MatchProcesses.AddRange(matchProcessesRound2);
            }

            return this;
        }

        private MMParlayProcessRuleGroupBettingModel ProcessWithAssociationRound1(MMParlayStagingRuleGroupBettingEntity matchInfo, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(matchInfo.CTSCustIDList, matchInfo.CustIDList)?
                                            .Where(n => n.GroupID.HasValue);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                return ProcessRuleGroupBetting(isLive, matchInfo, matchInfo.TransIDmList, JsonConvert.SerializeObject(assDetectionHasGroupID));
            }

            return null;
        }

        private MMParlayProcessRuleGroupBettingModel ProcessWithAssociationRound2(MMParlayStagingRuleGroupBettingEntity matchInfo, 
            MMParlayProcessRuleGroupBettingInfoEntity reprocessModel, 
            bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(reprocessModel.CTSCustIDList, reprocessModel.CustIDList)?
                                            .Where(n => n.GroupID.HasValue);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                var matchProcessRound2 = ProcessRuleGroupBetting(isLive, matchInfo, reprocessModel.TransIDmList, JsonConvert.SerializeObject(assDetectionHasGroupID));
                matchProcessRound2.RoundNumber = 2;

                return matchProcessRound2;
            }

            return null;
        }

        private IEnumerable<DetectAssociationMergeGroupLv1Entity> DetectAssociation(string ctsCustIDList, string custIDList)
        {
            var param = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDList,
                CustIds = custIDList,
                IsDevice = true,
                IsAI = true,
                IsIP = true,
                Is3MatchesLast7Days = true
            };

            return _associationDetection.DetectAssociationLv1(param);
        }

        private MMParlayProcessRuleGroupBettingModel ProcessRuleGroupBetting(bool isLive, 
            MMParlayStagingRuleGroupBettingEntity matchinfo, 
            string transIDmList, 
            string groupDetection)
        {
            return _dataAccess.ProcessRuleGroupBetting(isLive, matchinfo, transIDmList, groupDetection);
        }

        public void WriteLogSentryIfError(string functionName)
        {
            if (MatchProcesses.IsNotNullOrEmpty())
            {
                var matchProcessNotValid = MatchProcesses.Where(x => x.RoundNumber == 2 && x.ReprocessMatches.IsNotNullOrEmpty());

                if (matchProcessNotValid.IsNotNullOrEmpty())
                {
                    Utilities.LogSentryInfo(ServiceName, $"{functionName} Error", matchProcessNotValid, true);
                }
            }
        }
    }
}
