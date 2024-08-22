namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using System;
    using System.Collections.Generic;

    public class MMParlayStagingRuleGroupBettingGroupModel
    {
        public int MatchID { get; set; }

        public int ScoreDiff { get; set; }

        public int BettypeID { get; set; }

        public Int64 BetID { get; set; }

        public string Betteam { get; set; }

        public IEnumerable<MMParlayStagingRuleGroupBettingEntity> MatchGroupStagingList { get; set; }
    }
}
