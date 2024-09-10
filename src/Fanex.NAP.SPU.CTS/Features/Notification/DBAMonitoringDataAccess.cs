namespace Fanex.NAP.SPU.CTS.Features.Notification
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.Notification.Criteria;
    using Fanex.NAP.SPU.CTS.Features.Notification.Entities;
    using System.Collections.Generic;

    public interface IDBAMonitoringDataAccess
    {
        IEnumerable<DBAMonitoringEntity> ScanDataMonitoring();
    }

    public class DBAMonitoringDataAccess : IDBAMonitoringDataAccess
    {
        private readonly IDynamicRepository _repository;

        public DBAMonitoringDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public IEnumerable<DBAMonitoringEntity> ScanDataMonitoring()
        {
            var criteria = new DBAMonitoringCriteria();

            var result = _repository.Fetch<DBAMonitoringEntity>(criteria);

            return result;
        }
    }
}
