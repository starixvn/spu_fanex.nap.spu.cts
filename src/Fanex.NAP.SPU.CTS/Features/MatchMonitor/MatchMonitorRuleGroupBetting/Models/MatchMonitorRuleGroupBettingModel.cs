namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities;

    public class MatchMonitorRuleGroupBettingModel
    {
        public UInt64? MaxSequenceID { get; set; }

        public IEnumerable<MatchStagingRuleGroupBettingEntity> MatchStagingList { get; set; }

        /// <summary>
        /// Mapping CustId to CTSCustId for AssociationDetection
        /// </summary>
        public IEnumerable<MatchMonitorMappingCustEntity> CTSCustomerList { get; set; }
    }
}