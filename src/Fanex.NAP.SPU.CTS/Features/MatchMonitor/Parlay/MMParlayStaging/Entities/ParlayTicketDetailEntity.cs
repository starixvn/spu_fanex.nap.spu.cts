namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging
{
    using Newtonsoft.Json;
    using System;

    public class ParlayTicketDetailEntity
    {      
        public long SequenceID { get; set; }
      
        public long TransId { get; set; }
      
        public DateTime TransDate { get; set; }
       
        public long MatchId { get; set; }
       
        public long HomeId { get; set; }
    
        public long AwayId { get; set; }
        
        public long LeagueId { get; set; }
     
        public string LeagueName { get; set; }
       
        public DateTime EventDate { get; set; }
       
        public string EventStatus { get; set; }
      
        public DateTime KickOffTime { get; set; }
      
        public int BetType { get; set; }
      
        public long CustId { get; set; }
      
        public string BetTeam { get; set; }
        
        public Decimal Stake { get; set; }
        
        public Decimal Odds { get; set; }
      
        public Decimal Hdp1 { get; set; }
       
        public Decimal Hdp2 { get; set; }
       
        public int LiveHomeScore { get; set; }
    
        public int LiveAwayScore { get; set; }

        [JsonIgnore]
        public bool LiveIndicator { get; set; }

  
        public int BetId { get; set; }
     
        public int SportType { get; set; }
       
        public bool IsMajorLeague { get; set; }

        public long? LeagueGroupID { get; set; }
    }
}
