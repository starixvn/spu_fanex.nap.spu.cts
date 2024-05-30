namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria.MatchMonitorRuleSabaGB
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class MatchMonitorRuleSabaGBProcessCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_LiveIndicator")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "ip_MatchID")]
        public int MatchId { get; set; }

        [SpParam(SqlParamName = "ip_ScoreDiff")]
        public int ScoreDiff { get; set; }

        [SpParam(SqlParamName = "ip_BettypeID")]
        public int BettypeId { get; set; }

        [SpParam(SqlParamName = "ip_BetID")]
        public Int64 BetId { get; set; }

        [SpParam(SqlParamName = "ip_Betteam")]
        public string Betteam { get; set; }

        [SpParam(SqlParamName = "ip_SportType")]
        public int SportType { get; set; }

        [SpParam(SqlParamName = "ip_LeagueGroupId")]
        public int LeagueGroupId { get; set; }

        [SpParam(SqlParamName = "ip_SequenceIDList")]
        public string SequenceIDList { get; set; }

        [SpParam(SqlParamName = "ip_CustGroup")]
        public string CustGroup { get; set; }

        public override string GetSettingKey()
        {
            throw new System.NotImplementedException(); // modify
        }

        public override bool IsValid() => true;
    }
}
