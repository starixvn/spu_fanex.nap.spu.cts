namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models
{
    using System;

    public class MatchCompleteRuleArbitrageParameterModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRuleArbitrageGroupModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}