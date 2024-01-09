namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MatchStagingRuleHedgingModel
    {
        public int MatchId { get; set; }

        public int ScoreDiff { get; set; }

        public int BetTypeID { get; set; }

        public Int64 BetID { get; set; }

        public string BetTeam { get; set; }

        public int SportType { get; set; }

        public int OldGroup { get; set; }

        public string CTSCustIDList { get; set; }

        public string AgentDetect_CTSCustIDList { get; set; }

        public string SequenceIDList { get; set; }
    }
}