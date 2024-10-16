namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Linq;

    public interface IMMParlayRuleGroupBettingService
    {
        MMParlayStagingRuleGroupBettingProcessModel GetMMParlayRuleGroupBetting(bool isLive);

        IMMParlayRuleProcessService<MMParlayStagingRuleGroupBettingGroupModel> GetInstanceParlayProcessGBService();

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);
    }

    public class MMParlayRuleGroupBettingService : IMMParlayRuleGroupBettingService
    {
        private readonly IMMParlayRuleGroupBettingDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;

        public MMParlayRuleGroupBettingService(IMMParlayRuleGroupBettingDataAccess dataAccess, IServiceProvider serviceProvider)
        {
            _dataAccess = dataAccess;
            _serviceProvider = serviceProvider;
        }

        public MMParlayStagingRuleGroupBettingProcessModel GetMMParlayRuleGroupBetting(bool isLive)
        {
            var match = _dataAccess.GetMMParlayRuleGroupBettting(isLive);

            var result = new MMParlayStagingRuleGroupBettingProcessModel();

            if (match != null)
            {
                result.MaxSequenceID = match.MaxSequenceID;

                if (match.MatchStagingList.IsNotNullOrEmpty())
                {
                    result.MatchStagingList = match.MatchStagingList.GroupBy(x => new { x.MatchID, x.ScoreDiff, x.BettypeID, x.BetID, x.Betteam })
                                                        .Select(y => new MMParlayStagingRuleGroupBettingGroupModel()
                                                        {
                                                            MatchID = y.Key.MatchID,
                                                            ScoreDiff = y.Key.ScoreDiff,
                                                            BettypeID = y.Key.BettypeID,
                                                            BetID = y.Key.BetID,
                                                            Betteam = y.Key.Betteam,
                                                            MatchGroupStagingList = y.ToList()
                                                        });
                }
            }

            return result;
        }

        public IMMParlayRuleProcessService<MMParlayStagingRuleGroupBettingGroupModel> GetInstanceParlayProcessGBService()
        {
            return _serviceProvider.GetService<MMParlayRuleGroupBettingProcessService>();
        }

        public void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID)
        {
            _dataAccess.CleanTransRuleGroupBetting(isLive, maxSequenceID);
        }
    }
}
