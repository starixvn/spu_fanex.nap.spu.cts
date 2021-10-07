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

        Task<bool> ProcessRule(int batchSize, bool isLive);
    }

    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly string serviceName;
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly ISystemParameterServices systemParameterServices;
        private readonly IAppSettings appSettings;

        public MatchMonitorJobService(
            ISentryClient sentryClient,
            IMatchMonitorServices matchMonitorServices,
            ISystemParameterServices commonServices,
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.systemParameterServices = commonServices;
            this.appSettings = appSettings;
            this.serviceName = "MatchMonitor";
        }

        public Task<bool> InsertTicketDetail(int batchSize, bool isLive)
        {
            try
            {
                var lastScannedTransId = GetLastScannedTransId(isLive);

                var matchTrans = GetMatchTrans(lastScannedTransId, batchSize, isLive);

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

        public Task<bool> ProcessRule(int batchSize, bool isLive)
        {
            try
            {
                var matchMonitorRule = matchMonitorServices.GetMatchMonitorRule(isLive, batchSize);

                if (matchMonitorRule != null && matchMonitorRule.MaxTransId != 0)
                {
                    matchMonitorRule
                        .MatchStagingList
                        .AsParallel()
                        .WithDegreeOfParallelism(appSettings.MatchMonitorNumberOfThread)
                        .ForAll(x =>
                        {
                            var matchRuleCompleted = matchMonitorServices.InsertMatchMonitorRule(isLive, x.MatchId, x.TotalScore, x.BetTypeId, matchMonitorRule.MaxTransId);

                            if (matchRuleCompleted != null && matchRuleCompleted.Any())
                            {
                                try
                                {
                                    matchMonitorServices.UpdateMatchTransComplete(
                                                                       isLive,
                                                                       x.MatchId,
                                                                       x.TotalScore,
                                                                       x.BetTypeId,
                                                                       JsonConvert.SerializeObject(matchRuleCompleted),
                                                                       matchMonitorRule.MaxTransId);
                                }
                                catch (Exception e)
                                {
                                    var matchInfo = $"input: isLive {isLive} - MatchId {x.MatchId} - TotalScore {x.TotalScore} - Bettypeid {x.BetTypeId} - TranGroup { JsonConvert.SerializeObject(matchRuleCompleted)} - MaxTranId { matchMonitorRule.MaxTransId}";
                                    Utilities.LogSentryError("MatchMonitor", matchInfo, string.Empty, e, sentryClient);
                                    throw e;
                                }
                               
                            }

                            Thread.Sleep(MatchMonitor_SleepTimes);
                        });
                }

                matchMonitorServices.CleanMatchStaging(isLive, matchMonitorRule.MaxTransId);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }
        }

        #region Private Methods

        private Int64 GetLastScannedTransId(bool isLive)
        {
            var matchMonitorLastScannedTransIdLive = isLive ? SystemParameters.MatchMonitorLastScannedTrans : SystemParameters.MatchMonitorLastScannedTransNoneLive;

            var lastScannedTransId = systemParameterServices.GetLastScannedId(matchMonitorLastScannedTransIdLive);

            return lastScannedTransId;
        }

        private IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 lastScannedTransId, int batchSize, bool isLive)
        {
            var matchTrans = new List<TicketDetailEntity>();

            if (batchSize > 0)
            {
                matchTrans = matchMonitorServices.GetMatchTrans(lastScannedTransId, batchSize, isLive).ToList();
            }

            return matchTrans;
        }

        #endregion Private Methods
    }
}