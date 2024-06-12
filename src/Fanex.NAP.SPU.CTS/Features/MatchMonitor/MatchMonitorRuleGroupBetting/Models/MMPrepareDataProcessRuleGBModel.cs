namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities;

    public class MMPrepareDataProcessRuleGBModel
    {
        public bool IsLive { get; set; }

        public ulong MaxSequenceID { get; set; }

        public string JsonSharedMatches { get; set; }

        public MatchStagingRuleGroupBettingEntity MatchInfo { get; set; }
    }
}