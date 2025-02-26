namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public class MatchMonitorRuleGroupBettingProcessService : IMatchMonitorRuleProcessService<MatchStagingRuleGroupBettingGroupModel>
    {
        private readonly IMatchMonitorRuleGroupBettingDataAccess _dataAccess;
        private readonly IAssociationDetectionService _associationDetection;
        private const string ServiceName = "Match Monitor Process Rule GroupBetting Service";

        private MatchStagingRuleGroupBettingGroupModel MatchStaging;
        private bool IsLive;

        private readonly List<MatchProcessRuleGroupBettingModel> MatchProcesses;

        public MatchMonitorRuleGroupBettingProcessService(IAssociationDetectionService associationDetection, IMatchMonitorRuleGroupBettingDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _associationDetection = associationDetection;

            MatchProcesses = new List<MatchProcessRuleGroupBettingModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "<Pending>")]
        public IMatchMonitorRuleProcessService PrepareProcess(MatchStagingRuleGroupBettingGroupModel matchStaging, bool isLive, IReadOnlyDictionary<int, int> mappingCustDictionary)
        {
            MatchStaging = matchStaging;
            IsLive = isLive;

            return this;
        }

        #region Process
        public IMatchMonitorRuleProcessService ProcessRound1()
        {
            if (MatchStaging != null && MatchStaging.MatchGroupStagingList.IsNotNullOrEmpty())
            {
                var matchProcessesRound1 =
                    MatchStaging.MatchGroupStagingList
                        .Select(matchInfo => ProcessWithAssociationRound1(matchInfo, IsLive))
                        .Where(matchInfo => matchInfo != null).ToList();

                MatchProcesses.AddRange(matchProcessesRound1);
            }

            return this;
        }

        public IMatchMonitorRuleProcessService ProcessRound2()
        {
            if (MatchProcesses.IsNotNullOrEmpty())
            {
                var matchProcessesRound2 =
                    MatchProcesses
                        .SelectMany(matchProcessRound1 =>
                            matchProcessRound1.ReprocessMatches.Select(
                                reprocessInfoRound2 => ProcessWithAssociationRound2(matchProcessRound1.MatchInfo, reprocessInfoRound2, IsLive))
                        )
                        .Where(matchInfo => matchInfo != null).ToList();

                MatchProcesses.AddRange(matchProcessesRound2);
            }

            return this;
        }

        private MatchProcessRuleGroupBettingModel ProcessWithAssociationRound1(MatchStagingRuleGroupBettingEntity matchInfo, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(matchInfo.CTSCustIDList, matchInfo.CustIDList, matchInfo.LicCustList, matchInfo.SportType)?
                                            .Where(n => n.GroupID.HasValue);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                return ProcessRuleGroupBetting(isLive, matchInfo, matchInfo.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
            }

            return null;
        }

        private MatchProcessRuleGroupBettingModel ProcessWithAssociationRound2(MatchStagingRuleGroupBettingEntity matchInfo, MatchProcessRuleGroupBettingInfoEntity reprocessModel, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(reprocessModel.CTSCustIDList, reprocessModel.CustIDList, reprocessModel.LicCustList, matchInfo.SportType)?
                                            .Where(n => n.GroupID.HasValue);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                var matchProcessRound2 = ProcessRuleGroupBetting(isLive, matchInfo, reprocessModel.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
                matchProcessRound2.RoundNumber = 2;

                return matchProcessRound2;
            }

            return null;
        }

        private IEnumerable<DetectAssociationMergeGroupLv1Entity> DetectAssociation(string ctsCustIDList, string custIDList, string licCustIDList, int sportType)
        {
            var param = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDList,
                CustIds = custIDList,
                IsDevice = true,
                IsAI = true,
                IsIP = IsValidSportTypeGetAssociation(sportType, AssociationDetectionType.IP),
                Is3MatchesLast7Days = IsValidSportTypeGetAssociation(sportType, AssociationDetectionType.MatchesLast7Days),
                IsIPLast3Days = IsValidSportTypeGetAssociation(sportType, AssociationDetectionType.IPLast3Days),
                CustIdsIPLast3Days = licCustIDList
            };

            return _associationDetection.DetectAssociationLv1(param);
        }

        private bool IsValidSportTypeGetAssociation(int sportType, AssociationDetectionType assType)
        {
            switch (assType)
            {
                case AssociationDetectionType.IP:
                case AssociationDetectionType.MatchesLast7Days:
                    return sportType == (int)MatchMonitor_SportType.Basketball
                          || sportType == (int)MatchMonitor_SportType.Esport
                          || sportType == (int)MatchMonitor_SportType.Cricket;

                case AssociationDetectionType.IPLast3Days:
                    return sportType == (int)MatchMonitor_SportType.Basketball
                          || sportType == (int)MatchMonitor_SportType.Esport;
                default:
                    return false;
            }
        }

        private MatchProcessRuleGroupBettingModel ProcessRuleGroupBetting(bool isLive, MatchStagingRuleGroupBettingEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            return _dataAccess.ProcessRuleGroupBetting(isLive, matchinfo, sequenceIDList, groupDetection);
        }
        #endregion

        public IMatchMonitorRuleProcessService Complete(ulong maxSequenceID)
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
                var para = new MatchCompleteRuleGroupBettingParameterModel
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

        public void WriteLogSentryIfError(string functionName, ISentryClient sentryClient)
        {
            if (MatchProcesses.IsNotNullOrEmpty())
            {
                var matchProcessNotValid = MatchProcesses.Where(x => x.RoundNumber == 2 && x.ReprocessMatches.IsNotNullOrEmpty());

                if (matchProcessNotValid.IsNotNullOrEmpty())
                {
                    Utilities.LogSentryInfo(ServiceName, $"{functionName} Error", matchProcessNotValid, true, sentryClient);
                }
            }
        }
    }
}
