namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;

    public interface IMatchMonitorServices
    {
        MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxTransId, MatchStagingRuleFixedGameModel matchinfo);

        IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo);

        void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para);

        void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para);

        void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorServices : IMatchMonitorServices
    {
        private readonly IMatchMonitorDataAccess matchDataAccess;

        public MatchMonitorServices(IMatchMonitorDataAccess matchDataAccess)
        {
            this.matchDataAccess = matchDataAccess;
        }

        public MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive)
        {
            return matchDataAccess.GetMatchRuleFixedGame(isLive);
        }

        public MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive)
        {
            return matchDataAccess.GetMatchMonitorRuleIrrigation(isLive);
        }

        public IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxTransId, MatchStagingRuleFixedGameModel matchinfo)
        {
            return matchDataAccess.ProcessRuleFixedGame(isLive, maxTransId, matchinfo);
        }

        public IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRuleIrrigation(isLive, maxSequenceID, matchinfo);
        }

        public void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para)
        {
            matchDataAccess.CompleteTransRuleFixedGame(para);
        }

        public void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para)
        {
            matchDataAccess.UpdateMatchMonitorRuleIrrigationComplete(para);
        }

        public void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleFixedGame(isLive, maxSequenceID);
        }

        public void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRuleIrrigation(isLive, maxSequenceID);
        }
    }
}