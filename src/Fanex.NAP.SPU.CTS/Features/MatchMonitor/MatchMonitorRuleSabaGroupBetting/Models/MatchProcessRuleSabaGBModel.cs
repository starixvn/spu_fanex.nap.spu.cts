namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models
{
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities;

    public class MatchProcessRuleSabaGBModel
    {
        public MatchStagingRuleSabaGBEntity MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleSabaGBEntity> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleSabaGBInfoEntity> ReprocessMatches { get; set; }

        public int RoundNumber { get; set; } = 1;
    }
}
