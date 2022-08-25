using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchMonitorRule2CompleteParaModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRule2Model MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxTransId { get; set; }
    }
}