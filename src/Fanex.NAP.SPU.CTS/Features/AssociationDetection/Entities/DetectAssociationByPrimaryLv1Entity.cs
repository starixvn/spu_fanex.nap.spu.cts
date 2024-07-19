namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities
{
    using Newtonsoft.Json;

    public class DetectAssociationByPrimaryLv1Entity
    {
        [JsonProperty("O")]
        public long OrigCustID { get; set; }
        
        [JsonProperty("D")]
        public long DestCustID { get; set; }
    }
}
