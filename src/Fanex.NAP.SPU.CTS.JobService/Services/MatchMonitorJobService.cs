namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMatchMonitorJobService
    {
        Task<bool> Execute(int batchSize);

        Task<bool> UpdateWriteLogSetting(bool isWriteLog);
    }

    public class MatchMonitorJobService : IMatchMonitorJobService
    {
        private readonly string serviceName;
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly IAppSettings appSettings;

        public MatchMonitorJobService(
            ISentryClient sentryClient, 
            IMatchMonitorServices matchMonitorServices, 
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorServices = matchMonitorServices;
            this.appSettings = appSettings;
            this.serviceName = "MatchMonitor";
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
                    Utilities.LogSentryInfo(serviceName, "Invalid NumberOfThread, BatchSize", new { appSettings.MatchMonitorNumberOfThread, batchSize }, appSettings.MatchMonitorIsWriteLog, sentryClient);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> UpdateWriteLogSetting(bool isWriteLog)
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
        private void JobRun(int batchSize)
        {
            Int64 lastScannedTransId = GetLastScannedTransId();

            var matchTrans = GetMatchTrans(lastScannedTransId, batchSize);

            if(matchTrans!=null && matchTrans.Any())
            {
                InsertMatchMonitor(matchTrans);
            }
        }

        private Int64 GetLastScannedTransId()
        {
            var milestone = matchMonitorServices.GetLastScannedTransID();

            Utilities.LogSentryInfo(serviceName, "Get LastScannedTransId", milestone, appSettings.MatchMonitorIsWriteLog, sentryClient);

            return milestone.LastScannedTransId;
        }

        private IEnumerable<TicketDetailEntity> GetMatchTrans(Int64 lastScannedTransId, int batchSize)
        {
            var matchTrans = new List<TicketDetailEntity>();

            if (batchSize > 0)
            {
                matchTrans = matchMonitorServices.GetMatchTrans(lastScannedTransId, batchSize).ToList();
            }

            Utilities.LogSentryInfo(serviceName, "Get MatchTrans", matchTrans, appSettings.MatchMonitorIsWriteLog, sentryClient);       
            return matchTrans;
        }

        private void InsertMatchMonitor(IEnumerable<TicketDetailEntity> matchTrans)
        {
            if (matchTrans != null && matchTrans.Any())
            {
                var temGroupIDs = matchMonitorServices.InsertMatchMonitorStaging(JsonConvert.SerializeObject(matchTrans));
                Utilities.LogSentryInfo(serviceName, "Get transaction", matchTrans, appSettings.MatchMonitorIsWriteLog, sentryClient);

                if (temGroupIDs != null && temGroupIDs.Any())
                {
                    InsertMatchMonitorRule(temGroupIDs);
                    Utilities.LogSentryInfo(serviceName, "Insert Rule done", temGroupIDs, appSettings.MatchMonitorIsWriteLog, sentryClient);
                }

                matchMonitorServices.UpdateMatchTransComplete();
                Utilities.LogSentryInfo(serviceName, "Update milestone done", matchTrans.Select(c=>c.TransId).Max(), appSettings.MatchMonitorIsWriteLog, sentryClient);
            }
        }

        private void InsertMatchMonitorRule(IEnumerable<int> tempGroupIDs)
        {
            if (tempGroupIDs == null && !tempGroupIDs.Any())
            {
                return;
            }

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
                            RetryInsertMatchRule(tempGroupIDs.ToArray()[index++]);
                        });

                        tasks.Add(task);
                    }
                }

                DisposeTask(tasks);
            }

            Task.WaitAll(tasks.ToArray());
        }

        private void RetryInsertMatchRule(int tempGroupIDs)
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
                        Utilities.LogSentryEror(serviceName, "Insert Rule fail", tempGroupIDs, e, sentryClient);
                    }
                }
            }
        }

        private void DisposeTask(List<Task> tasks)
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
