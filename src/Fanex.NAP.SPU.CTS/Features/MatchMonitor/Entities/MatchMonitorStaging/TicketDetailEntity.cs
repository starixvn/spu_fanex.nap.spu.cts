namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using Newtonsoft.Json;
    using System;

    public class TicketDetailEntity
    {
        [JsonProperty("SequenceID")]
        public long SequenceID { get; set; }

        [JsonProperty("TransID")]
        public Int64 TransId { get; set; }

        [JsonProperty("TransDate")]
        public DateTime TransDate { get; set; }

        [JsonProperty("MatchID")]
        public Int64 MatchId { get; set; }

        [JsonProperty("HomeID")]
        public Int64 HomeId { get; set; }

        [JsonProperty("AwayID")]
        public Int64 AwayId { get; set; }

        [JsonProperty("LeagueID")]
        public Int64 LeagueId { get; set; }

        [JsonProperty("LeagueName")]
        public string LeagueName { get; set; }

        [JsonProperty("EventDate")]
        public DateTime EventDate { get; set; }

        [JsonProperty("EventStatus")]
        public string EventStatus { get; set; }

        [JsonProperty("KickOffTime")]
        public DateTime KickOffTime { get; set; }

        [JsonProperty("Bettype")]
        public int BetType { get; set; }

        [JsonProperty("CustID")]
        public Int64 CustId { get; set; }

        [JsonProperty("Betteam")]
        public string BetTeam { get; set; }

        [JsonProperty("Stake")]
        public Decimal Stake { get; set; }

        [JsonProperty("Odds")]
        public Decimal Odds { get; set; }

        [JsonProperty("Hdp1")]
        public Decimal Hdp1 { get; set; }

        [JsonProperty("Hdp2")]
        public Decimal Hdp2 { get; set; }

        [JsonProperty("LiveHomeScore")]
        public int LiveHomeScore { get; set; }

        [JsonProperty("LiveAwayScore")]
        public int LiveAwayScore { get; set; }

        [JsonIgnore]
        public bool LiveIndicator { get; set; }

        [JsonProperty("BetID")]
        public int BetId { get; set; }

        [JsonProperty("SportType")]
        public int SportType { get; set; }

        [JsonProperty("IsMajorLeague")]
        public bool IsMajorLeague { get; set; }

        public long? LeagueGroupID { get; set; }
    }
}