namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class MatchMonitorRuleInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_TempGroupID")]
        public int TempGroupID { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitor_Rules";

        public override bool IsValid()
            => true;
    }
}