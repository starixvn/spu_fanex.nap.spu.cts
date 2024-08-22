namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting
{
    using System;
    using System.Linq;
    using Fanex.Data.Repository;

    public interface IMMParlayRuleGroupBettingDataAccess
    {
        MMParlayRuleGroupBettingModel GetMMParlayRuleGroupBettting(bool isLive);

        MMParlayProcessRuleGroupBettingModel ProcessRuleGroupBetting(bool isLive,
            MMParlayStagingRuleGroupBettingEntity matchInfo,
            string sequenceIDList,
            string groupDetection);

        void CompleteTransRuleGroupBetting(MMParlayCompleteRuleGroupBettingParameterModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);
    }

    public class MMParlayRuleGroupBettingDataAccess : IMMParlayRuleGroupBettingDataAccess
    {
        private readonly IDynamicRepository _repository;

        public MMParlayRuleGroupBettingDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public MMParlayRuleGroupBettingModel GetMMParlayRuleGroupBettting(bool isLive)
        {
            var criteria = new MMParlayRuleGroupBettingGetCriteria
            {
                IsLive = isLive
            };

            var matchStagings = _repository.Fetch<MMParlayStagingRuleGroupBettingEntity>(criteria);

            return new MMParlayRuleGroupBettingModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID.HasValue ? matchStagings : Enumerable.Empty<MMParlayStagingRuleGroupBettingEntity>()
            };
        }

        public MMParlayProcessRuleGroupBettingModel ProcessRuleGroupBetting(bool isLive,
            MMParlayStagingRuleGroupBettingEntity matchInfo, 
            string sequenceIDList, 
            string groupDetection)
        {
            var criteria = new MMParlayRuleGroupBettingProcessCriteria()
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

            (var completeMatches, var reprocessMatches) = _repository.FetchMultiple<MMParlayCompleteRuleGroupBettingEntity, MMParlayProcessRuleGroupBettingInfoEntity>(criteria);

            return new MMParlayProcessRuleGroupBettingModel
            {
                MatchInfo = matchInfo,
                CompleteMatches = completeMatches,
                ReprocessMatches = reprocessMatches
            };
        }

        public void CompleteTransRuleGroupBetting(MMParlayCompleteRuleGroupBettingParameterModel para)
        {
            var criteria = new MMParlayRuleGroupBettingCompleteCriteria
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
            var criteria = new MMRuleGroupBettingTransCleanCriteria()
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            _repository.Execute(criteria);
        }

    }
}
