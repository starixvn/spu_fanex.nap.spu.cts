namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleSabaGB;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleHedging;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive);

        IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxTransId, MatchStagingRuleFixedGameModel matchinfo);

        IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo);

        IEnumerable<MatchCompleteRuleArbitrageModel> ProcessRuleArbitrage(bool isLive, UInt64 maxTransId, MatchStagingRuleArbitrageModel matchinfo);

        void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para);

        void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para);

        void CompleteTransRuleArbitrage(MatchCompleteRuleArbitrageParameterModel para);

        void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID);

        #region Rule Hedging

        MatchStagingRuleHedgingProcessModel GetMatchRuleHedging(bool isLive);

        IEnumerable<MatchProcessRuleHedgingModel> ProcessRuleHedgingWithRound(MatchStagingRuleHedgingGroupModel matchStaging, bool isLive, int numberInternalOfThread);

        void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para);

        void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID);

        #endregion

        #region Rule GroupBetting
        MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive);

        IEnumerable<MatchCompleteRuleGroupBettingModel> ProcessRuleGroupBetting(MMPrepareDataProcessRuleGBModel prepareData);

        void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);

        MatchStagingRuleSabaGBProcessModel GetMatchRuleSabaGB(bool isLive);

        IEnumerable<MatchProcessRuleSabaGBModel> ProcessRuleSabGBWithRound(MatchStagingRuleSabaGBGroupModel matchStaging, bool isLive);

        void CompleteTransRuleSabaGB(MatchCompleteRuleSabaGBParameterModel para);

        void CleanTransRuleSabaGB(bool isLive, UInt64 maxSequenceID);
        #endregion
    }

    public class MatchMonitorServices : IMatchMonitorServices
    {
        private readonly IMatchMonitorDataAccess matchDataAccess;
        private readonly IAssociationDetectionService associationDetectionService;

        public MatchMonitorServices(IMatchMonitorDataAccess matchDataAccess,
                                    IAssociationDetectionService associationDetectionService)
        {
            this.matchDataAccess = matchDataAccess;
            this.associationDetectionService = associationDetectionService;
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos)
        {
            return matchDataAccess.GetMatchTrans(lastScannedSequenceID, batchSize, isLive, betTypeInfos);
        }

        public void InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            var arrInsertType = isLive ? typeof(MatchMonitorStagingLiveInsertType) : typeof(MatchMonitorStagingNonLiveInsertType);

            Enum.GetValues(arrInsertType).Cast<int>().AsParallel().ForAll(poolType =>
            {
                matchDataAccess.InsertMatchMonitorStaging(matchTransJson, isLive, poolType);
            });
        }

        public MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive)
        {
            return matchDataAccess.GetMatchRuleFixedGame(isLive);
        }

        public MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive)
        {
            return matchDataAccess.GetMatchMonitorRuleIrrigation(isLive);
        }

        public MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive)
        {
            return matchDataAccess.GetMatchRuleArbitrage(isLive);
        }

        public IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxSequenceID, MatchStagingRuleFixedGameModel matchinfo)
        {
            return matchDataAccess.ProcessRuleFixedGame(isLive, maxSequenceID, matchinfo);
        }

        public IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRuleIrrigation(isLive, maxSequenceID, matchinfo);
        }

        public IEnumerable<MatchCompleteRuleArbitrageModel> ProcessRuleArbitrage(bool isLive, UInt64 maxSequenceID, MatchStagingRuleArbitrageModel matchinfo)
        {
            return matchDataAccess.ProcessRuleArbitrage(isLive, maxSequenceID, matchinfo);
        }

        public void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para)
        {
            matchDataAccess.CompleteTransRuleFixedGame(para);
        }

        public void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para)
        {
            matchDataAccess.UpdateMatchMonitorRuleIrrigationComplete(para);
        }

        public void CompleteTransRuleArbitrage(MatchCompleteRuleArbitrageParameterModel para)
        {
            matchDataAccess.CompleteTransRuleArbitrage(para);
        }

        public void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleFixedGame(isLive, maxSequenceID);
        }

        public void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleIrrigation(isLive, maxSequenceID);
        }

        public void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleArbitrage(isLive, maxSequenceID);
        }

        #region Rule GroupBetting
        public MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive)
        {
            return matchDataAccess.GetMatchRuleGroupBetting(isLive);
        }

        public IEnumerable<MatchCompleteRuleGroupBettingModel> ProcessRuleGroupBetting(MMPrepareDataProcessRuleGBModel prepareData)
        {
            return matchDataAccess.ProcessRuleGroupBetting(prepareData);
        }

        public void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para)
        {
            matchDataAccess.CompleteTransRuleGroupBetting(para);
        }

        public void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleGroupBetting(isLive, maxSequenceID);
        }


        public MatchStagingRuleSabaGBProcessModel GetMatchRuleSabaGB(bool isLive)
        {
            var matches = matchDataAccess.GetMatchRuleSabaGB(isLive);

            var result = new MatchStagingRuleSabaGBProcessModel();

            if (matches != null)
            {
                result.MaxSequenceID = matches.MaxSequenceID;

                if (matches.MatchStagingList.IsNotNullOrEmpty())
                {
                    result.MatchStagingList = matches.MatchStagingList.GroupBy(x => new { x.MatchId, x.ScoreDiff, x.BetTypeID, x.BetID, x.BetTeam, x.LeagueGroupId })
                                                        .Select(y => new MatchStagingRuleSabaGBGroupModel()
                                                        {
                                                            MatchId = y.Key.MatchId,
                                                            ScoreDiff = y.Key.ScoreDiff,
                                                            BetTypeID = y.Key.BetTypeID,
                                                            BetID = y.Key.BetID,
                                                            BetTeam = y.Key.BetTeam,
                                                            LeagueGroupId = y.Key.LeagueGroupId,
                                                            MatchGroupStagingList = y.ToList()
                                                        });
                }
            }

            return result;
        }

        public IEnumerable<MatchProcessRuleSabaGBModel> ProcessRuleSabGBWithRound(MatchStagingRuleSabaGBGroupModel matchStaging, bool isLive)
        {
            if (matchStaging != null && matchStaging.MatchGroupStagingList.IsNotNullOrEmpty())
            {
                foreach (var matchInfo in matchStaging.MatchGroupStagingList)
                {
                    var mmRound1 = ProcessRuleSabaGBWithAssociation(matchInfo, isLive);

                    if (mmRound1 != null && mmRound1.CompleteMatches.IsNotNullOrEmpty())
                    {
                        mmRound1.RoundNumber = 1;
                        yield return mmRound1;
                    }

                    if (mmRound1 != null && mmRound1.ReprocessMatches.IsNotNullOrEmpty())
                    {
                        foreach (var mmRound2 in ProcessRuleSabaGBR2(mmRound1, isLive))
                        {
                            yield return mmRound2;
                        }
                    }
                }
            }
        }

        private IEnumerable<MatchProcessRuleSabaGBModel> ProcessRuleSabaGBR2(MatchProcessRuleSabaGBModel matchProcess, bool isLive)
        {
            if (matchProcess != null && matchProcess.ReprocessMatches.IsNotNullOrEmpty())
            {
                foreach (var matchPharse2 in matchProcess.ReprocessMatches)
                {
                    var mmRound2 = ProcessRuleSabaGBWithAssociation(matchProcess.MatchInfo, matchPharse2, isLive);
                    if (mmRound2 != null && mmRound2.CompleteMatches.IsNotNullOrEmpty())
                    {
                        mmRound2.RoundNumber = 2;
                        yield return mmRound2;
                    }
                }
            }
        }

        private MatchProcessRuleSabaGBModel ProcessRuleSabaGBWithAssociation(MatchStagingRuleSabaGBEntity matchInfo, bool isLive)
        {
            return ProcessRuleSabaGBWithAssociation(matchInfo, matchInfo, isLive);
        }

        private MatchProcessRuleSabaGBModel ProcessRuleSabaGBWithAssociation(MatchStagingRuleSabaGBEntity matchInfo, MatchProcessRuleSabaGBInfoEntity reprocessModel, bool isLive)
        {
            var assDetectionHasGroupID = associationDetectionService
                                .DetectAssociationRuleSabaGB(reprocessModel.CTSCustIDList)?
                                .Where(c => c.GroupID != null);

            if (assDetectionHasGroupID.IsNotNullOrEmpty())
            {
                return ProcessRuleSabaGB(isLive, matchInfo, reprocessModel.SequenceIDList, JsonConvert.SerializeObject(assDetectionHasGroupID));
            }    

            return null;
        }

        private MatchProcessRuleSabaGBModel ProcessRuleSabaGB(bool isLive, MatchStagingRuleSabaGBEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            return matchDataAccess.ProcessRuleSabaGB(isLive, matchinfo, sequenceIDList, groupDetection);
        }

        public void CompleteTransRuleSabaGB(MatchCompleteRuleSabaGBParameterModel para)
        {
            matchDataAccess.CompleteTransRuleSabaGB(para);
        }

        public void CleanTransRuleSabaGB(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleSabaGB(isLive, maxSequenceID);
        }
        #endregion

        #region Rule Hedging

        public MatchStagingRuleHedgingProcessModel GetMatchRuleHedging(bool isLive)
        {
            var matches = matchDataAccess.GetMatchRuleHedging(isLive);

            var result = new MatchStagingRuleHedgingProcessModel();

            if (matches != null)
            {
                result.MaxSequenceID = matches.MaxSequenceID;

                if (matches.MatchStagingList != null && matches.MatchStagingList.Any())
                {
                    var finalResult = matches.MatchStagingList.GroupBy(x => new { x.MatchId, x.ScoreDiff, x.BetTypeID, x.BetID, x.BetTeam })
                                                             .Select(y => new MatchStagingRuleHedgingGroupModel()
                                                             {
                                                                 MatchId = y.Key.MatchId,
                                                                 ScoreDiff = y.Key.ScoreDiff,
                                                                 BetTypeID = y.Key.BetTypeID,
                                                                 BetID = y.Key.BetID,
                                                                 BetTeam = y.Key.BetTeam,
                                                                 MatchGroupStagingList = y.ToList()
                                                             });

                    result.MatchStagingList = finalResult;
                }
            }

            return result;
        }

        public IEnumerable<MatchProcessRuleHedgingModel> ProcessRuleHedgingWithRound(MatchStagingRuleHedgingGroupModel matchStaging, bool isLive, int numberInternalOfThread)
        {
            if (matchStaging != null && matchStaging.MatchGroupStagingList.IsNotNullOrEmpty())
            {
                foreach (var matchInfoR1 in matchStaging.MatchGroupStagingList.AsParallel().WithDegreeOfParallelism(numberInternalOfThread))
                {
                    var mmRound1 = ProcessRuleHedgingWithAssociation(matchInfoR1, isLive);

                    if (mmRound1 != null && mmRound1.CompleteMatches.IsNotNullOrEmpty())
                    {
                        mmRound1.RoundNumber = 1;
                        yield return mmRound1;
                    }

                    if (mmRound1 != null && mmRound1.ReprocessMatches.IsNotNullOrEmpty())
                    {
                        foreach (var mmRound2 in ProcessRuleHedgingRound2(mmRound1, isLive))
                        {
                            yield return mmRound2;
                        }
                    }
                }
            }
        }

        private IEnumerable<MatchProcessRuleHedgingModel> ProcessRuleHedgingRound2(MatchProcessRuleHedgingModel mmRound1, bool isLive)
        {
            if (mmRound1 != null && mmRound1.ReprocessMatches.IsNotNullOrEmpty())
            {
                foreach (var matchInfoR2 in mmRound1.ReprocessMatches)
                {
                    var mmRound2 = ProcessRuleHedgingWithAssociation(mmRound1.MatchInfo, matchInfoR2, isLive);
                    if (mmRound2 != null && mmRound2.CompleteMatches.IsNotNullOrEmpty())
                    {
                        mmRound2.RoundNumber = 2;
                        yield return mmRound2;
                    }
                }
            }
        }

        private MatchProcessRuleHedgingModel ProcessRuleHedgingWithAssociation(MatchStagingRuleHedgingEntity matchInfo, bool isLive)
        {
            return ProcessRuleHedgingWithAssociation(matchInfo, matchInfo, isLive);
        }

        private MatchProcessRuleHedgingModel ProcessRuleHedgingWithAssociation(MatchStagingRuleHedgingEntity matchInfo, MatchProcessRuleHedgingInfoEntity reprocessModel, bool isLive)
        {
            var assDetectionHasGroupID = associationDetectionService
                                .DetectAssociationRuleHedging(reprocessModel.CTSCustIDList, reprocessModel.AgentDetect_CTSCustIDList)?
                                .Where(c => c.GroupID != null);

            var assDetectionToJson = assDetectionHasGroupID.IsNotNullOrEmpty() ? JsonConvert.SerializeObject(assDetectionHasGroupID) : string.Empty;

            return ProcessRuleHedging(isLive, matchInfo, reprocessModel.SequenceIDList, assDetectionToJson);
        }

        private MatchProcessRuleHedgingModel ProcessRuleHedging(bool isLive, MatchStagingRuleHedgingEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            if (string.IsNullOrEmpty(groupDetection)) return null;

            return matchDataAccess.ProcessRuleHedging(isLive, matchinfo, sequenceIDList, groupDetection);
        }

        public void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para)
        {
            matchDataAccess.CompleteTransRuleHedging(para);
        }

        public void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleHedging(isLive, maxSequenceID);
        }

        #endregion
    }
}