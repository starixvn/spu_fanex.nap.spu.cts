namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IAccountJobService accountJobService;
        private readonly IMatchMonitorJobService matchMonitorJobService;
        private readonly IUserPermissionJobService userPermissionJobService;
        private readonly IProblemAccountJobService problemAccountInsertJobService;
        private readonly IUpdateLastLoginJobService updateLastLoginJobService;
        private readonly INormalAccountJobService normalAccountInsertJobService;
        private readonly IAssociationByDeviceJobService associationByDeviceJobService;
        private readonly IAffectedEvidenceJobService affectedEvidenceJobService;


        public JobController(ISentryClient sentryClient,
            IAccountJobService accountJobService,
            IMatchMonitorJobService matchMonitorJobService,
            IUserPermissionJobService userPermissionJobService,
            IUpdateLastLoginJobService updateLastLoginJobService,
            IProblemAccountJobService problemAccountInsertJobService,
            INormalAccountJobService normalAccountInsertJobService,
            IAssociationByDeviceJobService associationByDeviceJobService,
            IAffectedEvidenceJobService affectedEvidenceJobService)
        {
            this.sentryClient = sentryClient;
            this.accountJobService = accountJobService;
            this.matchMonitorJobService = matchMonitorJobService;
            this.userPermissionJobService = userPermissionJobService;
            this.problemAccountInsertJobService = problemAccountInsertJobService;
            this.updateLastLoginJobService = updateLastLoginJobService;
            this.normalAccountInsertJobService = normalAccountInsertJobService;
            this.associationByDeviceJobService = associationByDeviceJobService;
            this.affectedEvidenceJobService = affectedEvidenceJobService;
        }

        [HttpPost("transformaccount")]
        public async Task<bool> Account(string jobName, [FromForm] int size, [FromForm] int numberOfBatch)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.accountJobService.Execute(size, numberOfBatch);
        }

        [HttpPost("matchmonitor")]
        public async Task<bool> MatchMonitor([FromForm] int size)
        {
            sentryClient.CaptureMessage($"Match Monitor Service is running at {DateTime.Now}");

            return await this.matchMonitorJobService.Execute(size);
        }

        [HttpPost("matchmonitor_updatewritelog")]
        public async Task<bool> UpdateMatchMonitorWriteLog([FromForm] bool isWriteLog)
        {
            return await this.matchMonitorJobService.UpdateWriteLogSetting(isWriteLog);
        }

        [HttpPost("sync-user-permission")]
        public async Task<bool> UserPermission()
        {
            return await userPermissionJobService.SyncUserPermissionFromNAP();
        }

        [HttpPost("update-last-login")]
        public async Task<bool> UpdateLastLoginTime()
        {
            return await updateLastLoginJobService.UpdateLastLoginTime();
        }

        [HttpPost("problemAccountInsert")]
        public async Task<bool> InsertProblemAccount([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await this.problemAccountInsertJobService.Execute(batchExternalSize, batchInternalSize);
        }

        [HttpPost("normalAccountInsert")]
        public async Task<bool> InsertNormalAccount([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await this.normalAccountInsertJobService.Execute(batchExternalSize, batchInternalSize);
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
