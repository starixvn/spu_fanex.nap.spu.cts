namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using System;
    using System.Collections.Generic;

    public interface IMatchMonitorServices
    {
        IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive);

        void InsertMatchMonitorStaging(string matchTransJson, bool isLive);

        MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize);

        MatchMonitorRule2Model GetMatchMonitorRule2(bool isLive);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxTransId, MatchStagingRuleModel matchinfo);

        IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxTransId, MatchStagingRule2Model matchinfo);

        void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para);

        void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para);

        void CleanMatchStaging(bool isLive, UInt64 maxSequenceID);

        void CleanTransRule2(bool isLive, UInt64 maxTransId);
    }

    public class MatchMonitorServices : IMatchMonitorServices
    {
        private readonly IMatchMonitorDataAccess matchDataAccess;

        public MatchMonitorServices(IMatchMonitorDataAccess matchDataAccess)
        {
            this.matchDataAccess = matchDataAccess;
        }

        public IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive)
        {
            return matchDataAccess.GetMatchTrans(lastScannedSequenceID, batchSize, isLive);
        }

        public void InsertMatchMonitorStaging(string matchTransJson, bool isLive)
        {
            matchDataAccess.InsertMatchMonitorStaging(matchTransJson, isLive);
        }

        public MatchMonitorRuleModel GetMatchMonitorRule(bool isLive, int rowSize)
        {
            return matchDataAccess.GetMatchMonitorRule(isLive, rowSize);
        }

        public MatchMonitorRule2Model GetMatchMonitorRule2(bool isLive)
        {
            return matchDataAccess.GetMatchMonitorRule2(isLive);
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxTransId, MatchStagingRuleModel matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRule(isLive, maxTransId, matchinfo);
        }

        public IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxTransId, MatchStagingRule2Model matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRule2(isLive, maxTransId, matchinfo);
        }

        public void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para)
        {
            matchDataAccess.UpdateMatchTransComplete(para);
        }

        public void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para)
        {
            matchDataAccess.UpdateMatchMonitorRule2Complete(para);
        }

        public void CleanMatchStaging(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanMatchStaging(isLive, maxSequenceID);
        }

        public void CleanTransRule2(bool isLive, UInt64 maxTransId)
        {
            matchDataAccess.CleanTransRule2(isLive, maxTransId);
        }
    }
}