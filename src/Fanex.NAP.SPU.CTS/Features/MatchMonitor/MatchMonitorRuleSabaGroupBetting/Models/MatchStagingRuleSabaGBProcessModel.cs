namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models
{
    using System;
    using System.Collections.Generic;

    public class MatchStagingRuleSabaGBProcessModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleSabaGBGroupModel> MatchStagingList { get; set; }

        public IReadOnlyDictionary<int, int> MappingCustDictionary { get; set; }
    }
}
