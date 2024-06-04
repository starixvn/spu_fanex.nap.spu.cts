namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleSabaGBGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "op_MaxSequenceID")]
        public UInt64? MaxSequenceID { get; set; }


        public override string GetSettingKey()
        {
            throw new System.NotImplementedException(); // modify
        }

        public override bool IsValid() => true;
    }
}
