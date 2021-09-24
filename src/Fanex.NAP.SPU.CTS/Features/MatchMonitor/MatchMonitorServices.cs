namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId, long maxTransId);

        void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson, long maxTransId);

        void CleanMatchStaging(bool isLive, long maxTransId);

        MatchMonitorRuleModel GetMatchMonitorRule(bool isLive);
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

        public void InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            matchDataAccess.InsertMatchMonitorStaging(matchTransJson, isLive);
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId, long maxTransId)
        {
            return matchDataAccess.InsertMatchMonitorRule(isLive, matchId, totalScore, bettypeId, maxTransId);
        }

        public void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson, long maxTransId)
        {
            matchDataAccess.UpdateMatchTransComplete(isLive, matchId, totalScore, bettypeId, transGroupJson, maxTransId);
        }

        public void CleanMatchStaging(bool isLive, long maxTransId)
        {
            matchDataAccess.CleanMatchStaging(isLive, maxTransId);
        }

        public MatchMonitorRuleModel GetMatchMonitorRule(bool isLive)
        {
            return matchDataAccess.GetMatchMonitorRule(isLive);
        }
    }
}