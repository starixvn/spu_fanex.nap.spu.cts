namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MMParlayRuleGroupBettingProcessCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MatchID")]
        public int MatchId { get; set; }

        [SpParam(SqlParamName = "ip_ScoreDiff")]
        public int ScoreDiff { get; set; }

        [SpParam(SqlParamName = "ip_BettypeID")]
        public int BettypeId { get; set; }

        [SpParam(SqlParamName = "ip_BetID")]
        public Int64 BetId { get; set; }

        [SpParam(SqlParamName = "ip_Betteam")]
        public string Betteam { get; set; }

        [SpParam(SqlParamName = "ip_SportType")]
        public int SportType { get; set; }

        [SpParam(SqlParamName = "ip_TransIDmList")]
        public string TransIDmList { get; set; }

        [SpParam(SqlParamName = "ip_CustGroup")]
        public string CustGroup { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitorParlay_RuleGroupBetting_Process";

        public override bool IsValid()
            => true;
    }
}
