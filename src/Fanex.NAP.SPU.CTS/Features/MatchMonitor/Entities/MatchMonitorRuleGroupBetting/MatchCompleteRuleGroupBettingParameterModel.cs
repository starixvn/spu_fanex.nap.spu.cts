namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    public class MatchCompleteRuleGroupBettingParameterModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRuleGroupBettingModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}