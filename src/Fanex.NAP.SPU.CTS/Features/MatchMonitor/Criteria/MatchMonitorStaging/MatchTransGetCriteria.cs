namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class MatchTransGetCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "QueryType")]
        public int QueryType { get; set; }

        [SpParam(SqlParamName = "LastScannedSequenceID")]
        public long LastScannedSequenceID { get; set; }

        [SpParam(SqlParamName = "BatchSize")]
        public int BatchSize { get; set; }

        [SpParam(SqlParamName = "ListSportBettype")]
        public string ListSportBettype { get; set; }

        public bool IsLive { get; set; }

        public override string GetSettingKey()
                => "CTS_Rpt_MatchMonitor_GetTicketInfo";

        public override bool IsValid()
            => true;
    }
}