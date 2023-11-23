namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleGroupBetting;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive);

        MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive);

        MatchMonitorRuleHedgingModel GetMatchRuleHedging(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive);

        IEnumerable<MatchCompleteRuleGroupBettingModel> ProcessRuleGroupBetting(MMPrepareDataProcessRuleGBModel prepareData);

        IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxTransId, MatchStagingRuleFixedGameModel matchinfo);

        IEnumerable<MatchCompleteRuleHedgingModel> ProcessRuleHedging(bool isLive, UInt64 maxTransId, MatchStagingRuleHedgingModel matchinfo);

        IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo);

        IEnumerable<MatchCompleteRuleArbitrageModel> ProcessRuleArbitrage(bool isLive, UInt64 maxTransId, MatchStagingRuleArbitrageModel matchinfo);

        void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para);

        void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para);

        void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para);

        void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para);

        void CompleteTransRuleArbitrage(MatchCompleteRuleArbitrageParameterModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorServices : IMatchMonitorServices
    {
        private readonly IMatchMonitorDataAccess matchDataAccess;

        public MatchMonitorServices(IMatchMonitorDataAccess matchDataAccess)
        {
            this.matchDataAccess = matchDataAccess;
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

        public MatchMonitorRuleHedgingModel GetMatchRuleHedging(bool isLive)
        {
            return matchDataAccess.GetMatchRuleHedging(isLive);
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

        public IEnumerable<MatchCompleteRuleHedgingModel> ProcessRuleHedging(bool isLive, UInt64 maxSequenceID, MatchStagingRuleHedgingModel matchinfo)
        {
            return matchDataAccess.ProcessRuleHedging(isLive, maxSequenceID, matchinfo);
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

        public void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para)
        {
            matchDataAccess.CompleteTransRuleHedging(para);
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

        public void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleHedging(isLive, maxSequenceID);
        }

        public void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleIrrigation(isLive, maxSequenceID);
        }

        public void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleArbitrage(isLive, maxSequenceID);
        }
    }
}