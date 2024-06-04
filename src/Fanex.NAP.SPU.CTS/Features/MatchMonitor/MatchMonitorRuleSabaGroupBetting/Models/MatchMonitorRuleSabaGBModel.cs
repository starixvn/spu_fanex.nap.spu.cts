namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities;
    using System;
    using System.Collections.Generic;

    public class MatchMonitorRuleSabaGBModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleSabaGBEntity> MatchStagingList { get; set; }
    }
}
