namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;
    using System.Collections.Generic;

    public class MatchMonitorRuleGroupBettingModel
    {
        public UInt64 MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleGroupBettingModel> MatchStagingList { get; set; }
    }
}