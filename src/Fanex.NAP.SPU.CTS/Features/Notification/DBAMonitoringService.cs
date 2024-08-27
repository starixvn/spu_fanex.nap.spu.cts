using Fanex.NAP.SPU.CTS.Features.Notification.Entities;
using System.Collections.Generic;

namespace Fanex.NAP.SPU.CTS.Features.Notification
{
    public interface IDBAMonitoringService
    {
        IEnumerable<DBAMonitoringEntity> ScanDataMonitoring();
    }

    public class DBAMonitoringService : IDBAMonitoringService
    {
        private readonly IDBAMonitoringDataAccess _dataAccess;

        public DBAMonitoringService(IDBAMonitoringDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<DBAMonitoringEntity> ScanDataMonitoring()
        {
            return _dataAccess.ScanDataMonitoring();
        }
    }
}
