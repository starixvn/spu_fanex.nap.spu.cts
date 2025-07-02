namespace Fanex.NAP.SPU.CTS.JobService.Services.ArchiveAssociationCustomer
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IArchiveAssociationCustomerJobService
    {
        Task<bool> Execute();
    }

    public class ArchiveAssociationCustomerJobService : IArchiveAssociationCustomerJobService
    {
        private readonly IArchiveAssociationCustomerService archiveAssociationCustomerService;
        private const string serviceName = "ArchiveAssociationCustomer";

        public ArchiveAssociationCustomerJobService(IArchiveAssociationCustomerService archiveAssociationCustomerService)
        {
            this.archiveAssociationCustomerService = archiveAssociationCustomerService;
        }

        public Task<bool> Execute()
        {
            try
            {
                ArchiveAssociationCustomer();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void ArchiveAssociationCustomer()
        {
            this.archiveAssociationCustomerService.ArchiveAssociationCustomer();
        }

    }
}
