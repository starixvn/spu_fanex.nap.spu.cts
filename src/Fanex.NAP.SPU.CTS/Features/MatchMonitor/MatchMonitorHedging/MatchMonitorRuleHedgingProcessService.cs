namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorHedging
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class MatchMonitorRuleHedgingProcessService : IMatchMonitorRuleProcessService<MatchStagingRuleHedgingGroupModel>
    {
        private readonly IMatchMonitorRuleHedgingDataAccess _dataAccess;
        private readonly IAssociationDetectionService _associationDetection;
        private const string ServiceName = "Match Monitor Process Rule Hedging Service";

        private MatchStagingRuleHedgingGroupModel MatchStaging;
        private bool IsLive;
        private int NumberInternalOfThread;

        private readonly List<MatchProcessRuleHedgingModel> MatchProcesses;

        public MatchMonitorRuleHedgingProcessService(IMatchMonitorRuleHedgingDataAccess dataAccess, IAssociationDetectionService associationDetection)
        {
            _dataAccess = dataAccess;
            _associationDetection = associationDetection;

            MatchProcesses = new List<MatchProcessRuleHedgingModel>();
        }

        public IMatchMonitorRuleProcessService PrepareProcessWithThread(MatchStagingRuleHedgingGroupModel matchStaging, bool isLive, int numberInternalOfThread)
        {
            MatchStaging = matchStaging;
            IsLive = isLive;
            NumberInternalOfThread = numberInternalOfThread;

            return this;
        }

        #region Process
        public IMatchMonitorRuleProcessService ProcessRound1()
        {
            if (MatchStaging != null && MatchStaging.MatchGroupStagingList.IsNotNullOrEmpty())
            {
                var matchProcessesRound1 =
                    MatchStaging.MatchGroupStagingList.AsParallel().WithDegreeOfParallelism(NumberInternalOfThread)
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
                    MatchProcesses.AsParallel().WithDegreeOfParallelism(NumberInternalOfThread)
                        .SelectMany(matchProcessRound1 =>
                            matchProcessRound1.ReprocessMatches.Select(
                                reprocessInfoRound2 => ProcessWithAssociationRound2(matchProcessRound1.MatchInfo, reprocessInfoRound2, IsLive))
                        )
                        .Where(matchInfo => matchInfo != null).ToList();

                MatchProcesses.AddRange(matchProcessesRound2);
            }

            return this;
        }

         private MatchProcessRuleHedgingModel ProcessWithAssociationRound1(MatchStagingRuleHedgingEntity matchInfo, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(matchInfo.CTSCustIDList, matchInfo.AgentDetect_CTSCustIDList)?
                                            .Where(n => n.GroupID.HasValue);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                return ProcessRuleHedging(isLive, matchInfo, matchInfo.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
            }

            return null;
        }

        private MatchProcessRuleHedgingModel ProcessWithAssociationRound2(MatchStagingRuleHedgingEntity matchInfo, MatchProcessRuleHedgingInfoEntity reprocessModel, bool isLive)
        {
            var assDetectionHasGroupID = DetectAssociation(reprocessModel.CTSCustIDList, reprocessModel.AgentDetect_CTSCustIDList)?
                                            .Where(n => n.GroupID.HasValue);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                var matchProcessRound2 = ProcessRuleHedging(isLive, matchInfo, reprocessModel.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
                matchProcessRound2.RoundNumber = 2;

                return matchProcessRound2;
            }

            return null;
        }

        private IEnumerable<DetectAssociationMergeGroupEntity> DetectAssociation(string ctsCustIDList, string agentCTSCustIds)
        {
            var param = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDList,
                AgentCTSCustIds = agentCTSCustIds,
                IsDevice = true,
                IsAI = true,
                IsIP = true,
            };

            return _associationDetection.DetectAssociation(param);
        }

        private MatchProcessRuleHedgingModel ProcessRuleHedging(bool isLive, MatchStagingRuleHedgingEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            return _dataAccess.ProcessRuleHedging(isLive, matchinfo, sequenceIDList, groupDetection);
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
                var para = new MatchCompleteRuleHedgingParameterModel
                {
                    IsLive = IsLive,
                    MatchInfo = MatchStaging,
                    MaxSequenceID = maxSequenceID,
                    TransGroupJson = JsonConvert.SerializeObject(mergeMatchComplete)
                };

                _dataAccess.CompleteTransRuleHedging(para);
            }

            return this;
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
