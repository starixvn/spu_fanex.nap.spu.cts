namespace Fanex.NAP.SPU.CTS.Features.JobMonitoring.IncorrectDataMonitoring
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IIncorrectDataMonitoringService
    {
        void InsertIncorrectDataMonitoring(IEnumerable<InsertMonitoringEntity> incorrectList, JobMonitor_IssueType issueType, int batchSize);

        void InsertIncorrectDataMonitoring(IEnumerable<long> incorrectList, JobMonitor_IssueType issueType, int batchSize);
    }

    public class IncorrectDataMonitoringService : IIncorrectDataMonitoringService
{
        private readonly IIncorrectDataMonitoringDataAccess incorrectDataMonitoringDataAccess;
        private readonly ISentryClient sentryClient;

        public IncorrectDataMonitoringService(IIncorrectDataMonitoringDataAccess incorrectDataMonitoringDataAccess, ISentryClient sentryClient)
        {
            this.incorrectDataMonitoringDataAccess = incorrectDataMonitoringDataAccess;
            this.sentryClient = sentryClient;
        }

        public void InsertIncorrectDataMonitoring(IEnumerable<InsertMonitoringEntity> incorrectList, JobMonitor_IssueType issueType, int batchSize)
        {
            if (incorrectList != null && incorrectList.Any())
            {
                var dataChunks = incorrectList.Chunk(batchSize);

                foreach (var logDataBatch in dataChunks)
                {
                    var dataJson = JsonConvert.SerializeObject(logDataBatch);
                    incorrectDataMonitoringDataAccess.InsertIncorrectDataMonitoring(dataJson, issueType);
                }
            }
        }

        public void InsertIncorrectDataMonitoring(IEnumerable<long> incorrectList, JobMonitor_IssueType issueType, int batchSize)
        {
            if (incorrectList != null && incorrectList.Any())
            {
                var dataChunks = incorrectList.Chunk(batchSize);

                foreach (var logDataBatch in dataChunks)
                {
                    var dataJson = JsonConvert.SerializeObject(logDataBatch);
                    incorrectDataMonitoringDataAccess.InsertIncorrectDataMonitoring(dataJson, issueType);
                }
            }
        }
    }
}