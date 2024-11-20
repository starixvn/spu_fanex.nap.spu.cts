namespace Fanex.NAP.SPU.CTS.Features.Notification
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.Notification.Criteria;
    using Fanex.NAP.SPU.CTS.Features.Notification.Entities;
    using Fanex.NAP.SPU.CTS.Features.Notification.Model;

    public interface IDBAMonitoringDataAccess
    {
        DBAMonitoringModel ScanDataMonitoring();
    }

    public class DBAMonitoringDataAccess : IDBAMonitoringDataAccess
    {
        private readonly IDynamicRepository _repository;

        public DBAMonitoringDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public DBAMonitoringModel ScanDataMonitoring()
        {
            var criteria = new DBAMonitoringCriteria();

            var result = _repository.Fetch<DBAMonitoringEntity>(criteria);

            return new DBAMonitoringModel
            {
                Data = result,
                Status = criteria.Status
            };
        }
    }
}
