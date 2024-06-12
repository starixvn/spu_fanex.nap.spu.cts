namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleHedging;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models;

    public interface IMatchMonitorDataAccess
    {
        MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive);

        MatchMonitorRuleHedgingModel GetMatchRuleHedging(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxSequenceID, MatchStagingRuleFixedGameModel matchinfo);

        MatchProcessRuleHedgingModel ProcessRuleHedging(bool isLive, MatchStagingRuleHedgingEntity matchinfo, string sequenceIDList, string groupDetection);

        void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para);

        void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para);

        void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorDataAccess : IMatchMonitorDataAccess
    {
        private readonly IDynamicRepository repository;

        public MatchMonitorDataAccess()
        {
            repository = new DynamicRepository();
        }

        #region Process Rule
        public MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive)
        {
            var criteria = new MatchMonitorRuleFixedGameGetCriteria
            {
                IsLive = isLive,
                MaxSequenceID = 0
            };

            var result = repository.Fetch<MatchStagingRuleFixedGameModel>(criteria);

            return new MatchMonitorRuleFixedGameModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleFixedGameModel>()
            };
        }

        public MatchMonitorRuleHedgingModel GetMatchRuleHedging(bool isLive)
        {
            var criteria = new MatchMonitorRuleHedgingGetCriteria
            {
                IsLive = isLive
            };

            var result = repository.Fetch<MatchStagingRuleHedgingEntity>(criteria);

            return new MatchMonitorRuleHedgingModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleHedgingEntity>()
            };
        }

        public MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive)
        {
            var criteria = new MatchMonitorRuleIrrigationGetCriteria { IsLive = isLive };

            var result = repository.Fetch<MatchStagingRuleIrrigationModel>(criteria);

            return new MatchMonitorRuleIrrigationModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleIrrigationModel>()
            };
        }

        public IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxSequenceID, MatchStagingRuleFixedGameModel matchinfo)
        {
            var criteria = new MatchMonitorRuleFixedGameProcessCriteria()
            {
                IsLive = isLive,
                BettypeId = matchinfo.BetTypeID,
                MatchId = matchinfo.MatchID,
                ScoreDiff = matchinfo.ScoreDiff,
                MaxSequenceID = maxSequenceID,
                BetId = matchinfo.BetID,
                HDP = matchinfo.HDP,
                BetTeam = matchinfo.Betteam
            };

            return repository.Fetch<MatchCompleteRuleFixedGameModel>(criteria);
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

            var result = repository.FetchMultiple<MatchCompleteRuleHedgingEntity, MatchProcessRuleHedgingInfoEntity>(criteria);

            return new MatchProcessRuleHedgingModel
            {
                MatchInfo = matchinfo,
                CompleteMatches = result.firstResultSet,
                ReprocessMatches = result.secondResultSet
            };
        }

        public IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo)
        {
            var criteria = new MatchMonitorRuleIrrigationInsertCriteria
            {
                IsLive = isLive,
                SportType = matchinfo.SportType,
                BettypeID = matchinfo.BettypeID,
                MatchID = matchinfo.MatchID,
                ScoreDiff = matchinfo.ScoreDiff,
                MaxSequenceID = maxSequenceID,
                BetID = matchinfo.BetID,
                Betteam = matchinfo.Betteam,
                HDP = matchinfo.HDP
            };

            return repository.Fetch<MatchRuleIrrigationCompletedModel>(criteria);
        }

        public void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para)
        {
            var criteria = new MatchMonitorRuleFixedGameCompleteCriteria
            {
                IsLive = para.IsLive,
                MaxSequenceID = para.MaxSequenceID,
                MatchId = para.MatchInfo.MatchID,
                ScoreDiff = para.MatchInfo.ScoreDiff,
                BettypeId = para.MatchInfo.BetTypeID,
                BetID = para.MatchInfo.BetID,
                HDP = para.MatchInfo.HDP,
                Betteam = para.MatchInfo.Betteam,
                TransGroupJson = para.TransGroupJson
            };

            repository.Execute(criteria);
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

            repository.Execute(criteria);
        }

        public void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para)
        {
            var criteria = new MatchMonitorRuleIrrigationCompleteCriteria
            {
                IsLive = para.IsLive,
                SportType = para.MatchInfo.SportType,
                BettypeID = para.MatchInfo.BettypeID,
                BetID = para.MatchInfo.BetID,
                MatchID = para.MatchInfo.MatchID,
                ScoreDiff = para.MatchInfo.ScoreDiff,
                Betteam = para.MatchInfo.Betteam,
                TransGroupJson = para.TransGroupJson,
                MaxSequenceID = para.MaxSequenceID,
                HDP = para.MatchInfo.HDP
            };

            repository.Execute(criteria);
        }

        public void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleFixedGameTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            repository.Execute(criteria);
        }

        public void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleHedgingTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            repository.Execute(criteria);
        }

        public void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleIrrigationTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            repository.Execute(criteria);
        }
        #endregion Process Rule
    }
}