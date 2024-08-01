namespace Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models
{
    using System;

    public class MatchCompleteRuleHedgingParameterModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRuleHedgingGroupModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}