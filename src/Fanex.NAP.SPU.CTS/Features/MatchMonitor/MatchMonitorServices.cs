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

        MatchMonitorRuleModel GetMatchMonitorRule3(bool isLive, int rowSize);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxTransId, MatchStagingRuleModel matchinfo);

        IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxTransId, MatchStagingRule2Model matchinfo);

        IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule3(bool isLive, UInt64 maxTransId, MatchStagingRuleModel matchinfo);

        void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para);

        void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para);
       
        void UpdateMatchMonitorRule3Complete(MatchMonitorRuleCompleteParaModel para);

        void CleanMatchStaging(bool isLive, UInt64 maxSequenceID);

        void CleanTransRule2(bool isLive, UInt64 maxSequenceID);
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

        public MatchMonitorRuleModel GetMatchMonitorRule3(bool isLive, int rowSize)
        {
            return matchDataAccess.GetMatchMonitorRule3(isLive, rowSize);
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule(bool isLive, UInt64 maxSequenceID, MatchStagingRuleModel matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRule(isLive, maxSequenceID, matchinfo);
        }

        public IEnumerable<MatchRule2CompletedModel> InsertMatchMonitorRule2(bool isLive, UInt64 maxSequenceID, MatchStagingRule2Model matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRule2(isLive, maxSequenceID, matchinfo);
        }

        public IEnumerable<MatchRuleCompletedModel> InsertMatchMonitorRule3(bool isLive, UInt64 maxSequenceID, MatchStagingRuleModel matchinfo)
        {
            return matchDataAccess.InsertMatchMonitorRule3(isLive, maxSequenceID, matchinfo);
        }

        public void UpdateMatchTransComplete(MatchMonitorRuleCompleteParaModel para)
        {
            matchDataAccess.UpdateMatchTransComplete(para);
        }

        public void UpdateMatchMonitorRule2Complete(MatchMonitorRule2CompleteParaModel para)
        {
            matchDataAccess.UpdateMatchMonitorRule2Complete(para);
        }

        public void UpdateMatchMonitorRule3Complete(MatchMonitorRuleCompleteParaModel para)
        {
            matchDataAccess.UpdateMatchMonitorRule3Complete(para);
        }

        public void CleanMatchStaging(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanMatchStaging(isLive, maxSequenceID);
        }

        public void CleanTransRule2(bool isLive, UInt64 maxSequenceID)
        {
            matchDataAccess.CleanTransRule2(isLive, maxSequenceID);
        }
    }
}