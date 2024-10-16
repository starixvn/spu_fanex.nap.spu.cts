namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using System;
    using System.Collections.Generic;

    public class MMParlayStagingRuleGroupBettingProcessModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MMParlayStagingRuleGroupBettingGroupModel> MatchStagingList { get; set; }
    }
}
