namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleArbitrageTransCleanCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MaxSequenceID")]
        public UInt64 MaxSequenceID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_MatchMonitor_RuleArbitrage_StagingClean";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}