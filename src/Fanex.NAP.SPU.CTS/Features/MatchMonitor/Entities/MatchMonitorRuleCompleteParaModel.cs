using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchMonitorRuleCompleteParaModel
    {
        public bool IsLive { get; set; }

        public int MatchId { get; set; }

        public int TotalScore { get; set; }

        public int BettypeId { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxTransId { get; set; }
    }
}
