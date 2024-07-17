namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models
{
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;

    public class MatchProcessRuleArbitrageModel
    {
        public MatchStagingRuleArbitrageEntity MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleArbitrageEntity> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleArbitrageInfoEntity> ReprocessMatches { get; set; }

        public int RoundNumber { get; set; } = 1;
    }
}
