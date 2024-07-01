namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage
{
    using System;
    using Sentry;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;

    public class MatchMonitorRuleArbitrageProcessService : IMatchMonitorRuleProcessService<MatchStagingRuleArbitrageGroupModel>
    {
        private readonly IAssociationDetectionService _assDetect;
        private readonly IMatchMonitorRuleArbitrageDataAccess _dataAccess;
        private const string ServiceName = "Match Monitor Process Rule Arbitrage Service";

        private MatchStagingRuleArbitrageGroupModel MatchStaging;
        private bool IsLive;

        private readonly List<MatchProcessRuleArbitrageModel> MatchProcesses;

        public MatchMonitorRuleArbitrageProcessService(IAssociationDetectionService assDetect, IMatchMonitorRuleArbitrageDataAccess dataAccess)
        {
            _assDetect = assDetect;
            _dataAccess = dataAccess;

            MatchProcesses = new List<MatchProcessRuleArbitrageModel>();
        }

        public IMatchMonitorRuleProcessService PrepareProcess(MatchStagingRuleArbitrageGroupModel matchStaging, bool isLive, IReadOnlyDictionary<int, int> mappingCustDictionary)
        {
            MatchStaging = matchStaging;
            IsLive = isLive;

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

        private MatchProcessRuleArbitrageModel ProcessWithAssociationRound1(MatchStagingRuleArbitrageEntity matchInfo, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(matchInfo.CTSCustIDList, matchInfo.AgentDetect_CTSCustIDList)?
                                            .Where(c => c.GroupID != null);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                return ProcessRuleArbitrage(isLive, matchInfo, matchInfo.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
            }

            return null;
        }

        private MatchProcessRuleArbitrageModel ProcessWithAssociationRound2(MatchStagingRuleArbitrageEntity matchInfo, MatchProcessRuleArbitrageInfoEntity reprocessModel, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(reprocessModel.CTSCustIDList, reprocessModel.AgentDetect_CTSCustIDList)?
                                            .Where(c => c.GroupID != null);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                var matchProcessRound2 = ProcessRuleArbitrage(isLive, matchInfo, reprocessModel.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
                matchProcessRound2.RoundNumber = 2;

                return matchProcessRound2;
            }

            return null;
        }

        private IEnumerable<DetectAssociationMergeGroupModel> DetectAssociation(string ctsCustIDList, string agentCTSCustIds)
        {
            var param = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDList,
                AgentCTSCustIds = agentCTSCustIds,
                IsDevice = true,
                IsAI = true,
                IsIP = true
            };

            return _assDetect.DetectAssociation(param);
        }

        private MatchProcessRuleArbitrageModel ProcessRuleArbitrage(bool isLive, MatchStagingRuleArbitrageEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            return _dataAccess.ProcessRuleArbitrage(isLive, matchinfo, sequenceIDList, groupDetection);
        }

        public IMatchMonitorRuleProcessService Complete(ulong maxSequenceID)
        {
            var mergeMatchComplete = MatchProcesses
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
                var para = new MatchCompleteRuleArbitrageParameterModel
                {
                    IsLive = IsLive,
                    MatchInfo = MatchStaging,
                    TransGroupJson = JsonConvert.SerializeObject(mergeMatchComplete),
                    MaxSequenceID = maxSequenceID
                };

                _dataAccess.CompleteTransRuleArbitrage(para);
            }

            return this;
        }

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
