namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleIrrigationInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_SportType")]
        public int SportType { get; set; }

        [SpParam(SqlParamName = "ip_MatchID")]
        public int MatchID { get; set; }

        [SpParam(SqlParamName = "ip_ScoreDiff")]
        public int ScoreDiff { get; set; }

        [SpParam(SqlParamName = "ip_BettypeID")]
        public int BettypeID { get; set; }

        [SpParam(SqlParamName = "ip_BetID")]
        public Int64 BetID { get; set; }

        [SpParam(SqlParamName = "ip_Betteam")]
        public string Betteam { get; set; }

        [SpParam(SqlParamName = "ip_HDP")]
        public decimal HDP { get; set; }

        [SpParam(SqlParamName = "ip_MaxSequenceID")]
        public UInt64 MaxSequenceID { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitor_RuleIrrigation_Process";

        public override bool IsValid()
            => true;
    }
}
