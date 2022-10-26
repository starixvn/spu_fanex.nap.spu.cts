using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System.Collections.Generic;

    public class MatchMonitorRule3Model
    {
        public UInt64 MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRule3Model> MatchStagingList { get; set; }
    }
}