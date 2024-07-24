namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorHedging
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models;

    public interface IMatchMonitorRuleHedgingDataAccess
    {
        MatchMonitorRuleHedgingModel GetMatchRuleHedging(bool isLive);

        MatchProcessRuleHedgingModel ProcessRuleHedging(bool isLive, MatchStagingRuleHedgingEntity matchinfo, string sequenceIDList, string groupDetection);

        void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para);

        void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorRuleHedgingDataAccess : IMatchMonitorRuleHedgingDataAccess
    {
        private readonly IDynamicRepository _repository;

        public MatchMonitorRuleHedgingDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public MatchMonitorRuleHedgingModel GetMatchRuleHedging(bool isLive)
        {
            var criteria = new MatchMonitorRuleHedgingGetCriteria
            {
                IsLive = isLive
            };

            var result = _repository.Fetch<MatchStagingRuleHedgingEntity>(criteria);

            return new MatchMonitorRuleHedgingModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleHedgingEntity>()
            };
        }

        public MatchProcessRuleHedgingModel ProcessRuleHedging(bool isLive, MatchStagingRuleHedgingEntity matchinfo, string sequenceIDList, string groupDetection)
        {
            var criteria = new MatchMonitorRuleHedgingProcessCriteria()
            {
                IsLive = isLive,
                MatchId = matchinfo.MatchId,
                ScoreDiff = matchinfo.ScoreDiff,
                BettypeId = matchinfo.BetTypeID,
                BetId = matchinfo.BetID,
                Betteam = matchinfo.BetTeam,
                SportType = matchinfo.SportType,
                SequenceIDList = sequenceIDList,
                CustGroup = groupDetection
            };

            var result = _repository.FetchMultiple<MatchCompleteRuleHedgingEntity, MatchProcessRuleHedgingInfoEntity>(criteria);

            return new MatchProcessRuleHedgingModel
            {
                MatchInfo = matchinfo,
                CompleteMatches = result.firstResultSet,
                ReprocessMatches = result.secondResultSet
            };
        }

        public void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para)
        {
            var criteria = new MatchMonitorRuleHedgingCompleteCriteria
            {
                IsLive = para.IsLive,
                MaxSequenceID = para.MaxSequenceID,
                MatchId = para.MatchInfo.MatchId,
                ScoreDiff = para.MatchInfo.ScoreDiff,
                BettypeId = para.MatchInfo.BetTypeID,
                BetID = para.MatchInfo.BetID,
                Betteam = para.MatchInfo.BetTeam,
                TransGroupJson = para.TransGroupJson,
            };

            _repository.Execute(criteria);
        }

        public void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleHedgingTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            _repository.Execute(criteria);
        }
    }
}
