namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    using System.Collections.Generic;

    public class MatchStagingRuleHedgingProcessModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleHedgingGroupModel> MatchStagingList { get; set; }
    }
}