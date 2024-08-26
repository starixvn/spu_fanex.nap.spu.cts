namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging
{   
    using System;

    public class ParlayTicketDetailEntity
    {      
        public long SequenceID { get; set; }
      
        public long TransID { get; set; }

        public long Refno { get; set; }

        public long TransIDm { get; set; }

        public DateTime TransDate { get; set; }

        public long MatchID { get; set; }

        public int SportType { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public int BetType { get; set; }

        public int BetID { get; set; }

        public int MyProperty { get; set; }

        public string BetTeam { get; set; }

        public long CustID { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime KickOffTime { get; set; }

        public string EventStatus { get; set; }

        public long HomeID { get; set; }

        public long AwayID { get; set; }

        public long LeagueID { get; set; }

        public string LeagueName { get; set; }
        
    }
}
