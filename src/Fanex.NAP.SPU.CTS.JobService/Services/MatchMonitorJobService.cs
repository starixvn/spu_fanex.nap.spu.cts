namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities.MatchMonitorRuleHedging;
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

        Task<bool> ProcessRuleHedging(bool isLive, int numberInternalOfThread, int numberExternalOfThread);

        Task<bool> ProcessRuleIrrigation(bool isLive, int numberOfThread);

        Task<bool> ProcessRuleArbitrage(bool isLive, int numberOfThread);
    }

    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly IAssociationDetectionService associationDetectionService;
        private readonly ISystemParameterServices systemParameterServices;
        private readonly string serviceName;

        public MatchMonitorJobService(
            ISentryClient sentryClient,
            IMatchMonitorServices matchMonitorServices,
            ISystemParameterServices commonServices,
            IAppSettings appSettings,
            IAssociationDetectionService associationDetectionService)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.systemParameterServices = commonServices;
            this.serviceName = "MatchMonitor Service";
            this.associationDetectionService = associationDetectionService;
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
                                var mmProcess = matchMonitorServices.ProcessRuleHedgingPharse(matchInfo, isLive, numberInternalOfThread).ToList();

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

        private IEnumerable<MatchCompleteRuleHedgingModel> MergeMatchCompleteRuleHedging(IEnumerable<MatchProcessRuleHedgingViewModel> matches)
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

        private void WriteLogSentryErrorCaseHedging(IEnumerable<MatchProcessRuleHedgingViewModel> matches)
        {
            var mmProcessPharse2 = matches.Where(x => x.PharseNumber == 2 && x.ReprocessMatches.IsNotNullOrEmpty());

            if (mmProcessPharse2.IsNotNullOrEmpty())
            {
                var logMessage = new LogMessage("MMHedging Error")
                    .AddMessage($"{serviceName}")
                    .AddMessage($"Data {JsonConvert.SerializeObject(mmProcessPharse2)} - { DateTime.Now}").ToString();

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
                var paras = new BetTypeSettingParaModel
                {
                    FunctionID = (int)BetTypeSettingsFunctionID.MMBetType,
                    BettypeIDList = null,
                    SportTypeIDList = null,
                    IsFilterMMDetection = true,
                    IsShowDetailsX = false,
                    Status = 1
                };

                var bettypeList = systemParameterServices.GetBetTypeSettings(paras).Select(x => new MatchBetTypeSettingModel
                {
                    SportTypeID = x.SportTypeID,
                    BetTypeID = x.BetTypeID,
                    BetChoiceType = x.BetChoiceType,
                    BetIDPattern = x.BetIDPattern
                });

                var betTypeInfos = JsonConvert.SerializeObject(bettypeList);

                return matchMonitorServices.GetMatchTrans(lastScannedSequenceID, batchSize, isLive, betTypeInfos);
            }

            return Enumerable.Empty<TicketDetailEntity>();
        }

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