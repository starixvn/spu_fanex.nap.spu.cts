namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleSabaGB
{
    using System.Collections.Generic;

    public class MatchProcessRuleSabaGBModel
    {
        public MatchStagingRuleSabaGBEntity MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleSabaGBEntity> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleSabaGBInfoEntity> ReprocessMatches { get; set; }

        public int RoundNumber { get; set; }
    }
}
