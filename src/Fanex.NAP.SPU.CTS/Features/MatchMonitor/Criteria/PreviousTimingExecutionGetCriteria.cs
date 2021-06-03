namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class PreviousTimingExecutionGetCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_ParamID")]
        public int ParamID { get; set; }

        public override string GetSettingKey()
               => "CTS_DC_GetSystemParameterById";

        public override bool IsValid()
            => true;
    }
}