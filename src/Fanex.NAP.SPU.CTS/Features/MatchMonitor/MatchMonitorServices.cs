namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize, bool isLive);

        IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        void InsertMatchMonitorRule(int tempGroupID, bool isLive);

        void UpdateMatchTransComplete(string result, bool isLive);
    }

    public class MatchMonitorServices : IMatchMonitorServices
    {
        private readonly IMatchMonitorDataAccess matchDataAccess;

        public MatchMonitorServices(IMatchMonitorDataAccess matchDataAccess)
        {
            this.matchDataAccess = matchDataAccess;
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize, bool isLive)
        {
            return matchDataAccess.GetMatchTrans(LastScannedTransID, batchSize, isLive);
        }

        public IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            return matchDataAccess.InsertMatchMonitorStaging(matchTransJson, isLive);
        }

        public void InsertMatchMonitorRule(int tempGroupID, bool isLive)
        {
            matchDataAccess.InsertMatchMonitorRule(tempGroupID, isLive);
        }

        public void UpdateMatchTransComplete(string result, bool isLive)
        {
            matchDataAccess.UpdateMatchTransComplete(result, isLive);
        }
    }
}