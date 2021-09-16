namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class AffectedEvidenceFromQueueInsertCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_IsDataExist")]
        public int IsDataExist { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustEvidenceQueue_Affected_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}