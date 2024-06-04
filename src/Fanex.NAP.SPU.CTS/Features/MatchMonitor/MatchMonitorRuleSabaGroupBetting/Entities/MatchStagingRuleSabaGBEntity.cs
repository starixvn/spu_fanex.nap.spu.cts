namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities
{
    using System;

    public class MatchStagingRuleSabaGBEntity : MatchProcessRuleSabaGBInfoEntity
    {
        public int MatchId { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public string BetTeam { get; set; }

        public int SportType { get; set; }

        public int OldGroup { get; set; }

        public int LeagueGroupId { get; set; }
    }
}
