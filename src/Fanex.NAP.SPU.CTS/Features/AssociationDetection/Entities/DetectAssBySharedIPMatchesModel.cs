namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities
{
    using Newtonsoft.Json;

    public class DetectAssBySharedIPMatchesModel
    {
        [JsonProperty("O")]
        public int CustID1 { get; set; }

        [JsonProperty("D")]
        public int CustID2 { get; set; }
    }
}
