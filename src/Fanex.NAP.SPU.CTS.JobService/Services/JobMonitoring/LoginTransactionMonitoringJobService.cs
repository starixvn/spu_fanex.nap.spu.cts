namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.MonitorJobs.CustomerJobMonitor;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface ILoginTransactionMonitoringJobService
    {
        Task<bool> LastLoginProcessMonitoring(int thresHold);
    }

    public class LoginTransactionMonitoringJobService : ILoginTransactionMonitoringJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly ILoginTransactionMonitoringService loginTransactionMonitoringService;
        private readonly string serviceName;

        public LoginTransactionMonitoringJobService(
            ISentryClient sentryClient,
            ILoginTransactionMonitoringService loginTransactionMonitoringService,
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
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
                    loginTransactionMonitoringService.NotifyExcessiveLoginProcess(records, 500);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}