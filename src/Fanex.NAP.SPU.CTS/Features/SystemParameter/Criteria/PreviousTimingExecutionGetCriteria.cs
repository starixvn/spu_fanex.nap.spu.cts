namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class PreviousTimingExecutionGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_ParamID")]
        public int ParamID { get; set; }

        public override string GetSettingKey()
               => "CTS_DC_GetSystemParameterById";

        public override bool IsValid()
            => true;
    }
}