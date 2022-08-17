using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System.Collections.Generic;

    public class MatchMonitorRuleModel
    {
        public UInt64 MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleModel> MatchStagingList { get; set; }
    }
}