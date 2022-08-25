namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRule2CompleteCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_MatchID")]
        public int MatchId { get; set; }

        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MaxTransID")]
        public UInt64 MaxTransId { get; set; }

        [SpParam(SqlParamName = "ip_TotalScore")]
        public int TotalScore { get; set; }

        [SpParam(SqlParamName = "ip_BettypeID")]
        public int BettypeId { get; set; }

        [SpParam(SqlParamName = "ip_BetID")]
        public Int64 BetID { get; set; }

        [SpParam(SqlParamName = "ip_Betteam")]
        public string Betteam { get; set; }

        [SpParam(SqlParamName = "ip_TransGroup")]
        public string TransGroupJson { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_MatchMonitor_Rule02_Complete";

        public override bool IsValid()
            => true;
    }
}