namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities
{
    using Newtonsoft.Json;

    public class MergeGroupAssociationFilterModel
    {
        public string CTSCustIDs { get; set; }

        public string AssociationByDeviceJson { get; set; } = "[]";

        public string AssociationByAIJson { get; set; } = "[]";

        public string AssociationByIPJson { get; set; } = "[]";

        public string AssociationByAgentJson { get; set; } = "[]";

        public string AssociationBySharedIPJson { get; set; } = "[]";

        public string AssociationBySharedMatchesJson { get; set; } = "[]";

        public bool IsGetLogTransInfo { get; set; }
    }
}
