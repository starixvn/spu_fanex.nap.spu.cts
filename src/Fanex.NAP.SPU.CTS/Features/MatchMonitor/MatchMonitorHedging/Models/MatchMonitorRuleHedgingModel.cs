namespace Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models
{
    using System;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Entities;

    using System.Collections.Generic;

    public class MatchMonitorRuleHedgingModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleHedgingEntity> MatchStagingList { get; set; }
    }
}