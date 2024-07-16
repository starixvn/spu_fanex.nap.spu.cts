namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities;
    using System;
    using System.Collections.Generic;

    public class MatchMonitorRuleSabaGBModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleSabaGBEntity> MatchStagingList { get; set; }

        /// <summary>
        /// Using CustID and CTSCustID to mapping AssociationDetection
        /// </summary>
        public IEnumerable<MatchMonitorMappingCustEntity> CTSCustomerList { get; set; }
    }
}
