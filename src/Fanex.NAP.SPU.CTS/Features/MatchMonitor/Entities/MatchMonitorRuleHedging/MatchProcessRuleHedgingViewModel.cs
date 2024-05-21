namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleHedging
{
    using System.Collections.Generic;

    public class MatchProcessRuleHedgingViewModel
    {
        public MatchStagingRuleHedgingModel MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleHedgingModel> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleHedgingReprocessModel> ReprocessMatches { get; set; }

        public int PharseNumber { get; set; }
    }
}
