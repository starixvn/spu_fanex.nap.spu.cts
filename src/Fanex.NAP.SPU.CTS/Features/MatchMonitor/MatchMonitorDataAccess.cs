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

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId, long maxTransId);

        void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson, long maxTransId);

        void CleanMatchStaging(bool isLive, long maxTransId);

        MatchMonitorRuleModel GetMatchMonitorRule(bool isLive);
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

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId, long maxTransId)
        {
            var criteria = new MatchMonitorRuleInsertCriteria
            {
                IsLive = isLive,
                BettypeId = bettypeId,
                MatchId = matchId,
                TotalScore = totalScore,
                MaxTransId = maxTransId
            };

            return repository.Fetch<MatchRuleCompletedModel>(criteria);
        }

        public void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson, long maxTransId)
        {
            var criteria = new MatchTransUpdateCompleteCriteria
            {
                IsLive = isLive,
                BettypeId = bettypeId,
                MatchId = matchId,
                TotalScore = totalScore,
                TransGroupJson = transGroupJson,
                MaxTransId = maxTransId
            };

            repository.Execute(criteria);
        }

        public void CleanMatchStaging(bool isLive, long maxTransId)
        {
            var criteria = new MatchMonitorStagingCleanCriteria
            {
                IsLive = isLive,
                MaxTransId = maxTransId
            };

            repository.Execute(criteria);
        }

        public MatchMonitorRuleModel GetMatchMonitorRule(bool isLive)
        {
            var criteria = new MatchMonitorRuleGetCriteria
            {
                IsLive = isLive,
                MaxTransId = 0
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