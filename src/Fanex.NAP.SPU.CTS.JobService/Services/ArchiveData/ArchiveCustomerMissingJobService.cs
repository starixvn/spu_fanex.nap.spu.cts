namespace Fanex.NAP.SPU.CTS.JobService.Services.ArchiveData
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveCustomerMissingJobService
    {
        Task<bool> ArchiveCreditCustomer(int batchSize);

        Task<bool> ArchiveDepositCustomer(int batchSize);
    }

    public class ArchiveCustomerMissingJobService : IArchiveCustomerMissingJobService
    {
        private readonly IArchiveCustomerMissingService archiveCustomerMissingService;

        public ArchiveCustomerMissingJobService(IArchiveCustomerMissingService archiveCustomerMissingService)
        {
            this.archiveCustomerMissingService = archiveCustomerMissingService;
        }

        public Task<bool> ArchiveCreditCustomer(int batchSize)
        {
            try
            {
                archiveCustomerMissingService.ArchiveCreditCustomerMissing(batchSize);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                      .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                      .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                      .ToString();

                logMessage.LogError();
                return Task.FromResult(false);
            }
        }

        public Task<bool> ArchiveDepositCustomer(int batchSize)
        {
            try
            {
                archiveCustomerMissingService.ArchiveDepositCustomerMissing(batchSize);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                      .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                      .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                      .ToString();

                logMessage.LogError();
                return Task.FromResult(false);
            }
        }
    }
}
