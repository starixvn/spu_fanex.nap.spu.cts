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

        public JobController(ISentryClient sentryClient,
            IAccountJobService accountJobService,
            IMatchMonitorJobService matchMonitorJobService,
            IUserPermissionJobService userPermissionJobService)
        {
            this.sentryClient = sentryClient;
            this.accountJobService = accountJobService;
            this.matchMonitorJobService = matchMonitorJobService;
            this.userPermissionJobService = userPermissionJobService;
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

        //[HttpPost("user-permission")]
        //public async Task<bool> UserPermission([FromForm] bool isWriteLog)
        //{
        //    return await userPermissionJobService.UpdateWriteLogSetting(isWriteLog);
        //}
    }
}
