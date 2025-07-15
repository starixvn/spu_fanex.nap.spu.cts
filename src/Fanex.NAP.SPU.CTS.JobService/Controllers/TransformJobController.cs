namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/TransformJob")]
    public class TransformJobController : ControllerBase
    {
        private readonly IAssociationByDeviceJobService associationByDeviceJobService;
        private readonly IAffectedEvidenceJobService affectedEvidenceJobService;
        private readonly IAccountJobService accountJobService;
        private readonly IUserPermissionJobService userPermissionJobService;
        private readonly IUpdateLastLoginJobService updateLastLoginJobService;

        public TransformJobController(
            IAssociationByDeviceJobService associationByDeviceJobService,
            IAccountJobService accountJobService,
            IAffectedEvidenceJobService affectedEvidenceJobService, 
            IUserPermissionJobService userPermissionJobService,
            IUpdateLastLoginJobService updateLastLoginJobService)
        {
            this.associationByDeviceJobService = associationByDeviceJobService;
            this.affectedEvidenceJobService = affectedEvidenceJobService;
            this.accountJobService = accountJobService;
            this.userPermissionJobService = userPermissionJobService;
            this.updateLastLoginJobService = updateLastLoginJobService;
        }

        [HttpPost("TransformAssByDevice")]
        public async Task<bool> TransformAssociationByDevice([FromForm] int batchSize, [FromForm] int numberOfBatch)
        {            
            return await associationByDeviceJobService.TransformAssociationByDevice(batchSize, numberOfBatch);
        }

        [HttpPost("AffectedEvidence")]
        public async Task<bool> TransformAffectedEvidence()
        {
            return await affectedEvidenceJobService.Execute();
        }

        [HttpPost("TransformAccount")]
        public async Task<bool> Account([FromForm] int size, [FromForm] int numberOfBatch)
        {            
            return await accountJobService.Execute(size, numberOfBatch);
        }

        [HttpPost("SyncUserPermission")]
        public async Task<bool> UserPermission()
        {
            return await userPermissionJobService.SyncUserPermissionFromNAP();
        }

        /// <summary>
        /// This function is unused - will be removed later
        /// </summary>
        /// <returns>This function is unused</returns>
        [HttpPost("UpdateLastLoginTime")]
        public async Task<bool> UpdateLastLoginTime()
        {
            return await updateLastLoginJobService.UpdateLastLoginTime();
        }
    }
}