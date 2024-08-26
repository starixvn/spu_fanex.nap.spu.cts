namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ParlayTicketDetailGetCriteria : NonQueryCommand
    {       
        [SpParam(SqlParamName = "LastScannedSequenceID")]
        public long LastScannedSequenceID { get; set; }

        [SpParam(SqlParamName = "BatchSize")]
        public int BatchSize { get; set; }

        [SpParam(SqlParamName = "ListSportBettype")]
        public string ListSportBettype { get; set; }

        [SpParam(SqlParamName = "IsLive")]
        public bool IsLive { get; set; }   

        public override string GetSettingKey()
              => "CTS_MatchMonitorParlay_GetNewTicket";

        public override bool IsValid()
            => true;
    }
}
