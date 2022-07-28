namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleGetCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_NoOfRow")]
        public int RowSize { get; set; }

        [SpParam(SqlParamName = "op_MaxTransID")]
        public UInt64 MaxTransId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_MatchMonitor_Rule_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}