namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    using System.Collections.Generic;

    public class MatchStagingRuleHedgingGroupModel
    {
        public int MatchId { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public string BetTeam { get; set; }

        public IEnumerable<MatchStagingRuleHedgingModel> MatchGroupStagingList { get; set; }
    }
}