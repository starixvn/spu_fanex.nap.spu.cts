namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ParlayTicketDetailInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_TransList")]
        public string MatchTransJson { get; set; }

        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_PoolType")]
        public int PoolType { get; set; }

        public override string GetSettingKey()
              => "CTS_DC_MatchMonitorParlay_Staging_Insert";

        public override bool IsValid()
            => true;
    }
}
