namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities
{
    using Newtonsoft.Json;

    public class DetectAssociationMergeGroupLv1Entity
    {
        [JsonProperty("CTSCustID")]
        public int NodeID { get; set; }

        public int? GroupID { get; set; }
    }
}
