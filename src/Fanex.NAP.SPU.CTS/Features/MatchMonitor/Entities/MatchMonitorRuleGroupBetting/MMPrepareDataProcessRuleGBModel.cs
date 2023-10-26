namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleGroupBetting
{
    public class MMPrepareDataProcessRuleGBModel
    {
        public bool IsLive { get; set; }

        public ulong MaxSequenceID { get; set; }

        public string JsonSharedMatches { get; set; }

        public MatchStagingRuleGroupBettingModel MatchInfo { get; set; }
    }
}