namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class AffectedEvidenceFromQueueRemoveCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_IsDataExist")]
        public int IsDataExist { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustEvidenceQueue_Affected_Remove";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}