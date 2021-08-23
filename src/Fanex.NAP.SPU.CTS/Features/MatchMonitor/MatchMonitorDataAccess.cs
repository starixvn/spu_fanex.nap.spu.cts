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

        IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        void InsertMatchMonitorRule(int tempGroupID, bool isLive);

        void UpdateMatchTransComplete(string result, bool isLive);
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

        public IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            var criteria = new MatchMonitorStagingInsertCriteria
            {
                MatchTransJson = matchTransJson,
                IsLive = isLive
            };

            return repository.Fetch<int>(criteria);
        }

        public void InsertMatchMonitorRule(int tempGroupID, bool isLive)
        {
            var criteria = new MatchMonitorRuleInsertCriteria
            {
                TempGroupID = tempGroupID,
                IsLive = isLive 
            };

            repository.Execute(criteria);
        }

        public void UpdateMatchTransComplete(string result, bool isLive)
        {
            var criteria = new MatchTransUpdateCompleteCriteria
            {
                Result = result,
                IsLive = isLive
            };

            repository.Execute(criteria);
        }
    }
}