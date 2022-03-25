using System;
namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System.Collections.Generic;

    public class MatchMonitorRuleModel
    {
        public UInt64 MaxTransId { get; set; }

        public IEnumerable<MatchStagingModel> MatchStagingList { get; set; }
    }
}
