namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class PreviousTimingExecutionUpdateCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_ParamID")]
        public int ParamID { get; set; }

        [SpParam(SqlParamName = "ip_ParamValue")]
        public string ParamValue { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_UpdateSystemParameterById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}