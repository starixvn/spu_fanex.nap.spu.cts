namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleSabaGB
{
    using System;
    using System.Collections.Generic;

    public class MatchStagingRuleSabaGBProcessModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleSabaGBGroupModel> MatchStagingList { get; set; }
    }
}
