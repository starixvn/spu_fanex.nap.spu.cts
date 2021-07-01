namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using Sentry;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/transform-job")]
    public class TransformJobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IAssociationByDeviceJobService associationByDeviceJobService;
        private readonly IAffectedEvidenceJobService affectedEvidenceJobService;

        public TransformJobController(ISentryClient sentryClient,
            IAssociationByDeviceJobService associationByDeviceJobService,
            IAffectedEvidenceJobService affectedEvidenceJobService)
        {
            this.sentryClient = sentryClient;
            this.associationByDeviceJobService = associationByDeviceJobService;
            this.affectedEvidenceJobService = affectedEvidenceJobService;
        }

        [HttpPost("transform-ass-by-device")]
        public async Task<bool> TransformAssociationByDevice([FromForm] int batchSize, [FromForm] int numberOfBatch)
        {
            return await associationByDeviceJobService.TransformAssociationByDevice(batchSize, numberOfBatch);
        }

        [HttpPost("affected-evidence")]
        public async Task<bool> TransformAffectedEvidence([FromForm] int batchSize)
        {
            return await affectedEvidenceJobService.Execute(batchSize);
        }
    }
}