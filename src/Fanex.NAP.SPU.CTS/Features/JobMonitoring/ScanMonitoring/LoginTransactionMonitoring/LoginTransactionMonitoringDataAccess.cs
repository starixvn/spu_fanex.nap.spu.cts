namespace Fanex.NAP.SPU.CTS.Features.MonitorJobs.CustomerJobMonitor
{
    using Fanex.Data.Repository;

    public interface ILoginTransactionMonitoringDataAccess
    {
        long GetLastLoginTransactionProcessMonitoring();
    }

    public class LoginTransactionMonitoringDataAccess : ILoginTransactionMonitoringDataAccess
    {
        private readonly IDynamicRepository repository;

        public LoginTransactionMonitoringDataAccess()
        {
            repository = new DynamicRepository();
        }

        public long GetLastLoginTransactionProcessMonitoring()
        {
            var criteria = new LastLoginProcessMonitoringCriteria();

            repository.Execute(criteria);

            return criteria.RecordNumber;
        }
    }
}