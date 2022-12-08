namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive);

        MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive);

        MatchMonitorRule3Model GetMatchMonitorRule3(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        IEnumerable<MatchMonitorRuleCompletedModel> ProcessRuleGroupBetting(bool isLive, UInt64 maxTransId, MatchStagingRuleGroupBettingModel matchinfo);

        IEnumerable<MatchRuleFixedGameCompletedModel> ProcessRuleFixedGame(bool isLive, UInt64 maxTransId, MatchStagingRuleFixedGameModel matchinfo);

        IEnumerable<MatchMonitorRuleCompletedModel> InsertMatchMonitorRule3(bool isLive, UInt64 maxTransId, MatchStagingRule3Model matchinfo);

        IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo);

        void CompleteTransRuleGroupBetting(RuleGroupBettingCompleteParameterModel para);

        void CompleteTransRuleFixedGame(RuleFixedGameCompleteParameterModel para);

        void UpdateMatchMonitorRule3Complete(MatchMonitorRule3CompleteParaModel para);

        void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID);

        void CleanTransRule3(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorServices : IMatchMonitorServices
    {
        private readonly IMatchMonitorDataAccess matchDataAccess;

        public MatchMonitorServices(IMatchMonitorDataAccess matchDataAccess)
        {
            this.matchDataAccess = matchDataAccess;
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive)
        {
            return matchDataAccess.GetMatchTrans(lastScannedSequenceID, batchSize, isLive);
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

        public MatchMonitorRule3Model GetMatchMonitorRule3(bool isLive)
        {
            return matchDataAccess.GetMatchMonitorRule3(isLive);
        }

        public MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive)
        {
            return matchDataAccess.GetMatchMonitorRuleIrrigation(isLive);
        }

        public IEnumerable<MatchMonitorRuleCompletedModel> ProcessRuleGroupBetting(bool isLive, UInt64 maxSequenceID, MatchStagingRuleGroupBettingModel matchinfo)
        {
            return matchDataAccess.ProcessRuleGroupBetting(isLive, maxSequenceID, matchinfo);
        }

        public IEnumerable<MatchRuleFixedGameCompletedModel> ProcessRuleFixedGame(bool isLive, UInt64 maxSequenceID, MatchStagingRuleFixedGameModel matchinfo)
        {
            return matchDataAccess.ProcessRuleFixedGame(isLive, maxSequenceID, matchinfo);
        }

        public IEnumerable<MatchMonitorRuleCompletedModel> InsertMatchMonitorRule3(bool isLive, UInt64 maxSequenceID, MatchStagingRule3Model matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRule3(isLive, maxSequenceID, matchinfo);
        }

        public IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRuleIrrigation(isLive, maxSequenceID, matchinfo);
        }

        public void CompleteTransRuleGroupBetting(RuleGroupBettingCompleteParameterModel para)
        {
            matchDataAccess.CompleteTransRuleGroupBetting(para);
        }

        public void CompleteTransRuleFixedGame(RuleFixedGameCompleteParameterModel para)
        {
            matchDataAccess.CompleteTransRuleFixedGame(para);
        }

        public void UpdateMatchMonitorRule3Complete(MatchMonitorRule3CompleteParaModel para)
        {
            matchDataAccess.UpdateMatchMonitorRule3Complete(para);
        }

        public void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para)
        {
            matchDataAccess.UpdateMatchMonitorRuleIrrigationComplete(para);
        }

        public void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleGroupBetting(isLive, maxSequenceID);
        }

        public void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleFixedGame(isLive, maxSequenceID);
        }

        public void CleanTransRule3(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRule3(isLive, maxSequenceID);
        }

        public void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleIrrigation(isLive, maxSequenceID);
        }
    }
}