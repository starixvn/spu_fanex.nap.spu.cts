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

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxTransId, MatchStagingModel matchinfo);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRuleToSlaveDB(bool isLive, UInt64 maxTransId, MatchStagingModel matchinfo);

        void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para);

        void CleanMatchStaging(bool isLive, UInt64 maxTransId);

        MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize);
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

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxTransId, MatchStagingModel matchinfo)
        {
            var criteria = new MatchMonitorRuleInsertCriteria
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


        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRuleToSlaveDB(bool isLive, UInt64 maxTransId, MatchStagingModel matchinfo)
        {
            var criteria = new MatchMonitorRuleInsertToSlaveDBCriteria
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

        public void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para)
        {
            var criteria = new MatchTransUpdateCompleteCriteria
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

        public void CleanMatchStaging(bool isLive, UInt64 maxTransId)
        {
            var criteria = new MatchMonitorStagingCleanCriteria
            {
                IsLive = isLive,
                MaxTransId = maxTransId
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

            var result = repository.Fetch<MatchStagingModel>(criteria);

            return new MatchMonitorRuleModel
            {
                MaxTransId = criteria.MaxTransId,
                MatchStagingList = criteria.MaxTransId != 0 ? result : new List<MatchStagingModel>()
            };
        }
    }
}