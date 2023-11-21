namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorDataAccess
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive);

        MatchMonitorRuleFixedGameModel GetMatchRuleFixedGame(bool isLive);

        MatchMonitorRuleHedgingModel GetMatchRuleHedging(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive);

        IEnumerable<MatchCompleteRuleGroupBettingModel> ProcessRuleGroupBetting(MMPrepareDataProcessRuleGBModel prepareData);

        IEnumerable<MatchCompleteRuleFixedGameModel> ProcessRuleFixedGame(bool isLive, UInt64 maxSequenceID, MatchStagingRuleFixedGameModel matchinfo);

        IEnumerable<MatchCompleteRuleHedgingModel> ProcessRuleHedging(bool isLive, UInt64 maxSequenceID, MatchStagingRuleHedgingModel matchinfo);

        IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo);

        IEnumerable<MatchCompleteRuleArbitrageModel> ProcessRuleArbitrage(bool isLive, UInt64 maxSequenceID, MatchStagingRuleArbitrageModel matchinfo);

        void CompleteTransRuleGroupBetting(MatchCompleteRuleGroupBettingParameterModel para);

        void CompleteTransRuleFixedGame(MatchCompleteRuleFixedGameParameterModel para);

        void CompleteTransRuleHedging(MatchCompleteRuleHedgingParameterModel para);

        void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para);

        void CompleteTransRuleArbitrage(MatchCompleteRuleArbitrageParameterModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleFixedGame(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleHedging(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorDataAccess : IMatchMonitorDataAccess
    {
        private readonly IDynamicRepository repository;

        public MatchMonitorDataAccess()
        {
            repository = new DynamicRepository();
        }

        #region Insert trans into staging

        public IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive)
        {
            var criteria = new MatchTransGetCriteria
            {
                QueryType = (int)Consts.MatchMonitorQueryTypes.Service,
                LastScannedSequenceID = lastScannedSequenceID,
                BatchSize = batchSize,
                EventDate = DateTime.Now,
                IsLive = isLive
            };

            return repository.Fetch<TicketDetailEntity>(criteria);
        }

        public void InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            var criteria = new MatchMonitorStagingInsertCriteria
            {
                MatchTransJson = matchTransJson,
                IsLive = isLive
            };

            repository.Execute(criteria);
        }

        #endregion Insert trans into staging

        #region Process Rule

        public MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive)
        {
            var criteria = new MatchMonitorRuleGroupBettingGetCriteria
            {
                IsLive = isLive
            };

            var result = repository.Fetch<MatchStagingRuleGroupBettingModel>(criteria);

            return new MatchMonitorRuleGroupBettingModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleGroupBettingModel>()
            };
        }

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

            var result = repository.Fetch<MatchStagingRuleHedgingModel>(criteria);

            return new MatchMonitorRuleHedgingModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleHedgingModel>()
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

        public MatchMonitorRuleArbitrageModel GetMatchRuleArbitrage(bool isLive)
        {
            var criteria = new MatchMonitorRuleArbitrageGetCriteria
            {
                IsLive = isLive
            };

            var result = repository.Fetch<MatchStagingRuleArbitrageModel>(criteria);

            return new MatchMonitorRuleArbitrageModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleArbitrageModel>()
            };
        }

        public IEnumerable<MatchCompleteRuleGroupBettingModel> ProcessRuleGroupBetting(MMPrepareDataProcessRuleGBModel prepareData)
        {
            var criteria = new MatchMonitorRuleGroupBettingProcessCriteria()
            {
                IsLive = prepareData.IsLive,
                MaxSequenceID = prepareData.MaxSequenceID,
                JsonSharedMatches = prepareData.JsonSharedMatches,
                BettypeId = prepareData.MatchInfo.BettypeID,
                MatchId = prepareData.MatchInfo.MatchID,
                ScoreDiff = prepareData.MatchInfo.ScoreDiff,
                BetId = prepareData.MatchInfo.BetID,
                Betteam = prepareData.MatchInfo.Betteam
            };

            return repository.Fetch<MatchCompleteRuleGroupBettingModel>(criteria);
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

        public IEnumerable<MatchCompleteRuleHedgingModel> ProcessRuleHedging(bool isLive, UInt64 maxSequenceID, MatchStagingRuleHedgingModel matchinfo)
        {
            var criteria = new MatchMonitorRuleHedgingProcessCriteria()
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID,
                MatchId = matchinfo.MatchId,
                ScoreDiff = matchinfo.ScoreDiff,
                BettypeId = matchinfo.BetTypeID,
                BetId = matchinfo.BetID,
                Betteam = matchinfo.BetTeam
            };

            return repository.Fetch<MatchCompleteRuleHedgingModel>(criteria);
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

        public IEnumerable<MatchCompleteRuleArbitrageModel> ProcessRuleArbitrage(bool isLive, UInt64 maxSequenceID, MatchStagingRuleArbitrageModel matchinfo)
        {
            var criteria = new MatchMonitorRuleArbitrageProcessCriteria()
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID,
                MatchId = matchinfo.MatchId,
                IsMajorLeague = matchinfo.IsMajorLeague,
                ScoreDiff = matchinfo.ScoreDiff,
                BettypeId = matchinfo.BetTypeID,
                BetId = matchinfo.BetID,
                HDP = matchinfo.HDP
            };

            return repository.Fetch<MatchCompleteRuleArbitrageModel>(criteria);
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

            repository.Execute(criteria);
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

            repository.Execute(criteria);
        }

        public void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleGroupBettingTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
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

        public void CleanTransRuleArbitrage(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleArbitrageTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            repository.Execute(criteria);
        }

        #endregion Process Rule
    }
}