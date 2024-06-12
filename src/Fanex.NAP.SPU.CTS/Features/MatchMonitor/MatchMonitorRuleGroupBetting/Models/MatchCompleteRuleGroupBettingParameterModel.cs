namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models
{
    using System;

    public class MatchCompleteRuleGroupBettingParameterModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRuleGroupBettingGroupModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}