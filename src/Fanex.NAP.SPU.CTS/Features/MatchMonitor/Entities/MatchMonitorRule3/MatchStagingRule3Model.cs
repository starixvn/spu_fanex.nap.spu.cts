using System;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchStagingRule3Model
    {
        public int MatchId { get; set; }

        public int TotalScore { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }
        
        public string BetTeam { get; set; }
    }
}