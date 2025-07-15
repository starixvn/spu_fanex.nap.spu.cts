namespace Fanex.NAP.SPU.CTS.JobService.Services.ArchiveData
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveCustomerJobService
    {
        Task<bool> ArchiveCTSCustomer(int batchSize);
    }

    public class ArchiveCustomerJobService : IArchiveCustomerJobService
    {
        private readonly IArchiveCustomerService archiveCustomerService;

        public ArchiveCustomerJobService(IArchiveCustomerService archiveCustomerService)
        {
            this.archiveCustomerService = archiveCustomerService;
        }

        public Task<bool> ArchiveCTSCustomer(int batchSize)
        {
            try
            {
                archiveCustomerService.ArchiveCTSCustomer(batchSize);
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
