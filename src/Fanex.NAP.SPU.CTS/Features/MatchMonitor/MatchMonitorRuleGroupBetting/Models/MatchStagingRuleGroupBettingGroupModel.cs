namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities;

    public class MatchStagingRuleGroupBettingGroupModel
    {
        public int MatchID { get; set; }

        public int ScoreDiff { get; set; }

        public int BettypeID { get; set; }

        public Int64 BetID { get; set; }

        public string Betteam { get; set; }

        public IEnumerable<MatchStagingRuleGroupBettingEntity> MatchGroupStagingList { get; set; }
    }
}
