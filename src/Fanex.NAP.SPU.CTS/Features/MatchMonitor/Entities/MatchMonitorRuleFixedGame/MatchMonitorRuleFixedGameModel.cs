namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    using System.Collections.Generic;

    public class MatchMonitorRuleFixedGameModel
    {
        public UInt64 MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleFixedGameModel> MatchStagingList { get; set; }
    }
}