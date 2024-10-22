namespace Fanex.NAP.SPU.CTS.Features.JobMonitoring.IncorrectDataMonitoring
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IIncorrectDataMonitoringService
    {
        void InsertIncorrectDataMonitoring(IEnumerable<InsertMonitoringEntity> incorrectList, JobMonitor_IssueType issueType, int batchSize);
    }

    public class IncorrectDataMonitoringService : IIncorrectDataMonitoringService
{
        private readonly IIncorrectDataMonitoringDataAccess incorrectDataMonitoringDataAccess;

        public IncorrectDataMonitoringService(IIncorrectDataMonitoringDataAccess incorrectDataMonitoringDataAccess)
        {
            this.incorrectDataMonitoringDataAccess = incorrectDataMonitoringDataAccess;
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
    }
}