namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);
        MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive);

        MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive);

        IEnumerable<MatchCompleteRuleGroupBettingModel> ProcessRuleGroupBetting(MMPrepareDataProcessRuleGBModel prepareData);

        IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxTransId, MatchStagingRuleFixedGameModel matchinfo);

        IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo);

        IEnumerable<MatchCompleteRuleArbitrageModel> ProcessRuleArbitrage(bool isLive, UInt64 maxTransId, MatchStagingRuleArbitrageModel matchinfo);

        void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para);

        void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para);

        void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para);

        void CompleteTransRuleArbitrage(MatchCompleteRuleArbitrageParameterModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID);

        #region Rule Hedging

        MatchStagingRuleHedgingProcessModel GetMatchRuleHedging(bool isLive);

        void ProcessRuleHedgingParallel(MatchStagingRuleHedgingGroupModel matchStaging, UInt64 maxSequenceID, int numberOfThread, bool isLive);

        void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID);

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
            matchDataAccess.InsertMatchMonitorStaging(matchTransJson, isLive);
        }

        public MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive)
        {
            return matchDataAccess.GetMatchRuleGroupBetting(isLive);
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

        public IEnumerable<MatchCompleteRuleGroupBettingModel> ProcessRuleGroupBetting(MMPrepareDataProcessRuleGBModel prepareData)
        {
            return matchDataAccess.ProcessRuleGroupBetting(prepareData);
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

        public void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para)
        {
            matchDataAccess.CompleteTransRuleGroupBetting(para);
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

        public void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleGroupBetting(isLive, maxSequenceID);
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

        #region Rule Hedging

        public MatchStagingRuleHedgingProcessModel GetMatchRuleHedging(bool isLive)
        {
            var result = matchDataAccess.GetMatchRuleHedging(isLive);

            if (result != null && result.MatchStagingList != null && result.MatchStagingList.Any())
            {
                var finalResult = result.MatchStagingList.GroupBy(x => new { x.MatchId, x.ScoreDiff, x.BetTypeID, x.BetID, x.BetTeam })
                                                         .Select(y => new MatchStagingRuleHedgingGroupModel()
                                                         {
                                                             MatchId = y.Key.MatchId,
                                                             ScoreDiff = y.Key.ScoreDiff,
                                                             BetTypeID = y.Key.BetTypeID,
                                                             BetID = y.Key.BetID,
                                                             BetTeam = y.Key.BetTeam,
                                                             MatchStagingList = y.ToList()
                                                         });

                return new MatchStagingRuleHedgingProcessModel { MaxSequenceID = result.MaxSequenceID, MatchStagingList = finalResult };
            }

            return new MatchStagingRuleHedgingProcessModel();
        }

        public void ProcessRuleHedgingParallel(MatchStagingRuleHedgingGroupModel matchStaging, UInt64 maxSequenceID, int numberOfThread, bool isLive)
        {
            if (matchStaging != null && matchStaging.MatchStagingList.Any())
            {
                matchStaging
                       .MatchStagingList
                       .AsParallel()
                       .WithDegreeOfParallelism(numberOfThread)
                       .ForAll(matchInfo =>
                       {
                           var ctsCustIds = matchInfo.CustomerInfo.Select(c => c.CTSCustID).Distinct();

                           var assDetection = associationDetectionService.DetectAssociationRuleHedging(ctsCustIds, matchInfo.DetectAgentAssCustIDList);

                           var assDetectionToJson = assDetection.Any() ? JsonConvert.SerializeObject(assDetection) : "[]";

                           var matchRuleCompleted = ProcessRuleHedging(isLive, matchInfo, assDetectionToJson);

                           if (matchRuleCompleted != null && matchRuleCompleted.Any())
                           {
                               CompleteTransRuleHedging(new MatchCompleteRuleHedgingParameterModel
                               {
                                   IsLive = isLive,
                                   MatchInfo = matchInfo,
                                   TransGroupJson = JsonConvert.SerializeObject(matchRuleCompleted),
                                   MaxSequenceID = maxSequenceID
                               });
                           }

                           Thread.Sleep(Consts.MatchMonitor_SleepTimes);
                       });
            }
        }

        private IEnumerable<MatchCompleteRuleHedgingModel> ProcessRuleHedging(bool isLive, MatchStagingRuleHedgingModel matchinfo, string groupDetection)
        {
            return matchDataAccess.ProcessRuleHedging(isLive, matchinfo, groupDetection);
        }

        private void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para)
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