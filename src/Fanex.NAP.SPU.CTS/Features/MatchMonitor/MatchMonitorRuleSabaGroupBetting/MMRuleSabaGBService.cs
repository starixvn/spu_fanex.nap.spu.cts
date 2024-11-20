
namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting
{
    using System;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models;
    using Fanex.NAP.SPU.CTS.Shares;

    public interface IMMRuleSabaGBService
    {
        MatchStagingRuleSabaGBProcessModel GetMatchRuleSabaGB(bool isLive);

        IMatchMonitorRuleProcessService<MatchStagingRuleSabaGBGroupModel> GetInstanceProcessSabaGBService();

        void CleanTransRuleSabaGB(bool isLive, UInt64 maxSequenceID);
    }
    public class MMRuleSabaGBService : IMMRuleSabaGBService
    {
        private readonly IMMRuleSabaGBDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;

        public MMRuleSabaGBService(IMMRuleSabaGBDataAccess dataAccess, IServiceProvider serviceProvider)
        {
            _dataAccess = dataAccess;
            _serviceProvider = serviceProvider;
        }

        public MatchStagingRuleSabaGBProcessModel GetMatchRuleSabaGB(bool isLive)
        {
            var match = _dataAccess.GetMatchRuleSabaGB(isLive);

            var result = new MatchStagingRuleSabaGBProcessModel();

            if (match != null)
            {
                result.MaxSequenceID = match?.MaxSequenceID;

                if (match.MatchStagingList.IsNotNullOrEmpty())
                {
                    result.MappingCustDictionary = match?.CTSCustomerList
                                                    .Select(x => new { x.CustID, x.CTSCustID }).Distinct()
                                                    .ToDictionary(x => x.CustID, x => x.CTSCustID);

                    result.MatchStagingList = match.MatchStagingList.GroupBy(x => new { x.MatchId, x.ScoreDiff, x.BetTypeID, x.BetID, x.BetTeam })
                                                        .Select(y => new MatchStagingRuleSabaGBGroupModel
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

        public IMatchMonitorRuleProcessService<MatchStagingRuleSabaGBGroupModel> GetInstanceProcessSabaGBService()
        {
            return _serviceProvider.GetService<MMRuleSabaGBProcessService>();
        }

        public void CleanTransRuleSabaGB(bool isLive, UInt64 maxSequenceID)
        {
            _dataAccess.CleanTransRuleSabaGB(isLive, maxSequenceID);
        }
    }
}
