namespace Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models
{
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Entities;

    public class MatchProcessRuleHedgingModel
    {
        public MatchStagingRuleHedgingEntity MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleHedgingEntity> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleHedgingInfoEntity> ReprocessMatches { get; set; }

        public int RoundNumber { get; set; }
    }
}
