namespace Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleHedgingTransCleanCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MaxSequenceID")]
        public UInt64 MaxSequenceID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_MatchMonitor_RuleHedging_StagingClean";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}