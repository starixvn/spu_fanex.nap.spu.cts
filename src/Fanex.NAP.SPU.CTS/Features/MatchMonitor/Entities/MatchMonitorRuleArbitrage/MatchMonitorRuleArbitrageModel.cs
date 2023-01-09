namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    using System.Collections.Generic;

    public class MatchMonitorRuleArbitrageModel
    {
        public UInt64 MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleArbitrageModel> MatchStagingList { get; set; }
    }
}