namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;

    public class MatchStagingRuleArbitrageGroupModel
    {
        public int MatchId { get; set; }

        public bool IsMajorLeague { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public decimal HDP { get; set; }

        public IEnumerable<MatchStagingRuleArbitrageEntity> MatchGroupStagingList { get; set; }
    }
}