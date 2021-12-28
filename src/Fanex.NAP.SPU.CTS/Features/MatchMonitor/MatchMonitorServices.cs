namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 LastScannedTransID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId, UInt64 maxTransId, bool isProccessFromMaster);

        void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para);

        void CleanMatchStaging(bool isLive, UInt64 maxTransId);

        MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize);
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

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, int matchId, int totalScore, int bettypeId, UInt64 maxTransId, bool isProccessFromMaster)
        {
            return isProccessFromMaster ? matchDataAccess.InsertMatchMonitorRule(isLive, matchId, totalScore, bettypeId, maxTransId) 
                : matchDataAccess.InsertMatchMonitorRuleToSlaveDB(isLive, matchId, totalScore, bettypeId, maxTransId);
        }

        public void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para)
        {
            matchDataAccess.UpdateMatchTransComplete(para);
        }

        public void CleanMatchStaging(bool isLive, UInt64 maxTransId)
        {
            matchDataAccess.CleanMatchStaging(isLive, maxTransId);
        }

        public MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize)
        {
            return matchDataAccess.GetMatchMonitorRule(isLive, rowSize);
        }
    }
}