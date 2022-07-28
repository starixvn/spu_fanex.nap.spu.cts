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
        IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize);

        MatchMonitorRule2Model GetMatchMonitorRule2(bool isLive, int rowSize);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxTransId, MatchStagingRuleModel matchinfo, bool isFromMasterDB);

        IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxTransId, MatchStagingRule2Model matchinfo);

        void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para);

        void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para);

        void CleanMatchStaging(bool isLive, UInt64 maxTransId);

        void CleanTransRule2(bool isLive, UInt64 maxTransId);
    }

    public class MatchMonitorDataAccess : IMatchMonitorDataAccess
    {
        private readonly IDynamicRepository repository;

        public MatchMonitorDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize, bool isLive)
        {
            var criteria = new MatchTransGetCriteria
            {
                QueryType = (int)Consts.MatchMonitorQueryTypes.Service,
                LastScannedTransID = LastScannedTransID,
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
                MaxTransId = 0,
                RowSize = rowSize
            };

            var result = repository.Fetch<MatchStagingRuleModel>(criteria);

            return new MatchMonitorRuleModel
            {
                MaxTransId = criteria.MaxTransId,
                MatchStagingList = criteria.MaxTransId != 0 ? result : new List<MatchStagingRuleModel>()
            };
        }

        public MatchMonitorRule2Model GetMatchMonitorRule2(bool isLive, int rowSize)
        {
            var criteria = new MatchMonitorRule2GetCriteria
            {
                IsLive = isLive,
                MaxTransId = 0,
                RowSize = rowSize
            };

            var result = repository.Fetch<MatchStagingRule2Model>(criteria);

            return new MatchMonitorRule2Model
            {
                MaxTransId = criteria.MaxTransId,
                MatchStagingList = criteria.MaxTransId != 0 ? result : new List<MatchStagingRule2Model>()
            };
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxTransId, MatchStagingRuleModel matchinfo, bool isFromMasterDB)
        {
            var criteria = new MatchMonitorRuleInsertCriteria(isFromMasterDB)
            {
                IsLive = isLive,
                BettypeId = matchinfo.BetTypeID,
                MatchId = matchinfo.MatchId,
                TotalScore = matchinfo.TotalScore,
                MaxTransId = maxTransId,
                BetId = matchinfo.BetID
            };

            return repository.Fetch<MatchRuleCompletedModel>(criteria);
        }

        public IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxTransId, MatchStagingRule2Model matchinfo)
        {
            var criteria = new MatchMonitorRule2InsertCriteria()
            {
                IsLive = isLive,
                BettypeId = matchinfo.BetTypeID,
                MatchId = matchinfo.MatchId,
                TotalScore = matchinfo.TotalScore,
                MaxTransId = maxTransId,
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
                MaxTransId = para.MaxTransId
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
                MaxTransId = para.MaxTransId,
                Betteam = para.MatchInfo.Betteam
            };

            repository.Execute(criteria);
        }

        public void CleanMatchStaging(bool isLive, UInt64 maxTransId)
        {
            var criteria = new MatchMonitorRuleTransCleanCriteria
            {
                IsLive = isLive,
                MaxTransId = maxTransId
            };

            repository.Execute(criteria);
        }

        public void CleanTransRule2(bool isLive, UInt64 maxTransId)
        {
            var criteria = new MatchMonitorRule2TransCleanCriteria
            {
                IsLive = isLive,
                MaxTransId = maxTransId
            };

            repository.Execute(criteria);
        }
    }
}