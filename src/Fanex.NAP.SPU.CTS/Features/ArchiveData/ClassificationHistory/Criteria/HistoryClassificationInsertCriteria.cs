namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class HistoryClassificationInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_HistoryJson")]
        public string HistoryDataJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_Archive_CustClassification_History_Insert";
        }

        public override bool IsValid()
            => true;
    }
}