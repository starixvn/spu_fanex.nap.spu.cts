using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    class MatchMonitorRuleGetCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "op_MaxTransID")]
        public long MaxTransId { get; set; }

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
