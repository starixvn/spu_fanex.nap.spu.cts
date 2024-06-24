namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models
{
    using System;
    using System.Collections.Generic;

    public class MatchStagingRuleGroupBettingProcessModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleGroupBettingGroupModel> MatchStagingList { get; set; }
    }
}
