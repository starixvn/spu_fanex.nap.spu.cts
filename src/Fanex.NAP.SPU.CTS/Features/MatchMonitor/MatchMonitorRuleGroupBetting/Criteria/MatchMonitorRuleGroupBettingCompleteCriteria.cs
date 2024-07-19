namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleGroupBettingCompleteCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MaxSequenceID")]
        public UInt64 MaxSequenceID { get; set; }

        [SpParam(SqlParamName = "ip_MatchID")]
        public int MatchId { get; set; }

        [SpParam(SqlParamName = "ip_ScoreDiff")]
        public int ScoreDiff { get; set; }

        [SpParam(SqlParamName = "ip_BettypeID")]
        public int BettypeId { get; set; }

        [SpParam(SqlParamName = "ip_BetID")]
        public Int64 BetID { get; set; }

        [SpParam(SqlParamName = "ip_Betteam")]
        public string Betteam { get; set; }

        [SpParam(SqlParamName = "ip_TransGroup")]
        public string TransGroupJson { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitor_RuleGroupBetting_Complete";

        public override bool IsValid()
            => true;
    }
}