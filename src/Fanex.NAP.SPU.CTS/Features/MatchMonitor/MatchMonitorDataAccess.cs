namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorDataAccess
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize);

        MatchMonitorRule2Model GetMatchMonitorRule2(bool isLive);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxSequenceID, MatchStagingRuleModel matchinfo);

        IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxSequenceID, MatchStagingRule2Model matchinfo);

        void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para);

        void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para);

        void CleanMatchStaging(bool isLive, UInt64 maxSequenceID);

        void CleanTransRule2(bool isLive, UInt64 maxSequenceID);
    }

    public class MatchMonitorDataAccess : IMatchMonitorDataAccess
    {
        private readonly IDynamicRepository repository;

        public MatchMonitorDataAccess()
        {
            repository = new DynamicRepository();
        }

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

        public MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize)
        {
            var criteria = new MatchMonitorRuleGetCriteria
            {
                IsLive = isLive,
                RowSize = rowSize
            };

            var result = repository.Fetch<MatchStagingRuleModel>(criteria);

            return new MatchMonitorRuleModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID != 0 ? result : new List<MatchStagingRuleModel>()
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

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxSequenceID, MatchStagingRuleModel matchinfo)
        {
            var criteria = new MatchMonitorRuleInsertCriteria()
            {
                IsLive = isLive,
                BettypeId = matchinfo.BetTypeID,
                MatchId = matchinfo.MatchId,
                TotalScore = matchinfo.TotalScore,
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

        public void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para)
        {
            var criteria = new MatchMonitorRuleCompleteCriteria
            {
                IsLive = para.IsLive,
                BettypeId = para.MatchInfo.BetTypeID,
                BetID = para.MatchInfo.BetID,
                MatchId = para.MatchInfo.MatchId,
                TotalScore = para.MatchInfo.TotalScore,
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

        public void CleanMatchStaging(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleTransCleanCriteria
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
    }
}