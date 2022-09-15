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
        private readonly IArchiveHistoryCCBySportJobService archiveHistoryCCBySportJobService;
        private readonly IRollbackAssociationCustomerJobService rollbackAssociationCustomerJobService;

        public ArchiveDataController(IArchiveCustomerJobService archiveCustomerJobService
            , IArchiveAssociationCustomerJobService archiveAssociationCustomerJobService
            , IArchiveHistoryCCBySportJobService archiveHistoryCCBySportJobService
            , IRollbackAssociationCustomerJobService rollbackAssociationCustomerJobService)
        {
            this.archiveCustomerJobService = archiveCustomerJobService;
            this.archiveAssociationCustomerJobService = archiveAssociationCustomerJobService;
            this.archiveHistoryCCBySportJobService = archiveHistoryCCBySportJobService;
            this.rollbackAssociationCustomerJobService = rollbackAssociationCustomerJobService;
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

        [HttpPost("ArchiveHistoryCCBySport")]
        public async Task<bool> ArchiveHistoryCCBySportJobService()
        {
            return await this.archiveHistoryCCBySportJobService.ArchiveHistoryCCBySport();
        }
    }
}
