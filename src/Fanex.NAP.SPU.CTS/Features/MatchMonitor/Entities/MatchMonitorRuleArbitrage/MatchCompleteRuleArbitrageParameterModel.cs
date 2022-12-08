namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    public class MatchCompleteRuleArbitrageParameterModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRuleArbitrageModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}