namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class HistoryClassificationBySportArchiveCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "op_IsDataExist")]
        public int IsDataExist { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassification_BySport_History_Archive";
        }

        public override bool IsValid()
            => true;
    }
}