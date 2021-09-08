namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ChangedCustClassificationHistoryCleanUpCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_IsDataExist")]
        public int IsDataExist { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassification_History_DataChanged_CleanUp";
        }

        public override bool IsValid()
            => true;
    }
}