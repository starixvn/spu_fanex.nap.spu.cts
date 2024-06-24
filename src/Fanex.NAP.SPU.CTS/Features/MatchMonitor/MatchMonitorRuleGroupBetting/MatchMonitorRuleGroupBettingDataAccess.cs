namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models;

    public interface IMatchMonitorRuleGroupBettingDataAccess
    {
        MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive);

        MatchProcessRuleGroupBettingModel ProcessRuleGroupBetting(bool isLive, MatchStagingRuleGroupBettingEntity matchInfo, string sequenceIDList, string groupDetection);

        void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);

    }

    public class MatchMonitorRuleGroupBettingDataAccess : IMatchMonitorRuleGroupBettingDataAccess
    {
        private readonly IDynamicRepository _repository;

        public MatchMonitorRuleGroupBettingDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive)
        {
            var criteria = new MatchMonitorRuleGroupBettingGetCriteria
            {
                IsLive = isLive
            };

            (var matchStagings, var ctsCustomers) = _repository.FetchMultiple<MatchStagingRuleGroupBettingEntity, MatchMonitorMappingCustEntity>(criteria);

            return new MatchMonitorRuleGroupBettingModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID.HasValue ? matchStagings : Enumerable.Empty<MatchStagingRuleGroupBettingEntity>(),
                CTSCustomerList = criteria.MaxSequenceID.HasValue ? ctsCustomers : Enumerable.Empty<MatchMonitorMappingCustEntity>()
            };
        }

        public MatchProcessRuleGroupBettingModel ProcessRuleGroupBetting(bool isLive, MatchStagingRuleGroupBettingEntity matchInfo, string sequenceIDList, string groupDetection)
        {
            var criteria = new MatchMonitorRuleGroupBettingProcessCriteria()
            {
                IsLive = isLive,
                BetId = matchInfo.BetID,
                Betteam = matchInfo.Betteam,
                BettypeId = matchInfo.BettypeID,
                ScoreDiff = matchInfo.ScoreDiff,
                SportType = matchInfo.SportType,
                MatchId = matchInfo.MatchID,
                SequenceIDList = sequenceIDList,
                CustGroup = groupDetection
            };

            (var completeMatches, var reprocessMatches) = _repository.FetchMultiple<MatchCompleteRuleGroupBettingEntity, MatchProcessRuleGroupBettingInfoEntity>(criteria);

            return new MatchProcessRuleGroupBettingModel
            {
                MatchInfo = matchInfo,
                CompleteMatches = completeMatches,
                ReprocessMatches = reprocessMatches
            };
        }

        public void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para)
        {
            var criteria = new MatchMonitorRuleGroupBettingCompleteCriteria
            {
                IsLive = para.IsLive,
                BettypeId = para.MatchInfo.BettypeID,
                BetID = para.MatchInfo.BetID,
                MatchId = para.MatchInfo.MatchID,
                ScoreDiff = para.MatchInfo.ScoreDiff,
                Betteam = para.MatchInfo.Betteam,
                TransGroupJson = para.TransGroupJson,
                MaxSequenceID = para.MaxSequenceID
            };

            _repository.Execute(criteria);
        }

        public void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleGroupBettingTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            _repository.Execute(criteria);
        }
    }
}
