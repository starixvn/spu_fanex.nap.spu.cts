namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    using System.Collections.Generic;

    public class MatchMonitorRuleHedgingModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleHedgingEntity> MatchStagingList { get; set; }
    }
}