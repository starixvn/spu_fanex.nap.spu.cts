namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleArbitrageProcessCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MatchID")]
        public int MatchId { get; set; }

        [SpParam(SqlParamName = "ip_IsMajorLeague")]
        public bool IsMajorLeague { get; set; }

        [SpParam(SqlParamName = "ip_ScoreDiff")]
        public int ScoreDiff { get; set; }

        [SpParam(SqlParamName = "ip_BettypeID")]
        public int BettypeId { get; set; }

        [SpParam(SqlParamName = "ip_BetID")]
        public Int64 BetId { get; set; }

        [SpParam(SqlParamName = "ip_HDP")]
        public decimal HDP { get; set; }

        [SpParam(SqlParamName = "ip_SequenceIDList")]
        public string SequenceIDList { get; set; }

        [SpParam(SqlParamName = "ip_CustGroup")]
        public string CustGroup { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitor_RuleArbitrage_Process";

        public override bool IsValid()
            => true;
    }
}