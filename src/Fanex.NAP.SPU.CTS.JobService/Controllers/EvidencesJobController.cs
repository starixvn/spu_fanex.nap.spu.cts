namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/evidencesjob")]
    public class EvidencesJobController : ControllerBase
    {
        private readonly IRemoveAccountEvidenceJobService removeAccountEvidenceJobService;


        public EvidencesJobController(
            IRemoveAccountEvidenceJobService removeAccountEvidenceJobService)
        {
            this.removeAccountEvidenceJobService = removeAccountEvidenceJobService;
        }

        [HttpPost("removeAccountEvidence")]
        public async Task<bool> RemoveAccountEvidences([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await this.removeAccountEvidenceJobService.Execute(batchExternalSize, batchInternalSize);
        }
    }
}