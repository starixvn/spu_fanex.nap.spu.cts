namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MonitorJobs.CustomerJobMonitor;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Threading.Tasks;

    public interface ILoginTransactionMonitoringJobService
    {
        Task<bool> LastLoginProcessMonitoring(int thresHold);
    }

    public class LoginTransactionMonitoringJobService : ILoginTransactionMonitoringJobService
    {
        private readonly ILoginTransactionMonitoringService loginTransactionMonitoringService;
        private readonly string serviceName;

        public LoginTransactionMonitoringJobService(
            ILoginTransactionMonitoringService loginTransactionMonitoringService,
            IAppSettings appSettings)
        {
            this.loginTransactionMonitoringService = loginTransactionMonitoringService;
            this.serviceName = "LoginTransactionMonitoring Service";
        }

        public Task<bool> LastLoginProcessMonitoring(int thresHold)
        {
            try
            {
                var records = loginTransactionMonitoringService.GetLastLoginProcessMonitoring();

                if (records > thresHold)
                {
                    loginTransactionMonitoringService.NotifyExcessiveLoginProcess(records, 500, thresHold);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}