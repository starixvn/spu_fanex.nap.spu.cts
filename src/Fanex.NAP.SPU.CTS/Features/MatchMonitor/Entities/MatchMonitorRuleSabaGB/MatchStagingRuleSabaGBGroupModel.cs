namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleSabaGB
{
    using System;
    using System.Collections.Generic;

    public class MatchStagingRuleSabaGBGroupModel
    {
        public int MatchId { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public string BetTeam { get; set; }

        public int LeagueGroupId { get; set; }

        public IEnumerable<MatchStagingRuleSabaGBEntity> MatchGroupStagingList { get; set; }
    }
}
