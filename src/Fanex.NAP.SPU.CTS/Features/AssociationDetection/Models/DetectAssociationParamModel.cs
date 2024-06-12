namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models
{
    public class DetectAssociationParamModel
    {
        public string CTSCustIds { get; set; }

        public string AgentCTSCustIds { get; set; }

        public bool IsDevice { get; set; }

        public bool IsAI { get; set; }

        public bool IsIP { get; set; }

        public bool IsAgent { get; set; }

        public bool IsIPLast3Days { get; set; }

        public bool Is3MatchesLast7Days { get; set; }
    }
}
