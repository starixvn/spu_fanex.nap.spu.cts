namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting
{
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;

    public class MMRuleSabaGBProcessService : IMatchMonitorRuleProcessService<MatchStagingRuleSabaGBGroupModel>
    {
        private readonly IMMRuleSabaGBDataAccess _dataAccess;
        private readonly IAssociationDetectionService _assDetectService;
        private const string ServiceName = "Match Monitor Process Rule Saba GroupBetting Service";

        private MatchStagingRuleSabaGBGroupModel MatchStaging;
        private bool IsLive;
        private IReadOnlyDictionary<int, int> MappingCustDictionary;

        private readonly List<MatchProcessRuleSabaGBModel> MatchProcesses;

        public MMRuleSabaGBProcessService(IAssociationDetectionService assDetectService, IMMRuleSabaGBDataAccess dataAccess)
        {
            _assDetectService = assDetectService;
            _dataAccess = dataAccess;

            MatchProcesses = new List<MatchProcessRuleSabaGBModel>();
        }

        #region Process
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "<Pending>")]
        public IMatchMonitorRuleProcessService PrepareProcess(MatchStagingRuleSabaGBGroupModel matchStaging, bool isLive, IReadOnlyDictionary<int, int> mappingCustDictionary)
        {
            MatchStaging = matchStaging;
            IsLive = isLive;
            MappingCustDictionary = mappingCustDictionary;

            return this;
        }

        public IMatchMonitorRuleProcessService ProcessRound1()
        {
            if (MatchStaging != null && MatchStaging.MatchGroupStagingList.IsNotNullOrEmpty())
            {
                var matchProcessesRound1 =
                    MatchStaging.MatchGroupStagingList
                        .Select(matchInfo => ProcessWithAssociationRound1(matchInfo, IsLive))
                        .Where(matchInfo => matchInfo != null)
                        .ToList();

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
                        .Where(matchInfo => matchInfo != null)
                        .ToList();

                MatchProcesses.AddRange(matchProcessesRound2);
            }

            return this;
        }

        private MatchProcessRuleSabaGBModel ProcessWithAssociationRound1(MatchStagingRuleSabaGBEntity matchInfo, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(matchInfo.CTSCustIDList, matchInfo.CustIDList)?
                                            .Where(c => c.GroupID != null);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                return ProcessRuleSabaGB(isLive, matchInfo, matchInfo.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
            }

            return null;
        }

        private MatchProcessRuleSabaGBModel ProcessWithAssociationRound2(MatchStagingRuleSabaGBEntity matchInfo, MatchProcessRuleSabaGBInfoEntity reprocessModel, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(reprocessModel.CTSCustIDList, reprocessModel.CustIDList)?
                                            .Where(c => c.GroupID != null);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                var matchProcessRound2 = ProcessRuleSabaGB(isLive, matchInfo, reprocessModel.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
                matchProcessRound2.RoundNumber = 2;

                return matchProcessRound2;
            }

            return null;
        }

        private IEnumerable<DetectAssociationMergeGroupEntity> DetectAssociation(string ctsCustIDList, string custIDList)
        {
            var param = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDList,
                CustIds = custIDList,
                IsDevice = true,
                IsAI = true,
                IsIP = true,
                Is3MatchesLast7Days = true,
                MappingCustDictionary = MappingCustDictionary
            };

            return _assDetectService.DetectAssociation(param);
        }

        private MatchProcessRuleSabaGBModel ProcessRuleSabaGB(bool isLive, MatchStagingRuleSabaGBEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            return _dataAccess.ProcessRuleSabaGB(isLive, matchinfo, sequenceIDList, groupDetection);
        }
        #endregion

        #region Complete
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
                var para = new MatchCompleteRuleSabaGBParameterModel
                {
                    IsLive = IsLive,
                    MatchInfo = MatchStaging,
                    TransGroupJson = JsonConvert.SerializeObject(mergeMatchComplete),
                    MaxSequenceID = maxSequenceID
                };

                _dataAccess.CompleteTransRuleSabaGB(para);
            }

            return this;
        }
        #endregion

        #region WriteLogSentryError
        /// <summary>
        /// Write log the case Process Round 2 not correct
        /// </summary>
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
        #endregion
    }
}
