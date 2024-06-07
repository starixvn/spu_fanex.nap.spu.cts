namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting
{
    using Sentry;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities;

    public class MMRuleSabaGBProcessService : IMatchMonitorRuleProcessService<MatchStagingRuleSabaGBGroupModel>
    {
        private readonly IMMRuleSabaGBDataAccess _dataAccess;
        private readonly IAssociationDetectionService _assDetectService;
        private const string ServiceName = "Match Monitor Process Rule Saba GroupBetting Service";

        private MatchStagingRuleSabaGBGroupModel MatchStaging;
        private bool IsLive;

        private List<MatchProcessRuleSabaGBModel> MatchProcesses;

        public MMRuleSabaGBProcessService(IAssociationDetectionService assDetectService, IMMRuleSabaGBDataAccess dataAccess)
        {
            _assDetectService = assDetectService;
            _dataAccess = dataAccess;
        }

        #region Process
        public IMatchMonitorRuleProcessService Start(MatchStagingRuleSabaGBGroupModel matchStaging, bool isLive)
        {
            MatchStaging = matchStaging;
            IsLive = isLive;

            MatchProcesses = new List<MatchProcessRuleSabaGBModel>();

            return this;
        }

        public IMatchMonitorRuleProcessService ProcessRound1()
        {
            if (MatchStaging != null && MatchStaging.MatchGroupStagingList.IsNotNullOrEmpty())
            {
                var matchProcessesRound1 =
                    MatchStaging.MatchGroupStagingList
                        .Select(matchInfo => ProcessWithAssociationRound1(matchInfo, IsLive))
                        .Where(matchInfo => matchInfo != null && matchInfo.CompleteMatches.IsNotNullOrEmpty());

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
                        .Where(matchInfo => matchInfo != null && matchInfo.CompleteMatches.IsNotNullOrEmpty());

                MatchProcesses.AddRange(matchProcessesRound2);
            }

            return this;
        }

        private MatchProcessRuleSabaGBModel ProcessWithAssociationRound1(MatchStagingRuleSabaGBEntity matchInfo, bool isLive)
        {
            var assDetectionHasGroupID = _assDetectService
                                            .DetectAssociationRuleSabaGB(matchInfo.CTSCustIDList)?
                                            .Where(c => c.GroupID != null);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                return ProcessRuleSabaGB(isLive, matchInfo, matchInfo.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
            }

            return null;
        }

        private MatchProcessRuleSabaGBModel ProcessWithAssociationRound2(MatchStagingRuleSabaGBEntity matchInfo, MatchProcessRuleSabaGBInfoEntity reprocessModel, bool isLive)
        {
            var assDetectionHasGroupID = _assDetectService
                                            .DetectAssociationRuleSabaGB(reprocessModel.CTSCustIDList)?
                                            .Where(c => c.GroupID != null);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                var matchProcessRound2 = ProcessRuleSabaGB(isLive, matchInfo, reprocessModel.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
                matchProcessRound2.RoundNumber = 2;

                return matchProcessRound2;
            }

            return null;
        }

        private MatchProcessRuleSabaGBModel ProcessRuleSabaGB(bool isLive, MatchStagingRuleSabaGBEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            return _dataAccess.ProcessRuleSabaGB(isLive, matchinfo, sequenceIDList, groupDetection);
        }
        #endregion

        #region Complete
        public IMatchMonitorRuleProcessService Complete(ulong maxSequenceID)
        {
            if (MatchProcesses.IsNotNullOrEmpty())
            {
                var mergeMatchComplete = MatchProcesses
                                            .SelectMany(x => x.CompleteMatches)
                                            .GroupBy(item => item.TransIDList)
                                            .Select((group, index) =>
                                            {
                                                var result = group.First();
                                                result.GroupId = index + 1;
                                                return result;
                                            });

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
        #endregion
    }
}
