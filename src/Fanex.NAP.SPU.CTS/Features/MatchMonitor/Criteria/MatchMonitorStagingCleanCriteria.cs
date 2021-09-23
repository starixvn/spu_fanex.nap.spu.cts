using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    public class MatchMonitorStagingCleanCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_MatchMonitor_Staging_Clean";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
