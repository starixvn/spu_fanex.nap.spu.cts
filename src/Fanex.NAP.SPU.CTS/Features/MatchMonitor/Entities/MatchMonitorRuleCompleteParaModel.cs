using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchMonitorRuleCompleteParaModel
    {
        public bool IsLive { get; set; }

        public MatchStagingModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxTransId { get; set; }
    }
}
