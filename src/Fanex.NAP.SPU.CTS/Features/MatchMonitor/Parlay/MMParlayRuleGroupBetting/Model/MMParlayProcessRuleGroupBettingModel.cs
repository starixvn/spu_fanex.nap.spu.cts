namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using System.Collections.Generic;

    public class MMParlayProcessRuleGroupBettingModel
    {
        public MMParlayStagingRuleGroupBettingEntity MatchInfo { get; set; }

        public IEnumerable<MMParlayCompleteRuleGroupBettingEntity> CompleteMatches { get; set; }

        public IEnumerable<MMParlayProcessRuleGroupBettingInfoEntity> ReprocessMatches { get; set; }

        public int RoundNumber { get; set; } = 1;
    }
}
