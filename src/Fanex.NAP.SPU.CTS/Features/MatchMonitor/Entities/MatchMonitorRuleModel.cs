using System.Collections.Generic;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchMonitorRuleModel
    {
        public long MaxTransId { get; set; }

        public IEnumerable<MatchStagingModel> MatchStagingList { get; set; }
    }
}
