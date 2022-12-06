namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorDataAccess
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive);

        MatchMonitorRule2Model GetMatchMonitorRule2(bool isLive);

        MatchMonitorRule3Model GetMatchMonitorRule3(bool isLive);

        MatchMonitorRuleIrrigationModel GetMatchMonitorRuleIrrigation(bool isLive);

        IEnumerable<MatchRuleCompletedModel> ProcessRuleGroupBetting(bool isLive, UInt64 maxSequenceID, MatchStagingRuleGroupBettingModel matchinfo);

        IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxSequenceID, MatchStagingRule2Model matchinfo);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule3(bool isLive, UInt64 maxSequenceID, MatchStagingRule3Model matchinfo);

        IEnumerable<MatchRuleIrrigationCompletedModel> InsertMatchMonitorRuleIrrigation(bool isLive, UInt64 maxSequenceID, MatchStagingRuleIrrigationModel matchinfo);

        void CompleteTransRuleGroupBetting(RuleGroupBettingCompleteParameterModel para);

        void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para);

        void UpdateMatchMonitorRule3Complete(MatchMonitorRule3CompleteParaModel para);

        void UpdateMatchMonitorRuleIrrigationComplete(MatchMonitorRuleIrrigationCompleteParaModel para);

        void CleanTransRuleGroupBetting(bool isLive, UInt64 maxSequenceID);

        void CleanTransRule2(bool isLive, UInt64 maxSequenceID);

        void CleanTransRule3(bool isLive, UInt64 maxSequenceID);

        void CleanTransRuleIrrigation(bool isLive, UInt64 maxSequenceID);
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
            try
            {
                var criteria = new MatchMonitorStagingInsertCriteria
                {
                    MatchTransJson = matchTransJson,
                    IsLive = isLive
                };

                repository.Execute(criteria);
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error($"Insert Match Monitor Staging Json { matchTransJson } Exception { logMessage }", ex);

                throw;
            }
        }

        #endregion Insert trans into staging

        #region Process Rule

        public MatchMonitorRuleGroupBettingModel GetMatchRuleGroupBetting(bool isLive)
        {
            var criteria = new MatchRuleGroupBettingGetCriteria
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

        public MatchMonitorRule2Model GetMatchMonitorRule2(bool isLive)
        {
            var criteria = new MatchMonitorRule2GetCriteria
            {
                IsLive = isLive,
                MaxSequenceID = 0
            };

            var result = repository.Fetch<MatchStagingRule2Model>(criteria);

            return new MatchMonitorRule2Model
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRule2Model>()
            };
        }

        public MatchMonitorRule3Model GetMatchMonitorRule3(bool isLive)
        {
            var criteria = new MatchMonitorRule3GetCriteria
            {
                IsLive = isLive
            };

            var result = repository.Fetch<MatchStagingRule3Model>(criteria);

            return new MatchMonitorRule3Model
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRule3Model>()
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

        public IEnumerable<MatchRuleCompletedModel> ProcessRuleGroupBetting(bool isLive, UInt64 maxSequenceID, MatchStagingRuleGroupBettingModel matchinfo)
        {
            var criteria = new MatchMonitorRuleGroupBettingProcessCriteria()
            {
                IsLive = isLive,
                BettypeId = matchinfo.BettypeID,
                MatchId = matchinfo.MatchID,
                TotalScore = matchinfo.ScoreDiff,
                MaxSequenceID = maxSequenceID,
                BetId = matchinfo.BetID
            };

            return repository.Fetch<MatchRuleCompletedModel>(criteria);
        }

        public IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxSequenceID, MatchStagingRule2Model matchinfo)
        {
            var criteria = new MatchMonitorRule2InsertCriteria()
            {
                IsLive = isLive,
                BettypeId = matchinfo.BetTypeID,
                MatchId = matchinfo.MatchId,
                TotalScore = matchinfo.TotalScore,
                MaxSequenceID = maxSequenceID,
                BetId = matchinfo.BetID,
                BetTeam = matchinfo.Betteam
            };

            return repository.Fetch<MatchRule2CompletedModel>(criteria);
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule3(bool isLive, UInt64 maxSequenceID, MatchStagingRule3Model matchinfo)
        {
            var criteria = new MatchMonitorRule3InsertCriteria()
            {
                IsLive = isLive,
                BettypeId = matchinfo.BetTypeID,
                MatchId = matchinfo.MatchId,
                TotalScore = matchinfo.TotalScore,
                MaxSequenceID = maxSequenceID,
                BetId = matchinfo.BetID,
                Betteam = matchinfo.BetTeam
            };

            return repository.Fetch<MatchRuleCompletedModel>(criteria);
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
                Betteam = matchinfo.Betteam
            };

            return repository.Fetch<MatchRuleIrrigationCompletedModel>(criteria);
        }

        public void CompleteTransRuleGroupBetting(RuleGroupBettingCompleteParameterModel para)
        {
            var criteria = new MatchMonitorRuleGroupBettingCompleteCriteria
            {
                IsLive = para.IsLive,
                BettypeId = para.MatchInfo.BettypeID,
                BetID = para.MatchInfo.BetID,
                MatchId = para.MatchInfo.MatchID,
                ScoreDiff = para.MatchInfo.ScoreDiff,
                TransGroupJson = para.TransGroupJson,
                MaxSequenceID = para.MaxSequenceID
            };

            repository.Execute(criteria);
        }

        public void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para)
        {
            var criteria = new MatchMonitorRule2CompleteCriteria
            {
                IsLive = para.IsLive,
                BettypeId = para.MatchInfo.BetTypeID,
                BetID = para.MatchInfo.BetID,
                MatchId = para.MatchInfo.MatchId,
                TotalScore = para.MatchInfo.TotalScore,
                TransGroupJson = para.TransGroupJson,
                MaxSequenceID = para.MaxSequenceID,
                Betteam = para.MatchInfo.Betteam
            };

            repository.Execute(criteria);
        }

        public void UpdateMatchMonitorRule3Complete(MatchMonitorRule3CompleteParaModel para)
        {
            var criteria = new MatchMonitorRule3CompleteCriteria
            {
                IsLive = para.IsLive,
                BettypeId = para.MatchInfo.BetTypeID,
                BetID = para.MatchInfo.BetID,
                MatchId = para.MatchInfo.MatchId,
                TotalScore = para.MatchInfo.TotalScore,
                Betteam = para.MatchInfo.BetTeam,
                TransGroupJson = para.TransGroupJson,
                MaxSequenceID = para.MaxSequenceID
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
                MaxSequenceID = para.MaxSequenceID
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

        public void CleanTransRule2(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRule2TransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            repository.Execute(criteria);
        }

        public void CleanTransRule3(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRule3TransCleanCriteria
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