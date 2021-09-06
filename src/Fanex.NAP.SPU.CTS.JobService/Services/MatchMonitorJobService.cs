namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IMatchMonitorJobService
    {
        Task<bool> Execute(int batchSize, bool isLive);

        Task<bool> UpdateWriteLogSetting(string serviceName, bool isWriteLog);
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

        public Task<bool> Execute(int batchSize, bool isLive)
        {
            try
            {
                if (appSettings.MatchMonitorNumberOfThread > 0 && batchSize > 0)
                {
                    JobRun(batchSize, isLive);
                }
                else
                {
                    Utilities.LogSentryError(serviceName, "Invalid NumberOfThread, BatchSize", (appSettings.MatchMonitorNumberOfThread, batchSize), new NullReferenceException(), sentryClient);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> UpdateWriteLogSetting(string serviceName, bool isWriteLog)
        {
            try
            {
                bool result = appSettings.UpdateIsWriteLogSetting(serviceName, isWriteLog);

                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                sentryClient.CaptureException(e);
                return Task.FromResult(false);
            }
        }

        #region Private Methods

        private void JobRun(int batchSize, bool isLive)
        {
            Int64 lastScannedTransId = GetLastScannedTransId(isLive);

            var matchTrans = GetMatchTrans(lastScannedTransId, batchSize, isLive);

            if (matchTrans != null && matchTrans.Any())
            {
                InsertMatchMonitor(matchTrans, isLive);
            }

            matchMonitorServices.CleanMatchStaging(isLive);
        }

        private Int64 GetLastScannedTransId(bool isLive)
        {
            var matchMonitorLastScannedTransIdLive = isLive ? SystemParameters.MatchMonitorLastScannedTrans : SystemParameters.MatchMonitorLastScannedTransNoneLive;

            var lastScannedTransId = systemParameterServices.GetLastScannedId(matchMonitorLastScannedTransIdLive);

            Utilities.LogSentryInfo(serviceName, "Get LastScannedTransId", lastScannedTransId, appSettings.MatchMonitorIsWriteLog, sentryClient);

            return lastScannedTransId;
        }

        private IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 lastScannedTransId, int batchSize, bool isLive)
        {
            var matchTrans = new List<TicketDetailEntity>();

            if (batchSize > 0)
            {
                matchTrans = matchMonitorServices.GetMatchTrans(lastScannedTransId, batchSize, isLive).ToList();
            }

            Utilities.LogSentryInfo(serviceName, "Get MatchTrans", matchTrans, appSettings.MatchMonitorIsWriteLog, sentryClient);

            return matchTrans;
        }

        private void InsertMatchMonitor(IEnumerable<TicketDetailEntity> matchTrans, bool isLive)
        {
            if (matchTrans != null && matchTrans.Any())
            {
                var matchStaging = matchMonitorServices.InsertMatchMonitorStaging(JsonConvert.SerializeObject(matchTrans), isLive);

                if (matchStaging != null && matchStaging.Any())
                {
                    InsertParallelMatchRule(isLive, matchStaging);
                }               
            }
        }

        private void InsertParallelMatchRule(bool isLive, IEnumerable<MatchStagingModel> matchStaging)
        {
            try
            {
                matchStaging
                    .AsParallel()
                    .WithDegreeOfParallelism(appSettings.MatchMonitorNumberOfThread)
                    .ForAll(x =>
                    {
                        var matchRuleCompleted = matchMonitorServices.InsertMatchMonitorRule(isLive, x.MatchId, x.TotalScore, x.BetTypeId);

                        if (matchRuleCompleted != null && matchRuleCompleted.Any())
                        {
                            matchMonitorServices.UpdateMatchTransComplete(isLive, x.MatchId, x.TotalScore, x.BetTypeId, JsonConvert.SerializeObject(matchRuleCompleted));
                        }

                        Thread.Sleep(MatchMonitor_SleepTimes);
                    });
            }
            catch (AggregateException ex)
            {
                throw new AggregateException($"{serviceName} - Insert Match Monitor Rule failed.", ex);
            }
        }

        #endregion Private Methods
    }
}