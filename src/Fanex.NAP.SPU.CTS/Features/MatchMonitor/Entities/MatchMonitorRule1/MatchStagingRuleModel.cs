using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchStagingRuleModel
    {
        public int MatchId { get; set; }

        public int TotalScore { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }
    }
}