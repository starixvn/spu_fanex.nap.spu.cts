using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System.Collections.Generic;

    public class MatchMonitorRuleHedgingModel
    {
        public UInt64 MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleHedgingModel> MatchStagingList { get; set; }
    }
}