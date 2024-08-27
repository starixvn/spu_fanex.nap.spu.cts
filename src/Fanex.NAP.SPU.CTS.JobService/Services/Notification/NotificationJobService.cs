namespace Fanex.NAP.SPU.CTS.JobService.Services.Notification
{
    using Fanex.NAP.SPU.CTS.Features.Notification;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface INotificationJobService
    {
        Task<bool> ScanDBAMonitoring();
    }

    public class NotificationJobService : INotificationJobService
    {
        private readonly ISentryClient _sentryClient;
        private readonly IDBAMonitoringService _dbaMonitoringService;
        private readonly IAppSettings _appSettings;
        private readonly string _serviceName;

        public NotificationJobService(IDBAMonitoringService dbaMonitoringService, ISentryClient sentryClient, IAppSettings appSettings)
        {
            _dbaMonitoringService = dbaMonitoringService;
            _sentryClient = sentryClient;
            _appSettings = appSettings;
            _serviceName = "Notification Job Service";
        }

        public async Task<bool> ScanDBAMonitoring()
        {
            try
            {
                var scanData = _dbaMonitoringService.ScanDataMonitoring();

                if (scanData.IsNotNullOrEmpty())
                {
                    scanData.Chunk(5).ToList().ForEach(item =>
                    {
                        Utilities.NotifyTelegramMessage(item.BuildMessage(), nameof(ScanDBAMonitoring), _appSettings.EvironmentName, _appSettings.EvironmentName == "LOCAL" ? Telegram_GroupId_Local : Telegram_Notification_DBAMonitoring);
                    });
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return false;
            }

            return true;
        }
    }
}
