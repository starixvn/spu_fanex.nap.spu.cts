namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models;

    public interface IMatchMonitorRuleArbitrageDataAccess
    {
        MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive);

        MatchProcessRuleArbitrageModel ProcessRuleArbitrage(bool isLive, MatchStagingRuleArbitrageEntity matchInfo, string sequenceIDList, string groupDetection);

        void CompleteTransRuleArbitrage(MatchCompleteRuleArbitrageParameterModel para);

        void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorRuleArbitrageDataAccess : IMatchMonitorRuleArbitrageDataAccess
    {
        private readonly IDynamicRepository _repository;

        public MatchMonitorRuleArbitrageDataAccess(IDynamicRepository repository)
        {
            _repository = repository;
        }

        public MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive)
        {
            var criteria = new MatchMonitorRuleArbitrageGetCriteria
            {
                IsLive = isLive
            };

            var result = _repository.Fetch<MatchStagingRuleArbitrageEntity>(criteria);

            return new MatchMonitorRuleArbitrageModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleArbitrageEntity>()
            };
        }

        public MatchProcessRuleArbitrageModel ProcessRuleArbitrage(bool isLive, MatchStagingRuleArbitrageEntity matchInfo, string sequenceIDList, string groupDetection)
        {
            var criteria = new MatchMonitorRuleArbitrageProcessCriteria()
            {
                IsLive = isLive,
                MatchId = matchInfo.MatchId,
                IsMajorLeague = matchInfo.IsMajorLeague,
                ScoreDiff = matchInfo.ScoreDiff,
                BettypeId = matchInfo.BetTypeID,
                BetId = matchInfo.BetID,
                HDP = matchInfo.HDP,
                SequenceIDList = sequenceIDList,
                CustGroup = groupDetection
            };

            var (matchComplete, matchReprocess) = _repository.FetchMultiple<MatchCompleteRuleArbitrageEntity, MatchProcessRuleArbitrageInfoEntity>(criteria);

            return new MatchProcessRuleArbitrageModel
            {
                MatchInfo = matchInfo,
                CompleteMatches = matchComplete,
                ReprocessMatches = matchReprocess
            };
        }

        public void CompleteTransRuleArbitrage(MatchCompleteRuleArbitrageParameterModel para)
        {
            var criteria = new MatchMonitorRuleArbitrageCompleteCriteria
            {
                IsLive = para.IsLive,
                MaxSequenceID = para.MaxSequenceID,
                MatchId = para.MatchInfo.MatchId,
                ScoreDiff = para.MatchInfo.ScoreDiff,
                BettypeId = para.MatchInfo.BetTypeID,
                BetID = para.MatchInfo.BetID,
                HDP = para.MatchInfo.HDP,
                TransGroupJson = para.TransGroupJson,
            };

            _repository.Execute(criteria);
        }

        public void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleArbitrageTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            _repository.Execute(criteria);
        }
    }
}
