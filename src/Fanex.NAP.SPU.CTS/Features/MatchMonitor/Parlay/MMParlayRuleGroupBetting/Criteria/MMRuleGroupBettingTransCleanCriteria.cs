namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MMRuleGroupBettingTransCleanCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MaxSequenceID")]
        public UInt64 MaxSequenceID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_MatchMonitorParlay_RuleGroupBetting_StagingClean";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
