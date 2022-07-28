using System;
namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System.Collections.Generic;

    public class MatchMonitorRule2Model
    {
        public UInt64 MaxTransId { get; set; }

        public IEnumerable<MatchStagingRule2Model> MatchStagingList { get; set; }
    }
}
