namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities
{
    using Newtonsoft.Json;

    public class DetectAssociationByPrimaryEntity
    {
        [JsonProperty(PropertyName = "O")]
        public long OrigID { get; set; }

        [JsonProperty(PropertyName = "D")]
        public long DestID { get; set; }

        [JsonProperty(PropertyName = "A")]
        public int AssociationType { get; set; }
    }
}
