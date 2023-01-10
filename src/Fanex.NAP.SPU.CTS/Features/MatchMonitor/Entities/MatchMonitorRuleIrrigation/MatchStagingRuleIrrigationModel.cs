namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    public class MatchStagingRuleIrrigationModel
    {
        public int MatchID { get; set; }

        public int ScoreDiff { get; set; }

        public int BettypeID { get; set; }

        public Int64 BetID { get; set; }

        public decimal HDP { get; set; }

        public string Betteam { get; set; }

        public int SportType { get; set; }
    }
}