namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using System;
    using System.Collections.Generic;

    public class MMParlayRuleGroupBettingModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MMParlayStagingRuleGroupBettingEntity> MatchStagingList { get; set; }


    }
}
