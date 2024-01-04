namespace Fanex.NAP.SPU.CTS.Features.MonitorJobs.CustomerJobMonitor
{
    using Fanex.NAP.SPU.CTS.Features.JobMonitoring.IncorrectDataMonitoring;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface ILoginTransactionMonitoringService
    {
        long GetLastLoginProcessMonitoring();

        void NotifyExcessiveLoginProcess(long excessiveNumber, int batchSize);
    }

    public class LoginTransactionMonitoringService : ILoginTransactionMonitoringService
    {
        private readonly ILoginTransactionMonitoringDataAccess loginTransactionMonitoringDataAccess;
        private readonly IIncorrectDataMonitoringService incorrectDataMonitoringService;

        public LoginTransactionMonitoringService(
                ILoginTransactionMonitoringDataAccess loginTransactionMonitoringDataAccess,
                IIncorrectDataMonitoringService incorrectDataMonitoringService)
        {
            this.loginTransactionMonitoringDataAccess = loginTransactionMonitoringDataAccess;
            this.incorrectDataMonitoringService = incorrectDataMonitoringService;
        }

        #region Monitor Last Login Transaction Process
        public long GetLastLoginProcessMonitoring()
        {
            return loginTransactionMonitoringDataAccess.GetLastLoginTransactionProcessMonitoring();
        }

        public void NotifyExcessiveLoginProcess(long excessiveNumber, int batchSize)
        {
            if (excessiveNumber > 0)
            {
                var data = new List<InsertMonitoringEntity> { new InsertMonitoringEntity { TrackingInfo = $"{excessiveNumber}" } };
                incorrectDataMonitoringService.InsertIncorrectDataMonitoring(data, JobMonitor_IssueType.DCSTransaction_LastLoginProcessStatus, batchSize);
            }
        }
        #endregion Monitor Last Login Transaction Process
    }
}