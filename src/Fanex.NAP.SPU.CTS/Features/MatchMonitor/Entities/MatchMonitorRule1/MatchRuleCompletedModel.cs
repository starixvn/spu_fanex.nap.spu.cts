using Newtonsoft.Json;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchRuleCompletedModel
    {
        [JsonProperty("GroupID")]
        public int GroupId { get; set; }

        [JsonProperty("Reason")]
        public int Reason { get; set; }

        [JsonProperty("TransIDList")]
        public string TransIDList { get; set; }

        [JsonProperty("CustIDList")]
        public string CustIdList { get; set; }

        [JsonProperty("OldGroupIDList")]
        public string OldGroupIdList { get; set; }
    }
}