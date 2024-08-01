namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorHedging
{
    using System;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models;
    using System.Linq;

    public interface IMatchMonitorRuleHedgingService
    {
        MatchStagingRuleHedgingProcessModel GetMatchRuleHedging(bool isLive);

        IMatchMonitorRuleProcessService<MatchStagingRuleHedgingGroupModel> GetInstanceProcessHedgingService();

        void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorRuleHedgingService : IMatchMonitorRuleHedgingService
    {
        private readonly IMatchMonitorRuleHedgingDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;

        public MatchMonitorRuleHedgingService(IMatchMonitorRuleHedgingDataAccess dataAccess, IServiceProvider serviceProvider)
        {
            _dataAccess = dataAccess;
            _serviceProvider = serviceProvider;
        }

        public MatchStagingRuleHedgingProcessModel GetMatchRuleHedging(bool isLive)
        {
            var match = _dataAccess.GetMatchRuleHedging(isLive);

            var result = new MatchStagingRuleHedgingProcessModel();

            if (match != null)
            {
                result.MaxSequenceID = match.MaxSequenceID;

                if (match.MatchStagingList.IsNotNullOrEmpty())
                {
                    result.MatchStagingList = match.MatchStagingList.GroupBy(x => new { x.MatchId, x.ScoreDiff, x.BetTypeID, x.BetID, x.BetTeam })
                                                        .Select(y => new MatchStagingRuleHedgingGroupModel()
                                                        {
                                                            MatchId = y.Key.MatchId,
                                                            ScoreDiff = y.Key.ScoreDiff,
                                                            BetTypeID = y.Key.BetTypeID,
                                                            BetID = y.Key.BetID,
                                                            BetTeam = y.Key.BetTeam,
                                                            MatchGroupStagingList = y.ToList()
                                                        });
                }
            }

            return result;
        }

        public IMatchMonitorRuleProcessService<MatchStagingRuleHedgingGroupModel> GetInstanceProcessHedgingService()
        {
            return _serviceProvider.GetService<MatchMonitorRuleHedgingProcessService>();
        }

        public void CleanTransRuleHedging(bool isLive, ulong maxSequenceID)
        {
            _dataAccess.CleanTransRuleHedging(isLive, maxSequenceID);
        }
    }
}
