namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleSabaGB
{
    using System.Collections.Generic;

    public class MatchProcessRuleSabaGBViewModel
    {
        public MatchStagingRuleSabaGBModel MatchInfo { get; set; }

        public IEnumerable<MatchCompleteRuleSabaGBModel> CompleteMatches { get; set; }

        public IEnumerable<MatchProcessRuleSabaGBReprocessModel> ReprocessMatches { get; set; }

        public int PharseNumber { get; set; }
    }
}
