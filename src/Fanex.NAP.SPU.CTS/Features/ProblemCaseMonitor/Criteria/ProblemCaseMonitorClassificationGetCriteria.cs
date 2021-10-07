namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;    

    public class ProblemCaseMonitorClassificationGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_ClassificationHistoryID")]
        public long ClassificationHistoryID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_ProblemCaseMonitor_Classification_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
