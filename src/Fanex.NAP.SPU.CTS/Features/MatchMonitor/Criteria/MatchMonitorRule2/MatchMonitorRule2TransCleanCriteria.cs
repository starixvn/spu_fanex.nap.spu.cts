namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRule2TransCleanCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MaxTransID")]
        public UInt64 MaxTransId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_MatchMonitor_Staging_CleanTrans";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}