namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;

    public class MatchMonitorRuleArbitrageModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleArbitrageEntity> MatchStagingList { get; set; }
    }
}