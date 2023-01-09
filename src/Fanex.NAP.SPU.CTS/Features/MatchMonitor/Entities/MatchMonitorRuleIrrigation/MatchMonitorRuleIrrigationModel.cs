namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;
    using System.Collections.Generic;

    public class MatchMonitorRuleIrrigationModel
    {
        public UInt64 MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleIrrigationModel> MatchStagingList { get; set; }
    }
}