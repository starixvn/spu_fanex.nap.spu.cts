namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IMatchMonitorJobService
    {
        Task<bool> InsertTicketDetail(int batchSize, bool isLive);

        Task<bool> ProcessRule01(int batchSize, bool isLive, int numberOfThread);

        Task<bool> ProcessRule02(bool isLive, int numberOfThread);
    }

    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly ISystemParameterServices systemParameterServices;

        public MatchMonitorJobService(
            ISentryClient sentryClient,
            IMatchMonitorServices matchMonitorServices,
            ISystemParameterServices commonServices,
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.systemParameterServices = commonServices;
        }

        public Task<bool> InsertTicketDetail(int batchSize, bool isLive)
        {
            try
            {
                var lastScannedSequenceID = GetLastScannedSequenceID(isLive);

                var matchTrans = GetMatchTrans(lastScannedSequenceID, batchSize, isLive);

                if (matchTrans != null && matchTrans.Any())
                {
                    matchMonitorServices.InsertMatchMonitorStaging(JsonConvert.SerializeObject(matchTrans), isLive);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Process Rule for GroupBeting, Hedging, Arbitrage
        /// </summary>
        public Task<bool> ProcessRule01(int batchSize, bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchMonitorRule(isLive, batchSize);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = matchMonitorServices.InsertMatchMonitorRule(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                matchMonitorServices.UpdateMatchTransComplete(new MatchMonitorRuleCompleteParaModel
                                {
                                    IsLive = isLive,
                                    MatchInfo = matchInfo,
                                    TransGroupJson = JsonConvert.SerializeObject(matchRuleCompleted),
                                    MaxSequenceID = matchMonitorRule.MaxSequenceID
                                });
                            }

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });
                }

                matchMonitorServices.CleanMatchStaging(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Process Rule for Fixed Game
        /// </summary>
        public Task<bool> ProcessRule02(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule2 = matchMonitorServices.GetMatchMonitorRule2(isLive);

                if (matchMonitorRule2 != null && matchMonitorRule2.MaxTransId != 0)
                {
                    matchMonitorRule2
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = matchMonitorServices.InsertMatchMonitorRule2(isLive, matchMonitorRule2.MaxTransId, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                matchMonitorServices.UpdateMatchMonitorRule2Complete(new MatchMonitorRule2CompleteParaModel
                                {
                                    IsLive = isLive,
                                    MatchInfo = matchInfo,
                                    TransGroupJson = JsonConvert.SerializeObject(matchRuleCompleted),
                                    MaxTransId = matchMonitorRule2.MaxTransId
                                });
                            }

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });
                }

                matchMonitorServices.CleanTransRule2(isLive, matchMonitorRule2.MaxTransId);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }
        }

        #region Private Methods

        private Int64 GetLastScannedSequenceID(bool isLive)
        {
            var matchMonitorLastScannedSequenceIDLive = isLive ? SystemParameters.MatchMonitorLastScannedTrans : SystemParameters.MatchMonitorLastScannedTransNoneLive;

            return systemParameterServices.GetLastScannedId(matchMonitorLastScannedSequenceIDLive);
        }

        private IEnumerable<TicketDetailEntity> GetMatchTrans(long lastScannedSequenceID, int batchSize, bool isLive)
        {
            if (batchSize > 0)
            {
                return matchMonitorServices.GetMatchTrans(lastScannedSequenceID, batchSize, isLive);
            }

            return Enumerable.Empty<TicketDetailEntity>(); ;
        }

        #endregion Private Methods
    }
}