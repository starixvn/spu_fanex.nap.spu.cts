namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Fanex.NAP.SPU.CTS.JobService.Services.ArchiveAssociationCustomer;
    using Fanex.NAP.SPU.CTS.JobService.Services.ArchiveData;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/ArchiveData")]
    public class ArchiveDataController : ControllerBase
    {
        private readonly IArchiveCustomerJobService archiveCustomerJobService;
        private readonly IArchiveAssociationCustomerJobService archiveAssociationCustomerJobService;
        private readonly IRollbackAssociationCustomerJobService rollbackAssociationCustomerJobService;
        private readonly IArchiveHistoryClassificationJobService archiveHistoryClassificationJobService;
        private readonly IArchiveHistoryClassificationBySportJobService archiveHistoryClassificationBySportJobService;
        private readonly IArchiveCustomerMissingJobService archiveCustomerMissingJobService;

        public ArchiveDataController(IArchiveCustomerJobService archiveCustomerJobService
            , IArchiveAssociationCustomerJobService archiveAssociationCustomerJobService
            , IRollbackAssociationCustomerJobService rollbackAssociationCustomerJobService
            , IArchiveHistoryClassificationJobService archiveHistoryClassificationJobService
            , IArchiveHistoryClassificationBySportJobService archiveHistoryClassificationBySportJobService
            , IArchiveCustomerMissingJobService archiveCustomerMissingJobService)
        {
            this.archiveCustomerJobService = archiveCustomerJobService;
            this.archiveAssociationCustomerJobService = archiveAssociationCustomerJobService;
            this.rollbackAssociationCustomerJobService = rollbackAssociationCustomerJobService;
            this.archiveHistoryClassificationJobService = archiveHistoryClassificationJobService;
            this.archiveHistoryClassificationBySportJobService = archiveHistoryClassificationBySportJobService;
            this.archiveCustomerMissingJobService = archiveCustomerMissingJobService;
        }

        [HttpPost("ArchiveCustomer")]
        public async Task<bool> ArchiveCustomer([FromForm] int batchSize)
        {
            return await archiveCustomerJobService.ArchiveCTSCustomer(batchSize);
        }

        [HttpPost("ArchiveCreditCustMissing")]
        public async Task<bool> ArchiveCreditCustMissing([FromForm] int batchSize)
        {
            return await archiveCustomerMissingJobService.ArchiveCreditCustomer(batchSize);
        }

        [HttpPost("ArchiveDepositCustMissing")]
        public async Task<bool> ArchiveDepositCustMissing([FromForm] int batchSize)
        {
            return await archiveCustomerMissingJobService.ArchiveDepositCustomer(batchSize);
        }

        [HttpPost("ArchiveAssociationCustomer")]
        public async Task<bool> ArchiveAssociationCustomer()
        {
            return await this.archiveAssociationCustomerJobService.Execute();
        }

        [HttpPost("RollbackAssociationCustomer")]
        public async Task<bool> RollbackAssociationCustomerJobService()
        {
            return await this.rollbackAssociationCustomerJobService.Execute();
        }

        [HttpPost("ArchiveHistoryClassification")]
        public async Task<bool> ArchiveHistoryClassification()
        {
            return await this.archiveHistoryClassificationJobService.ArchiveHistoryClassification();
        }

        [HttpPost("ArchiveHistoryClassificationBySport")]
        public async Task<bool> ArchiveHistoryClassificationBySportJobService()
        {
            return await this.archiveHistoryClassificationBySportJobService.ArchiveHistoryClassificationBySport();
        }
    }
}