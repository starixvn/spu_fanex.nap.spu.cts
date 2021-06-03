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

        public JobController(ISentryClient sentryClient,
            IAccountJobService accountJobService,
            IMatchMonitorJobService matchMonitorJobService)
        {
            this.sentryClient = sentryClient;
            this.accountJobService = accountJobService;
            this.matchMonitorJobService = matchMonitorJobService;
        }

        [HttpPost("transformaccount")]
        public async Task<bool> Account(string jobName, int size, int numberOfBatch)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.accountJobService.Execute(size, numberOfBatch);                        
        }

        [HttpPost("matchmonitor")]
        public async Task<bool> MatchMonitor(int size)
        {
            sentryClient.CaptureMessage($"Match Monitor Service is running at {DateTime.Now}");

            return await this.matchMonitorJobService.Execute(size);
        }
    }
}
