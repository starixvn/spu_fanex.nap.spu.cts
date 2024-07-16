
namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging.Entities;

    public interface IMatchMonitorStagingDataAccess
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive, int poolType);
    }
    public class MatchMonitorStagingDataAccess : IMatchMonitorStagingDataAccess
    {
        private readonly IDynamicRepository repository;

        public MatchMonitorStagingDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos)
        {
            var criteria = new MatchTransGetCriteria
            {
                QueryType = (int)Consts.MatchMonitorQueryTypes.Service,
                LastScannedSequenceID = lastScannedSequenceID,
                BatchSize = batchSize,
                IsLive = isLive,
                ListSportBettype = betTypeInfos
            };

            return repository.Fetch<TicketDetailEntity>(criteria);
        }

        public void InsertMatchMonitorStaging(string matchTransJson, bool isLive, int poolType)
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
