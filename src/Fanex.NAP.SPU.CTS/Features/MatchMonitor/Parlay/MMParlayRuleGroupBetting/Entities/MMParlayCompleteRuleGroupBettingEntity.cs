namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using Newtonsoft.Json;

    public class MMParlayCompleteRuleGroupBettingEntity
    {
        [JsonProperty("GroupID")]
        public int GroupId { get; set; }

        [JsonProperty("CustIDList")]
        public string CustIdList { get; set; }       

        [JsonProperty("TransIDList")]
        public string TransIDList { get; set; }

        [JsonProperty("RefnoList")]
        public string RefnoList { get; set; }

        [JsonProperty("TransIDmList")]
        public string TransIDmList { get; set; }
    }
}