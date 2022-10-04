namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class HistoryClassificationBySportInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_HistoryJson")]
        public string HistoryDataJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_Archive_CustClassification_BySport_History_Insert";
        }

        public override bool IsValid()
            => true;
    }
}