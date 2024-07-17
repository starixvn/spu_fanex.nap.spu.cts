namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models
{
    using System;
    using System.Collections.Generic;

    public class MatchStagingRuleArbitrageProcessModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleArbitrageGroupModel> MatchStagingList { get; set; }
    }
}
