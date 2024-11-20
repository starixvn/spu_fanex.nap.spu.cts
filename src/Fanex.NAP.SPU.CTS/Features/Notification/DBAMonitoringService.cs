namespace Fanex.NAP.SPU.CTS.Features.Notification
{
    using Fanex.NAP.SPU.CTS.Features.Notification.Model;

    public interface IDBAMonitoringService
    {
        DBAMonitoringModel ScanDataMonitoring();
    }

    public class DBAMonitoringService : IDBAMonitoringService
    {
        private readonly IDBAMonitoringDataAccess _dataAccess;

        public DBAMonitoringService(IDBAMonitoringDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public DBAMonitoringModel ScanDataMonitoring()
        {
            return _dataAccess.ScanDataMonitoring();
        }
    }
}
