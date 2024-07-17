namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models;
    using Fanex.NAP.SPU.CTS.Shares;

    public interface IMatchMonitorRuleArbitrageService
    {
        MatchStagingRuleArbitrageProcessModel GetMatchRuleArbitrage(bool isLive);

        IMatchMonitorRuleProcessService<MatchStagingRuleArbitrageGroupModel> GetInstanceProcessArbitrageService();

        void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorRuleArbitrageService : IMatchMonitorRuleArbitrageService
    {
        private readonly IMatchMonitorRuleArbitrageDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;

        public MatchMonitorRuleArbitrageService(IMatchMonitorRuleArbitrageDataAccess dataAccess, IServiceProvider serviceProvider)
        {
            _dataAccess = dataAccess;
            _serviceProvider = serviceProvider;
        }

        public MatchStagingRuleArbitrageProcessModel GetMatchRuleArbitrage(bool isLive)
        {
            var matches = _dataAccess.GetMatchRuleArbitrage(isLive);

            var result = new MatchStagingRuleArbitrageProcessModel();

            if (matches != null)
            {
                result.MaxSequenceID = matches.MaxSequenceID;

                if (matches.MatchStagingList.IsNotNullOrEmpty())
                {
                    result.MatchStagingList = matches.MatchStagingList.GroupBy(x => new { x.MatchId, x.IsMajorLeague, x.ScoreDiff, x.BetTypeID, x.BetID, x.HDP })
                                                .Select(x => new MatchStagingRuleArbitrageGroupModel
                                                {
                                                    MatchId = x.Key.MatchId,
                                                    BetID = x.Key.BetID,
                                                    BetTypeID = x.Key.BetTypeID,
                                                    HDP = x.Key.HDP,
                                                    IsMajorLeague = x.Key.IsMajorLeague,
                                                    ScoreDiff = x.Key.ScoreDiff,
                                                    MatchGroupStagingList = x.ToList()
                                                });
                }
            }

            return result;
        }

        public IMatchMonitorRuleProcessService<MatchStagingRuleArbitrageGroupModel> GetInstanceProcessArbitrageService()
        {
            return _serviceProvider.GetService<MatchMonitorRuleArbitrageProcessService>();
        }

        public void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID)
        {
            _dataAccess.CleanTransRuleArbitrage(isLive, maxSequenceID);
        }
    }
}
