namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleSabaGB
{
    using System;
    using System.Collections.Generic;

    public class MatchMonitorRuleSabaGBModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleSabaGBModel> MatchStagingList { get; set; }
    }
}
