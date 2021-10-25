namespace Fanex.NAP.SPU.CTS.JobService.Services.ArchiveAssociationCustomer
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRollbackAssociationCustomerJobService
    {
        Task<bool> Execute();
    }

    public class RollbackAssociationCustomerJobService : IRollbackAssociationCustomerJobService
    {
        private readonly IRollbackAssociationCustomerService rollbackAssociationCustomerService;
        private readonly ISentryClient sentryClient;
        private const string serviceName = "RollbackAssociationCustomer";

        public RollbackAssociationCustomerJobService(ISentryClient sentryClient, IRollbackAssociationCustomerService rollbackAssociationCustomerService)
        {
            this.sentryClient = sentryClient;
            this.rollbackAssociationCustomerService = rollbackAssociationCustomerService;
        }

        public Task<bool> Execute()
        {
            try
            {
                this.rollbackAssociationCustomerService.RollbackAssociationCustomer();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
