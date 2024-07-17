namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities
{
    using System;

    public class MatchStagingRuleArbitrageEntity
    {
        public int MatchId { get; set; }

        public bool IsMajorLeague { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public decimal HDP { get; set; }

        public string CTSCustIDList { get; set; }

        public string AgentDetect_CTSCustIDList { get; set; }

        public string SequenceIDList { get; set; }
    }
}