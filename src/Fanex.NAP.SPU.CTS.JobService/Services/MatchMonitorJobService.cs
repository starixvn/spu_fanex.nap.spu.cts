namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorHedging;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Models;

    public interface IMatchMonitorJobService
    {
        Task<bool> InsertTicketDetail(int batchSize, bool isLive);

        Task<bool> ProcessRuleGroupBetting(bool isLive, int numberOfThread, MatchDetectAssociationSettingModel assDetectSetting);

        Task<bool> ProcessRuleSabaGroupBetting(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleFixedGame(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleHedging(bool isLive, int numberInternalOfThread, int numberExternalOfThread);

        Task<bool> ProcessRuleIrrigation(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleArbitrage(bool isLive, int numberOfThread);
    }

    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly ISentryClient _sentryClient;
        private readonly IMatchMonitorServices _matchMonitorServices;

        private readonly IServiceProvider _serviceProvider;
        private readonly IMMRuleSabaGBService _ruleSabaGBService;
        private readonly IMatchMonitorRuleGroupBettingService _ruleGBService;
        private readonly IMatchMonitorRuleArbitrageService _ruleArbitrageService;
        private readonly IMatchMonitorRuleHedgingService _ruleHedgingService;

        private readonly string serviceName;

        public MatchMonitorJobService(
            ISentryClient sentryClient,
            IMatchMonitorServices matchMonitorServices,
            IServiceProvider serviceProvider,
            IMMRuleSabaGBService ruleSabaGBService,
            IMatchMonitorRuleGroupBettingService ruleGBService,
            IMatchMonitorRuleArbitrageService ruleArbitrageService,
            IMatchMonitorRuleHedgingService ruleHedgingService)
        {
            _sentryClient = sentryClient;
            _matchMonitorServices = matchMonitorServices;
            serviceName = "MatchMonitor Service";

            _serviceProvider = serviceProvider;
            _ruleSabaGBService = ruleSabaGBService;
            _ruleGBService = ruleGBService;
            _ruleArbitrageService = ruleArbitrageService;
            _ruleHedgingService = ruleHedgingService;
        }

        public Task<bool> InsertTicketDetail(int batchSize, bool isLive)
        {
            try
            {
                _serviceProvider.GetService<IMatchMonitorStagingService>()
                    .Start(batchSize, isLive)
                    .GetLastScannedSequenceID()
                    .GetMatchTrans()
                    .InsertMatchMonitorStagingWithParallel()
                    .UpdateLastScannedSequenceID();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        #region GroupBetting
        /// <summary>
        /// Process Rule for GroupBeting
        /// </summary>
        public Task<bool> ProcessRuleGroupBetting(bool isLive, int numberOfThread, MatchDetectAssociationSettingModel assDetectSetting)
        {
            try
            {
                var matchMonitorRule = _ruleGBService.GetMatchRuleGroupBetting(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID.HasValue)
                {
                    matchMonitorRule
                        .MatchStagingList?
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            _ruleGBService.GetInstanceProcessGroupBettingService()
                                .PrepareProcess(matchInfo, isLive, assDetectSetting)
                                .ProcessRound1()
                                .ProcessRound2()
                                .Complete(matchMonitorRule.MaxSequenceID.Value)
                                .WriteLogSentryIfError(nameof(ProcessRuleGroupBetting), _sentryClient);

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });

                    _ruleGBService.CleanTransRuleGroupBetting(isLive, matchMonitorRule.MaxSequenceID.Value);
                }

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Process Rule Saba Group Betting
        /// </summary>
        public Task<bool> ProcessRuleSabaGroupBetting(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = _ruleSabaGBService.GetMatchRuleSabaGB(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID.HasValue)
                {
                    matchMonitorRule.MatchStagingList?
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            _ruleSabaGBService.GetInstanceProcessSabaGBService()?
                                .PrepareProcess(matchInfo, isLive, matchMonitorRule.MappingCustDictionary)
                                .ProcessRound1()
                                .ProcessRound2()
                                .Complete(matchMonitorRule.MaxSequenceID.Value)
                                .WriteLogSentryIfError(nameof(ProcessRuleSabaGroupBetting), _sentryClient);

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });

                    _ruleSabaGBService.CleanTransRuleSabaGB(isLive, matchMonitorRule.MaxSequenceID.Value);
                }

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
        }
        #endregion

        /// <summary>
        /// Process Rule for Fixed Game
        /// </summary>
        public Task<bool> ProcessRuleFixedGame(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = _matchMonitorServices.GetMatchRuleFixedGame(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = _matchMonitorServices.ProcessRuleFixedGame(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                _matchMonitorServices.CompleteTransRuleFixedGame(new MatchCompleteRuleFixedGameParameterModel
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

                _matchMonitorServices.CleanTransRuleFixedGame(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
        }

        #region Hedging
        /// <summary>
        /// Process Rule for Hedging
        /// </summary>
        public Task<bool> ProcessRuleHedging(bool isLive, int numberInternalOfThread, int numberExternalOfThread)
        {
            try
            {
                var matchMonitorRule = _ruleHedgingService.GetMatchRuleHedging(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID.HasValue)
                {
                    matchMonitorRule
                        .MatchStagingList?
                        .AsParallel()
                        .WithDegreeOfParallelism(numberExternalOfThread)
                        .ForAll(matchInfo =>
                        {
                            _ruleHedgingService.GetInstanceProcessHedgingService()
                                .PrepareProcessWithThread(matchInfo, isLive, numberInternalOfThread)
                                .ProcessRound1()
                                .ProcessRound2()
                                .Complete(matchMonitorRule.MaxSequenceID.Value)
                                .WriteLogSentryIfError(nameof(ProcessRuleHedging), _sentryClient);

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });

                    _ruleHedgingService.CleanTransRuleHedging(isLive, matchMonitorRule.MaxSequenceID.Value);
                }

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
        }
        #endregion

        /// <summary>
        /// Process Rule for Irrigation
        /// </summary>
        /// <returns></returns>
        public Task<bool> ProcessRuleIrrigation(bool isLive, int numberOfThread)
        {
            try
            {
                var matchMonitorRule = _matchMonitorServices.GetMatchMonitorRuleIrrigation(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            var matchRuleCompleted = _matchMonitorServices.InsertMatchMonitorRuleIrrigation(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                _matchMonitorServices.UpdateMatchMonitorRuleIrrigationComplete(new MatchMonitorRuleIrrigationCompleteParaModel
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

                _matchMonitorServices.CleanTransRuleIrrigation(isLive, matchMonitorRule.MaxSequenceID);

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

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
                var matchMonitorRule = _ruleArbitrageService.GetMatchRuleArbitrage(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID.HasValue)
                {
                    matchMonitorRule.MatchStagingList?
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            _ruleArbitrageService
                                .GetInstanceProcessArbitrageService()
                                .PrepareProcess(matchInfo, isLive)
                                .ProcessRound1()
                                .ProcessRound2()
                                .Complete(matchMonitorRule.MaxSequenceID.Value)
                                .WriteLogSentryIfError(nameof(ProcessRuleArbitrage), _sentryClient);

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });

                    _ruleArbitrageService.CleanTransRuleArbitrage(isLive, matchMonitorRule.MaxSequenceID.Value);
                }

                return Task.FromResult(true);
            }
            catch (AggregateException ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }
        }
    }
}