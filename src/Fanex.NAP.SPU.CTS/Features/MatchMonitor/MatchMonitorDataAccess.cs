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
        IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize);

        IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson);

        void InsertMatchMonitorRule(int tempGroupID);

        void UpdateMatchTransComplete();
    }

    public class MatchMonitorDataAccess : IMatchMonitorDataAccess
    {
        private readonly IDynamicRepository repository;

        public MatchMonitorDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize)
        {
            var criteria = new MatchTransGetCriteria
            {
                QueryType = (int)Consts.MatchMonitorQueryTypes.Service,
                LastScannedTransID = LastScannedTransID,
                BatchSize = batchSize,
                EventDate = DateTime.Now
            };

            return repository.Fetch<TicketDetailEntity>(criteria);
        }

        public IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson)
        {
            var criteria = new MatchMonitorStagingInsertCriteria
            {
                MatchTransJson = matchTransJson
            };

            return repository.Fetch<int>(criteria);
        }

        public void InsertMatchMonitorRule(int tempGroupID)
        {
            var criteria = new MatchMonitorRuleInsertCriteria
            {
                TempGroupID = tempGroupID
            };

            repository.Execute(criteria);
        }

        public void UpdateMatchTransComplete()
        {
            var criteria = new MatchTransUpdateCompleteCriteria();

            repository.Execute(criteria);
        }
    }
}