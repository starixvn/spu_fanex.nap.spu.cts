namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging.Criteria;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMMParlayStagingDataAccess
    {
        (long, IEnumerable<ParlayTicketDetailEntity>) GetParlayTickets(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos);

        void InsertParlayTickets(string matchTransJson, bool isLive, int poolType);
    }

    public class MMParlayStagingDataAccess : IMMParlayStagingDataAccess
    {
        private readonly IDynamicRepository repository;

        public MMParlayStagingDataAccess()
        {
            repository = new DynamicRepository();
        }

        public (long, IEnumerable<ParlayTicketDetailEntity>) GetParlayTickets(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos)
        {
            var criteria = new ParlayTicketDetailGetCriteria
            {              
                LastScannedSequenceID = lastScannedSequenceID,
                BatchSize = batchSize,
                IsLive = isLive,
                ListSportBettype = betTypeInfos
            };
           
            var (maxSequenceID, data) = repository.FetchMultiple<long?, ParlayTicketDetailEntity>(criteria);

            return (maxSequenceID?.FirstOrDefault() ?? 0, data);
        }

        public void InsertParlayTickets(string matchTransJson, bool isLive, int poolType)
        {
            var criteria = new ParlayTicketDetailInsertCriteria
            {
                MatchTransJson = matchTransJson,
                IsLive = isLive,
                PoolType = poolType
            };

            repository.Execute(criteria);
        }
    }
}
