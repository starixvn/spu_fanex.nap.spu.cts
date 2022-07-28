using Fanex.Data;
using Fanex.Data.Repository;
using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    class MatchMonitorRule2GetCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_NoOfRow")]
        public int RowSize { get; set; }

        [SpParam(SqlParamName = "op_MaxTransID")]
        public UInt64 MaxTransId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_MatchMonitor_Rule02_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}