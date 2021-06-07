namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
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
        private readonly ICommonJobServices commonJobServices;
        private readonly IAppSettings appSettings;

        public MatchMonitorJobService(
            ISentryClient sentryClient, 
            IMatchMonitorServices matchMonitorServices, 
            IAppSettings appSettings,
            ICommonJobServices commonJobServices)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.appSettings = appSettings;
            this.commonJobServices = commonJobServices;
        }

        public Task<bool> Execute(int batchSize)
        {
            try
            {
                if (appSettings.MatchMonitorNumberOfThread > 0 && batchSize > 0)
                {
                    JobRun(batchSize);
                }
                else
                {
                    commonJobServices.LogSentryInfo("Match-Monitor", "Invalid NumberOfThread/BatchSize", new { appSettings.MatchMonitorNumberOfThread, batchSize });
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        #region Private Methods
        private void JobRun(int batchSize)
        {
            var matchTrans = GetMatchTrans(GetMilestone(), batchSize);
            InsertMatchMonitor(matchTrans);
        }

        private SystemParameterEntity GetMilestone()
        {
            var milestone = matchMonitorServices.GetLastScannedTransID((int)Shares.Constants.SYSTEMPARAMETERS.MATCHMONITOR_LASTSCANNEDTRANS);
            commonJobServices.LogSentryInfo("Match-Monitor", "Get Milestone", milestone);

            return milestone;
        }

        private IEnumerable<TicketDetailEntity> GetMatchTrans(SystemParameterEntity milestone, int batchSize)
        {
            var matchTrans = new List<TicketDetailEntity>();

            if (!string.IsNullOrEmpty(milestone.ParameterValue))
            {
                matchTrans = matchMonitorServices.GetMatchTrans(Int64.Parse(milestone.ParameterValue), batchSize).ToList();
            }

            commonJobServices.LogSentryInfo("Match-Monitor", "Get transaction", matchTrans);
            return matchTrans;
        }

        private void InsertMatchMonitor(IEnumerable<TicketDetailEntity> matchTrans)
        {
            if (matchTrans != null && matchTrans.Any())
            {
                var temGroupIDs = matchMonitorServices.InsertMatchMonitorStaging(JsonConvert.SerializeObject(matchTrans));
                commonJobServices.LogSentryInfo("Match-Monitor", "Get transaction", matchTrans);

                if (temGroupIDs != null && temGroupIDs.Any())
                {
                    InsertMatchMonitorRule(temGroupIDs);
                    commonJobServices.LogSentryInfo("Match-Monitor", "Insert Rule done", temGroupIDs);
                }

                matchMonitorServices.UpdateMatchTransComplete();
                commonJobServices.LogSentryInfo("Match-Monitor", "Update milestone done", matchTrans.Select(c=>c.TransId).Max());
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
                    while (tasks.Count < appSettings.MatchMonitorNumberOfThread)
                    {
                        if (index < tempGroupIDs.Count())
                        {
                            var task = Task.Run(() =>
                            {
                                RetryInsertMatchMonitorRule(tempGroupIDs.ToArray()[index++]);
                            });

                            tasks.Add(task);
                        }
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
                        commonJobServices.LogSentryEror("Match-Monitor", "Insert Rule fail", tempGroupIDs, e);
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

        #endregion
    }
}
