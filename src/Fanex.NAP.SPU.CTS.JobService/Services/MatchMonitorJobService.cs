namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleHedging;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging;
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

        Task<bool> ProcessRuleSabaGroupBetting(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleFixedGame(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleHedging(bool isLive, int numberInternalOfThread, int numberExternalOfThread);

        Task<bool> ProcessRuleIrrigation(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleArbitrage(bool isLive, int numberOfThread);
    }

    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly IAssociationDetectionService associationDetectionService;

        private readonly IServiceProvider _serviceProvider;
        private readonly IMMRuleSabaGBService _ruleSabaGBService;
        private readonly IMatchMonitorRuleArbitrageService _ruleArbitrageService;

        private readonly string serviceName;

        public MatchMonitorJobService(
            ISentryClient sentryClient,
            IMatchMonitorServices matchMonitorServices,
            IAssociationDetectionService associationDetectionService,
            IServiceProvider serviceProvider,
            IMMRuleSabaGBService ruleSabaGBService,
            IMatchMonitorRuleArbitrageService ruleArbitrageService)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.serviceName = "MatchMonitor Service";
            this.associationDetectionService = associationDetectionService;

            _serviceProvider = serviceProvider;
            _ruleSabaGBService = ruleSabaGBService;
            _ruleArbitrageService = ruleArbitrageService;
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
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        #region GroupBetting
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
                            var prepareData = PrepareDataProcessRuleGroupBetting(isLive, matchMonitorRule.MaxSequenceID, matchInfo);

                            var matchRuleCompleted = matchMonitorServices.ProcessRuleGroupBetting(prepareData);

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
                                .WriteLogSentryIfError(nameof(ProcessRuleSabaGroupBetting), sentryClient);

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });

                    _ruleSabaGBService.CleanTransRuleSabaGB(isLive, matchMonitorRule.MaxSequenceID.Value);
                }

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
        #endregion

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

        #region Hedging
        /// <summary>
        /// Process Rule for Hedging
        /// </summary>
        public Task<bool> ProcessRuleHedging(bool isLive, int numberInternalOfThread, int numberExternalOfThread)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchRuleHedging(isLive);

                if (matchMonitorRule != null && matchMonitorRule.MaxSequenceID != null)
                {
                    if (matchMonitorRule.MatchStagingList != null && matchMonitorRule.MatchStagingList.Any())
                    {
                        matchMonitorRule
                            .MatchStagingList
                            .AsParallel()
                            .WithDegreeOfParallelism(numberExternalOfThread)
                            .ForAll(matchInfo =>
                            {
                                var mmProcess = matchMonitorServices.ProcessRuleHedgingWithRound(matchInfo, isLive, numberInternalOfThread).ToList();

                                if (mmProcess.IsNotNullOrEmpty())
                                {
                                    matchMonitorServices.CompleteTransRuleHedging(new MatchCompleteRuleHedgingParameterModel
                                    {
                                        IsLive = isLive,
                                        MatchInfo = matchInfo,
                                        TransGroupJson = JsonConvert.SerializeObject(MergeMatchCompleteRuleHedging(mmProcess)),
                                        MaxSequenceID = matchMonitorRule.MaxSequenceID.Value
                                    });

                                    WriteLogSentryErrorCaseHedging(mmProcess);
                                }

                                Thread.Sleep(MatchMonitor_SleepTimes);
                            });
                    }

                    matchMonitorServices.CleanTransRuleHedging(isLive, matchMonitorRule.MaxSequenceID.Value);
                }

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

        private IEnumerable<MatchCompleteRuleHedgingEntity> MergeMatchCompleteRuleHedging(IEnumerable<MatchProcessRuleHedgingModel> matches)
        {
            return matches
                .SelectMany(x => x.CompleteMatches)
                .GroupBy(item => item.TransIDList)
                .Select((group, index) =>
                {
                    var result = group.First();
                    result.GroupId = index + 1;
                    return result;
                });
        }

        private void WriteLogSentryErrorCaseHedging(IEnumerable<MatchProcessRuleHedgingModel> matches)
        {
            var mmProcessR2 = matches.Where(x => x.RoundNumber == 2 && x.ReprocessMatches.IsNotNullOrEmpty());

            if (mmProcessR2.IsNotNullOrEmpty())
            {
                var logMessage = new LogMessage("MMHedging Error")
                    .AddMessage($"{serviceName}")
                    .AddMessage($"Data {JsonConvert.SerializeObject(mmProcessR2)} - { DateTime.Now}").ToString();

                sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);
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
                                .Start(matchInfo, isLive)
                                .ProcessRound1()
                                .ProcessRound2()
                                .Complete(matchMonitorRule.MaxSequenceID.Value)
                                .WriteLogSentryIfError(nameof(ProcessRuleArbitrage), sentryClient);

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });

                    _ruleArbitrageService.CleanTransRuleArbitrage(isLive, matchMonitorRule.MaxSequenceID.Value);
                }

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
        private MMPrepareDataProcessRuleGBModel PrepareDataProcessRuleGroupBetting(bool isLive, UInt64 maxTransId, MatchStagingRuleGroupBettingModel matchinfo)
        {
            string assByShared3MatchesLast7Days = null;

            if (matchinfo.SportType == (int)MatchMonitor_SportType.Basketball
                || matchinfo.SportType == (int)MatchMonitor_SportType.Esport)
            {
                var assBySharedMatches = this.associationDetectionService.DetectAssociationBySharedMatches(matchinfo.CustIDList);

                if (assBySharedMatches != null && assBySharedMatches.Any())
                {
                    assByShared3MatchesLast7Days = JsonConvert.SerializeObject(assBySharedMatches);
                }
            }

            return new MMPrepareDataProcessRuleGBModel
            {
                IsLive = isLive,
                MaxSequenceID = maxTransId,
                MatchInfo = matchinfo,
                JsonSharedMatches = assByShared3MatchesLast7Days
            };
        }
        #endregion Private Methods
    }
}