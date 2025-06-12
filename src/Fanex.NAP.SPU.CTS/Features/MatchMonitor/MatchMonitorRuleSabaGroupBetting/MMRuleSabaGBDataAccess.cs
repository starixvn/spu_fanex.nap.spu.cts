namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting
{
    using System;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Criteria;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models;

    public interface IMMRuleSabaGBDataAccess
    {
        MatchMonitorRuleSabaGBModel GetMatchRuleSabaGB(bool isLive);

        MatchProcessRuleSabaGBModel ProcessRuleSabaGB(bool isLive, MatchStagingRuleSabaGBEntity matchInfo, string sequenceIDList, string groupDetection);

        void CompleteTransRuleSabaGB(MatchCompleteRuleSabaGBParameterModel para);

        void CleanTransRuleSabaGB(bool isLive, UInt64 maxSequenceID);
    }

    public class MMRuleSabaGBDataAccess : IMMRuleSabaGBDataAccess
    {
        private readonly IDynamicRepository _repository;

        public MMRuleSabaGBDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public MatchMonitorRuleSabaGBModel GetMatchRuleSabaGB(bool isLive)
        {
            var criteria = new MatchMonitorRuleSabaGBGetCriteria
            {
                IsLive = isLive
            };

            (var matchStagings, var ctsCustomers) = _repository.FetchMultiple<MatchStagingRuleSabaGBEntity, MatchMonitorMappingCustEntity>(criteria);

            return new MatchMonitorRuleSabaGBModel
            {
                MaxSequenceID = criteria.MaxSequenceID,
                MatchStagingList = criteria.MaxSequenceID.HasValue ? matchStagings : Enumerable.Empty<MatchStagingRuleSabaGBEntity>(),
                CTSCustomerList = criteria.MaxSequenceID.HasValue ? ctsCustomers : Enumerable.Empty<MatchMonitorMappingCustEntity>()
            };
        }

        public MatchProcessRuleSabaGBModel ProcessRuleSabaGB(bool isLive, MatchStagingRuleSabaGBEntity matchInfo, string sequenceIDList, string groupDetection)
        {
            var criteria = new MatchMonitorRuleSabaGBProcessCriteria
            {
                IsLive = isLive,
                MatchId = matchInfo.MatchId,
                ScoreDiff = matchInfo.ScoreDiff,
                BettypeId = matchInfo.BetTypeID,
                BetId = matchInfo.BetID,
                Betteam = matchInfo.BetTeam,
                SportType = matchInfo.SportType,
                SequenceIDList = sequenceIDList,
                CustGroup = groupDetection
            };

            var result = _repository.FetchMultiple<MatchCompleteRuleSabaGBEntity, MatchProcessRuleSabaGBInfoEntity>(criteria);

            return new MatchProcessRuleSabaGBModel
            {
                MatchInfo = matchInfo,
                CompleteMatches = result.FirstResultSet,
                ReprocessMatches = result.SecondResultSet
            };
        }

        public void CompleteTransRuleSabaGB(MatchCompleteRuleSabaGBParameterModel para)
        {
            var criteria = new MatchMonitorRuleSabaGBCompleteCriteria
            {
                IsLive = para.IsLive,
                BettypeId = para.MatchInfo.BetTypeID,
                BetID = para.MatchInfo.BetID,
                MatchId = para.MatchInfo.MatchId,
                ScoreDiff = para.MatchInfo.ScoreDiff,
                Betteam = para.MatchInfo.BetTeam,
                TransGroupJson = para.TransGroupJson,
                MaxSequenceID = para.MaxSequenceID
            };

            _repository.Execute(criteria);
        }

        public void CleanTransRuleSabaGB(bool isLive, UInt64 maxSequenceID)
        {
            var criteria = new MatchMonitorRuleSabaGBTransCleanCriteria
            {
                IsLive = isLive,
                MaxSequenceID = maxSequenceID
            };

            _repository.Execute(criteria);
        }
    }
}
