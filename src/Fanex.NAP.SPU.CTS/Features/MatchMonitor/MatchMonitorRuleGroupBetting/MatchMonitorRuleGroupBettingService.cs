namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting
{
    using System;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models;

    public interface IMatchMonitorRuleGroupBettingService
    {
        MatchStagingRuleGroupBettingProcessModel GetMatchRuleGroupBetting(bool isLive);

        IMatchMonitorRuleProcessService<MatchStagingRuleGroupBettingGroupModel> GetInstanceProcessGroupBettingService();

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorRuleGroupBettingService : IMatchMonitorRuleGroupBettingService
    {
        private readonly IMatchMonitorRuleGroupBettingDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;

        public MatchMonitorRuleGroupBettingService(IMatchMonitorRuleGroupBettingDataAccess dataAccess, IServiceProvider serviceProvider)
        {
            _dataAccess = dataAccess;
            _serviceProvider = serviceProvider;
        }

        public MatchStagingRuleGroupBettingProcessModel GetMatchRuleGroupBetting(bool isLive)
        {
            var match = _dataAccess.GetMatchRuleGroupBetting(isLive);

            var result = new MatchStagingRuleGroupBettingProcessModel();

            if (match != null)
            {
                result.MaxSequenceID = match.MaxSequenceID;

                if (match.MatchStagingList.IsNotNullOrEmpty())
                {
                    result.MappingCustDictionary = match?.CTSCustomerList
                                                    .Select(x => new { x.CustID, x.CTSCustID }).Distinct()
                                                    .ToDictionary(x => x.CustID, x => x.CTSCustID);

                    result.MatchStagingList = match.MatchStagingList.GroupBy(x => new { x.MatchID, x.ScoreDiff, x.BettypeID, x.BetID, x.Betteam })
                                                        .Select(y => new MatchStagingRuleGroupBettingGroupModel()
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

        public IMatchMonitorRuleProcessService<MatchStagingRuleGroupBettingGroupModel> GetInstanceProcessGroupBettingService()
        {
            return _serviceProvider.GetService<MatchMonitorRuleGroupBettingProcessService>();
        }

        public void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID)
        {
            _dataAccess.CleanTransRuleGroupBetting(isLive, maxSequenceID);
        }
    }
}
