namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class MatchTransUpdateCompleteCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_MatchID")]
        public int MatchId { get; set; }

        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_BettypeID")]
        public int BettypeId { get; set; }

        [SpParam(SqlParamName = "ip_TotalScore")]
        public int TotalScore { get; set; }

        [SpParam(SqlParamName = "ip_TransGroup")]
        public string TransGroupJson { get; set; }

        [SpParam(SqlParamName = "ip_MaxTransID")]
        public long MaxTransId { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitor_Rule_Complete";

        public override bool IsValid()
            => true;
    }
}