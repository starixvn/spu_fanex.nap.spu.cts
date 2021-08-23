namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class MatchTransUpdateCompleteCriteria : NonQueryCommand
    {

        [SpParam(SqlParamName = "ip_Result")]
        public string Result { get; set; }

        [SpParam(SqlParamName = "ip_IsLive")]
        public bool IsLive { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitor_Rule_Complete";

        public override bool IsValid()
            => true;
    }
}