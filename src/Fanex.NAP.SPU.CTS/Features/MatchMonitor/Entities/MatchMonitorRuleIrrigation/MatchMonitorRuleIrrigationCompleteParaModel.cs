namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    public class MatchMonitorRuleIrrigationCompleteParaModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRuleIrrigationModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}