namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleHedging;
    using System;

    public class MatchStagingRuleHedgingModel : MatchProcessRuleHedgingReprocessModel
    {
        public int MatchId { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public string BetTeam { get; set; }

        public int SportType { get; set; }

        public int OldGroup { get; set; }
    }
}