namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer;
    using Fanex.NAP.SPU.CTS.JobService.Services.ArchiveData;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/ArchiveData")]
    public class ArchiveDataController : ControllerBase
    {
        private readonly IArchiveCustomerJobService archiveCustomerJobService;

        public ArchiveDataController(IArchiveCustomerJobService archiveCustomerJobService)
        {
            this.archiveCustomerJobService = archiveCustomerJobService;
        }

        [HttpPost("ArchiveCustomer")]
        public async Task<bool> ArchiveCustomer([FromForm] int batchSize)
        {
            return await archiveCustomerJobService.ArchiveCTSCustomer(batchSize);
        }
    }
}
