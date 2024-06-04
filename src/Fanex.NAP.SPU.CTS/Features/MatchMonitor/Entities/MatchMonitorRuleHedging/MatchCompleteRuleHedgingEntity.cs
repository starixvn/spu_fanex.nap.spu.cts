namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using Newtonsoft.Json;

    public class MatchCompleteRuleHedgingEntity
    {
        [JsonProperty("GroupID")]
        public int GroupId { get; set; }

        [JsonProperty("IsHedging")]
        public bool IsHedging { get; set; }

        [JsonProperty("OldGroupIDList")]
        public string OldGroupIdList { get; set; }

        [JsonProperty("CustIDList")]
        public string CustIdList { get; set; }

        [JsonProperty("TransIDList")]
        public string TransIDList { get; set; }
    }
}