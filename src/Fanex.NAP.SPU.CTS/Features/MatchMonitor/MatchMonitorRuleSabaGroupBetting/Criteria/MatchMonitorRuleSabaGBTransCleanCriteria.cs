namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleSabaGBTransCleanCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MaxSequenceID")]
        public UInt64 MaxSequenceID { get; set; }

        public override string GetSettingKey() => "CTS_DC_MatchMonitor_RuleGroupBettingSaba_StagingClean";

        public override bool IsValid() => true;
    }
}
