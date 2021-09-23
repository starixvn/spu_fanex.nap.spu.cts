namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize, bool isLive);

        IEnumerable<MatchStagingModel> InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId);

        void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson);

        void CleanMatchStaging(bool isLive);
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

        public IEnumerable<MatchStagingModel> InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            return matchDataAccess.InsertMatchMonitorStaging(matchTransJson, isLive);
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId)
        {
            return matchDataAccess.InsertMatchMonitorRule(isLive, matchId, totalScore, bettypeId);
        }

        public void UpdateMatchTransComplete(bool isLive, int matchId, int totalScore, int bettypeId, string transGroupJson)
        {
            matchDataAccess.UpdateMatchTransComplete(isLive, matchId, totalScore, bettypeId, transGroupJson);
        }

        public void CleanMatchStaging(bool isLive)
        {
            matchDataAccess.CleanMatchStaging(isLive);
        }
    }
}