namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Shares;
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

        Task<bool> ProcessRuleGroupBetting(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleFixedGame(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleHedging(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleIrrigation(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleArbitrage(bool isLive, int numberOfThread);
    }

    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly ISystemParameterServices systemParameterServices;
        private readonly string serviceName;

        public MatchMonitorJobService(
            ISentryClient sentryClient,
            IMatchMonitorServices matchMonitorServices,
            ISystemParameterServices commonServices,
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.systemParameterServices = commonServices;
            this.serviceName = "MatchMonitor Service";
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
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Process Rule for GroupBeting
        /// </summary>
        public Task<bool> ProcessRuleGroupBetting(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchRuleGroupBetting(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = matchMonitorServices.ProcessRuleGroupBetting(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                matchMonitorServices.CompleteTransRuleGroupBetting(new MatchCompleteRuleGroupBettingParameterModel
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

                matchMonitorServices.CleanTransRuleGroupBetting(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Process Rule for Fixed Game
        /// </summary>
        public Task<bool> ProcessRuleFixedGame(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchRuleFixedGame(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = matchMonitorServices.ProcessRuleFixedGame(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                matchMonitorServices.CompleteTransRuleFixedGame(new MatchCompleteRuleFixedGameParameterModel
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

                matchMonitorServices.CleanTransRuleFixedGame(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Process Rule for Hedging
        /// </summary>
        public Task<bool> ProcessRuleHedging(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchRuleHedging(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = matchMonitorServices.ProcessRuleHedging(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                matchMonitorServices.CompleteTransRuleHedging(new MatchCompleteRuleHedgingParameterModel
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

                matchMonitorServices.CleanTransRuleHedging(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Process Rule for Irrigation
        /// </summary>
        /// <returns></returns>
        public Task<bool> ProcessRuleIrrigation(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchMonitorRuleIrrigation(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = matchMonitorServices.InsertMatchMonitorRuleIrrigation(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                matchMonitorServices.UpdateMatchMonitorRuleIrrigationComplete(new MatchMonitorRuleIrrigationCompleteParaModel
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

                matchMonitorServices.CleanTransRuleIrrigation(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Process Rule for Arbitrage
        /// </summary>
        public Task<bool> ProcessRuleArbitrage(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchRuleArbitrage(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = matchMonitorServices.ProcessRuleArbitrage(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                matchMonitorServices.CompleteTransRuleArbitrage(new MatchCompleteRuleArbitrageParameterModel
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

                matchMonitorServices.CleanTransRuleArbitrage(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);
                
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

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