namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ProblemCaseMonitorAssociationGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_AssDeviceID")]
        public ulong AssDeviceID { get; set; }

        [SpParam(SqlParamName = "op_AssIPID")]
        public ulong AssIPID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_ProblemCaseMonitor_Association_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
