namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ProblemCaseMonitorAssociationInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_ProblemCaseData")]
        public string ProblemCaseJson { get; set; }

        [SpParam(SqlParamName = "ip_AssDeviceID")]
        public ulong AssDeviceID { get; set; }

        [SpParam(SqlParamName = "ip_AssIPID")]
        public ulong AssIPID { get; set; }        

        public override string GetSettingKey()
        {
            return "CTS_DC_ProblemCaseMonitor_Association_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
