namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    public class MatchMonitorRule3CompleteParaModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRule3Model MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
        
    }
}