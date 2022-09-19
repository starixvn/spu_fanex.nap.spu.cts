namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class HistoryClassificationArchiveCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_IsDataExist")]
        public int IsDataExist { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassification_History_Archive";
        }

        public override bool IsValid()
            => true;
    }
}