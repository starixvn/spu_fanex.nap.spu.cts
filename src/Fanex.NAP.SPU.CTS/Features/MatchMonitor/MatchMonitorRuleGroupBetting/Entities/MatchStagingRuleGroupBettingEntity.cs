namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities
{
    using System;

    public class MatchStagingRuleGroupBettingEntity
    {
        public int MatchID { get; set; }

        public int ScoreDiff { get; set; }

        public int BettypeID { get; set; }

        public Int64 BetID { get; set; }

        public string Betteam { get; set; }

        public int SportType { get; set; }

        public string CTSCustIDList { get; set; }

        public string CustIDList { get; set; }

        public string SequenceIDList { get; set; }

        public string LicCustList { get; set; }
    }
}