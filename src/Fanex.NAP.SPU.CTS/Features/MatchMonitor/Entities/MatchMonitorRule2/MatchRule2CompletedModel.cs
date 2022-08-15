using Newtonsoft.Json;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    public class MatchRule2CompletedModel
    {
        [JsonProperty("Reason")]
        public int Reason { get; set; }

        [JsonProperty("TransIDList")]
        public string TransIDList { get; set; }

        [JsonProperty("CustIDList")]
        public string CustIdList { get; set; }
    }
}