namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using Newtonsoft.Json;

    public class MatchRuleIrrigationCompletedModel
    {
        [JsonProperty("CustID")]
        public int CustID { get; set; }

        [JsonProperty("TransIDList")]
        public string TransIDList { get; set; }

        [JsonProperty("OldGroupID")]
        public string OldGroupID { get; set; }
    }
}