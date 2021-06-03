namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMatchMonitorJobService
    {
        Task<bool> Execute(int batchSize);

    }
    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly IAppSettings appSettings;

        public MatchMonitorJobService(ISentryClient sentryClient, IMatchMonitorServices matchMonitorServices, IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.appSettings = appSettings;
        }

        public Task<bool> Execute(int batchSize)
        {
            try
            {
                if (appSettings.NumberOfThread > 0 && batchSize > 0)
                {
                    JobRun(batchSize);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun(int batchSize)
        {
            var systemParameter = matchMonitorServices.GetLastScannedTransID((int)Shares.Constants.SYSTEMPARAMETERS.MATCHMONITOR_LASTSCANNEDTRANS);

            if (!string.IsNullOrEmpty(systemParameter.ParameterValue))
            {
                var matchTrans = matchMonitorServices.GetMatchTrans(Int64.Parse(systemParameter.ParameterValue), batchSize);

                if (matchTrans != null && matchTrans.Any())
                {
                    var temGroupIDs = matchMonitorServices.InsertMatchMonitorStaging(JsonConvert.SerializeObject(matchTrans));

                    if (temGroupIDs != null && temGroupIDs.Any())
                    {
                        InsertMatchMonitorRule(temGroupIDs);
                    }

                    matchMonitorServices.UpdateMatchTransComplete();
                }
            }
        }

        private async void InsertMatchMonitorRule(IEnumerable<int> tempGroupIDs)
        {
            if (tempGroupIDs != null && tempGroupIDs.Any())
            {
                var tasks = new List<Task>();
                var index = 0;

                while (index < tempGroupIDs.Count())
                {
                    while (tasks.Count < appSettings.NumberOfThread)
                    {
                        var task = Task.Run(() =>
                        {
                            RetryInsertMatchMonitorRule(tempGroupIDs.ToArray()[index++]);
                        });

                        tasks.Add(task);
                    }

                    DisposeCompleteTask(tasks);
                }

                await Task.WhenAll(tasks.ToArray());
            }
        }

        private void RetryInsertMatchMonitorRule(int tempGroupIDs)
        {
            int numberOfTimes = 3;

            while (numberOfTimes > 0)
            {
                try
                {
                    matchMonitorServices.InsertMatchMonitorRule(tempGroupIDs);
                    numberOfTimes = 0;
                }
                catch (Exception e)
                {
                    numberOfTimes--;

                    if (numberOfTimes == 0)
                    {
                        throw new Exception(e.Message, e);
                    }
                }
            }
        }

        private void DisposeCompleteTask(List<Task> tasks)
        {
            List<Task> completedTasks = new List<Task>();

            tasks.ForEach(task =>
            {
                if (task.IsCompleted)
                {
                    task.Dispose();
                    completedTasks.Add(task);
                }
            });

            completedTasks.ForEach(i =>
            {
                tasks.Remove(i);
            });
        }
    }
}
