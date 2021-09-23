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

        IEnumerable<MatchStagingModel> InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId);

        void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson);

        void CleanMatchStaging(bool isLive);
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

        public IEnumerable<MatchStagingModel> InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            var criteria = new MatchMonitorStagingInsertCriteria
            {
                MatchTransJson = matchTransJson,
                IsLive = isLive
            };

            return repository.Fetch<MatchStagingModel>(criteria);
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId)
        {
            var criteria = new MatchMonitorRuleInsertCriteria
            {
               IsLive = isLive,
               BettypeId = bettypeId,
               MatchId = matchId,
               TotalScore = totalScore
            };

            return repository.Fetch<MatchRuleCompletedModel>(criteria);
        }

        public void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson)
        {
            var criteria = new MatchTransUpdateCompleteCriteria
            {
                IsLive = isLive,
                BettypeId = bettypeId,
                MatchId = matchId,
                TotalScore = totalScore,
                TransGroupJson = transGroupJson
            };

            repository.Execute(criteria);
        }

        public void CleanMatchStaging(bool isLive)
        {
            var criteria = new MatchMonitorStagingCleanCriteria
            {
                IsLive = isLive
            };

            repository.Execute(criteria);
        }
    }
}