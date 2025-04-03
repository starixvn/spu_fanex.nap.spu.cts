namespace Fanex.NAP.SPU.CTS.JobService.Services.Notification
{
    using Fanex.NAP.SPU.CTS.Features.Notification;
    using Fanex.NAP.SPU.CTS.Features.Notification.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface INotificationJobService
    {
        Task<bool> ScanDataConsistencyMonitoring();
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
            _serviceName = "DataConsistencyMonitoring";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1449:Culture should be specified for \"string\" operations", Justification = "<Pending>")]
        public Task<bool> ScanDataConsistencyMonitoring()
        {
            try
            {
                var timeSleepArr = new[] { 2, 4, 6, 8, 10 };
                const int retryTime = 5;
                var retryCount = 0;

                do
                {
                    if (retryCount > 0)
                    {
                        Thread.Sleep(timeSleepArr[retryCount - 1] * 1000 * 60);
                    }

                    var scanData = _dbaMonitoringService.ScanDataMonitoring();

                    if ((scanData.Status.IsNotNullOrEmpty() && scanData.Status.ToUpper().Equals(Consts.STOP)) ||
                        retryCount == retryTime)
                    {
                        var message = $"CTS_DC_DataConsistency_CustomerCategory_Get, Status = {scanData.Status ?? "NULL"}";
                        message += BuildMessage(scanData.Data);

                        Utilities.NotifyChannelMessage(
                            message,
                            _serviceName,
                            _appSettings.EvironmentName,
                            _appSettings.NotificationChannelConfig.DBAMonitoringGroupChannelId,
                            isInfo: scanData.Data.IsNullOrEmpty());

                        return Task.FromResult(true);
                    }

                    retryCount++;
                } while (retryCount <= retryTime);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private string BuildMessage(IEnumerable<DBAMonitoringEntity> list)
        {
            var messageArr = list?.Select(x =>
            {
                var text = "\r\n";
                text += $"\n 🆘 {x.RuleMessage} \n";
                text += $"\n {nameof(DBAMonitoringEntity.LogInfo)} {x.LogInfo} \n";
                return text;
            });

            return messageArr.IsNotNullOrEmpty() ?
                    string.Join("\r\n", messageArr) :
                    "\r\n✅ DATA IS PERFECT!";
        }
    }
}
