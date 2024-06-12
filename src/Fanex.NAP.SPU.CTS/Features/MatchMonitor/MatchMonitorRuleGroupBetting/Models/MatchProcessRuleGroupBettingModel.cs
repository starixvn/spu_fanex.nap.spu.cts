namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models
{
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities;

    public class MatchProcessRuleGroupBettingModel
    {
        public MatchStagingRuleGroupBettingEntity MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleGroupBettingEntity> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleGroupBettingInfoEntity> ReprocessMatches { get; set; }

        public int RoundNumber { get; set; } = 1;
    }
}
