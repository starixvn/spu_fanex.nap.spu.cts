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
        private readonly IArchiveCustClassificationHistoryJobService archiveCustClassHistoryJobService;
        private readonly IArchiveHistoryCCBySportJobService archiveHistoryCCBySportJobService;

        public ArchiveDataController(IArchiveCustomerJobService archiveCustomerJobService
            , IArchiveAssociationCustomerJobService archiveAssociationCustomerJobService
            , IRollbackAssociationCustomerJobService rollbackAssociationCustomerJobService
            , IArchiveCustClassificationHistoryJobService archiveCustClassHistoryJobService
            , IArchiveHistoryCCBySportJobService archiveHistoryCCBySportJobService)
        {
            this.archiveCustomerJobService = archiveCustomerJobService;
            this.archiveAssociationCustomerJobService = archiveAssociationCustomerJobService;
            this.rollbackAssociationCustomerJobService = rollbackAssociationCustomerJobService;
            this.archiveCustClassHistoryJobService = archiveCustClassHistoryJobService;
            this.archiveHistoryCCBySportJobService = archiveHistoryCCBySportJobService;
        }

        [HttpPost("ArchiveCustomer")]
        public async Task<bool> ArchiveCustomer([FromForm] int batchSize)
        {
            return await archiveCustomerJobService.ArchiveCTSCustomer(batchSize);
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

        [HttpPost("CleanUpUnChangedCCHistory")]
        public async Task<bool> CleanUpUnchangedCustClassificationHistory()
        {
            return await this.archiveCustClassHistoryJobService.CleanUpUnChangedCustClassHistory();
        }

        [HttpPost("ArchiveChangedCCHistory")]
        public async Task<bool> ArchiveChangedCustClassificationHistory()
        {
            return await this.archiveCustClassHistoryJobService.ArchiveChangedCustClassHistory();
        }

        [HttpPost("ArchiveCCHistorBySport")]
        public async Task<bool> ArchiveHistoryCCBySportJobService()
        {
            return await this.archiveHistoryCCBySportJobService.ArchiveHistoryCCBySport();
        }
    }
}