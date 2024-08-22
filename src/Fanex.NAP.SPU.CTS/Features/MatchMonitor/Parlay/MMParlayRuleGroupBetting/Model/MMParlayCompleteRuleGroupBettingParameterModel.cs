namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using System;

    public class MMParlayCompleteRuleGroupBettingParameterModel
    {
        public bool IsLive { get; set; }

        public MMParlayStagingRuleGroupBettingGroupModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}
