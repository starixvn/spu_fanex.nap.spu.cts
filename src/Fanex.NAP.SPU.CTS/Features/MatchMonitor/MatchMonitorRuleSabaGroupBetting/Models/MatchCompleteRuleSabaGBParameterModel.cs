namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models
{
    using System;

    public class MatchCompleteRuleSabaGBParameterModel
    {
        public bool IsLive { get; set; }

        public MatchStagingRuleSabaGBGroupModel MatchInfo { get; set; }

        public string TransGroupJson { get; set; }

        public UInt64 MaxSequenceID { get; set; }
    }
}
