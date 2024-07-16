namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleSabaGBProcessCriteria : CriteriaBase
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

        [SpParam(SqlParamName = "ip_SequenceIDList")]
        public string SequenceIDList { get; set; }

        [SpParam(SqlParamName = "ip_CustGroup")]
        public string CustGroup { get; set; }

        public override string GetSettingKey() => "CTS_DC_MatchMonitor_RuleGroupBettingSaba_Process";

        public override bool IsValid() => true;
    }
}
