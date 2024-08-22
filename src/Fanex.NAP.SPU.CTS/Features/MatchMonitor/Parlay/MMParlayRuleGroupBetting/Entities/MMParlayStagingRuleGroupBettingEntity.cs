namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using System;    

    public class MMParlayStagingRuleGroupBettingEntity
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
    }
}
