namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class AffectedEvidenceFromQueueRemoveCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_BatchSize")]
        public int BatchSize { get; set; }

        [SpParam(SqlParamName = "op_IsExist")]
        public bool IsExist { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustEvidenceQueue_Affected_Remove";
        }

        public override bool IsValid()
        {
            return BatchSize > 0;
        }
    }
}