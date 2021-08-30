namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UnChangedCustClassificationHistoryCleanUpCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "op_IsDataExist")]
        public bool IsDataExist { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassification_History_DataUnChanged_CleanUp";
        }

        public override bool IsValid()
            => true;
    }
}