namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;    

    public class ProblemCaseMonitorClassificationInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_ClassificationHistoryID")]
        public ulong ClassificationHistoryID { get; set; }

        [SpParam(SqlParamName = "ip_ProblemCaseData")]
        public string ProblemCaseJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_ProblemCaseMonitor_Classification_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
