namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMatchMonitorServices
    {
        SystemParameterEntity GetLastScannedTransID();

        IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize);

        IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson);

        void InsertMatchMonitorRule(int tempGroupID);

        void UpdateMatchTransComplete();
    }

    public class MatchMonitorServices : IMatchMonitorServices
    {
        private readonly IMatchMonitorDataAccess matchDataAccess;

        public MatchMonitorServices(IMatchMonitorDataAccess matchDataAccess)
        {
            this.matchDataAccess = matchDataAccess;
        }

        public SystemParameterEntity GetLastScannedTransID()
        {
            return matchDataAccess.GetPreviousTimingExecution((int)Constants.SYSTEMPARAMETERS.MATCHMONITOR_LASTSCANNEDTRANS)?.SingleOrDefault();
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize)
        {
            return matchDataAccess.GetMatchTrans(LastScannedTransID, batchSize);
        }

        public IEnumerable<int> InsertMatchMonitorStaging(string matchTransJson)
        {
            return matchDataAccess.InsertMatchMonitorStaging(matchTransJson);
        }

        public void InsertMatchMonitorRule(int tempGroupID)
        {
            matchDataAccess.InsertMatchMonitorRule(tempGroupID);
        }

        public void UpdateMatchTransComplete()
        {
            matchDataAccess.UpdateMatchTransComplete();
        }
    }
}
