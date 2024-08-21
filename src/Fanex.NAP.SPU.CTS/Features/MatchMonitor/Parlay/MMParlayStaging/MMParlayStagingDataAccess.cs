namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging.Criteria;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;

    public interface IMMParlayStagingDataAccess
    {
        IEnumerable<ParlayTicketDetailEntity> GetParlayTickets(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos);

        void InsertParlayTickets(string matchTransJson, bool isLive, int poolType);
    }

    public class MMParlayStagingDataAccess : IMMParlayStagingDataAccess
    {
        private readonly IDynamicRepository repository;

        public MMParlayStagingDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<ParlayTicketDetailEntity> GetParlayTickets(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos)
        {
            var criteria = new ParlayTicketDetailGetCriteria
            {
                QueryType = (int)Consts.MatchMonitorQueryTypes.Service,
                LastScannedSequenceID = lastScannedSequenceID,
                BatchSize = batchSize,
                IsLive = isLive,
                ListSportBettype = betTypeInfos
            };

            return repository.Fetch<ParlayTicketDetailEntity>(criteria);
        }

        public void InsertParlayTickets(string matchTransJson, bool isLive, int poolType)
        {
            var criteria = new MatchMonitorStagingInsertCriteria
            {
                MatchTransJson = matchTransJson,
                IsLive = isLive,
                PoolType = poolType
            };

            repository.Execute(criteria);
        }
    }
}
