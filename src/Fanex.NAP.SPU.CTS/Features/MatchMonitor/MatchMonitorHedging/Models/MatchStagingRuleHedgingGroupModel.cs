namespace Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Entities;

    public class MatchStagingRuleHedgingGroupModel
    {
        public int MatchId { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public string BetTeam { get; set; }

        public IEnumerable<MatchStagingRuleHedgingEntity> MatchGroupStagingList { get; set; }
    }
}