namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleHedging
{
    using System.Collections.Generic;

    public class MatchProcessRuleHedgingModel
    {
        public MatchStagingRuleHedgingEntity MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleHedgingEntity> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleHedgingInfoEntity> ReprocessMatches { get; set; }

        public int RoundNumber { get; set; }
    }
}
