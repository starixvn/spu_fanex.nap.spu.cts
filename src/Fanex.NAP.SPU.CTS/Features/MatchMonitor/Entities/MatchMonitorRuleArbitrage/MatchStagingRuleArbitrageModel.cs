namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    public class MatchStagingRuleArbitrageModel
    {
        public int MatchId { get; set; }

        public bool IsMajorLeague { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public decimal HDP { get; set; }
    }
}