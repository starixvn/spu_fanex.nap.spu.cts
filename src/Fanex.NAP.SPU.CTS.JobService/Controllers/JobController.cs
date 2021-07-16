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
        private readonly IMatchMonitorJobService matchMonitorJobService;

        public JobController(ISentryClient sentryClient,
            IMatchMonitorJobService matchMonitorJobService)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorJobService = matchMonitorJobService;
        }

        [HttpPost("matchmonitor")]
        public async Task<bool> MatchMonitor([FromForm] int size)
        {
            sentryClient.CaptureMessage($"Match Monitor Service is running at {DateTime.Now}");

            return await this.matchMonitorJobService.Execute(size);
        }
    }
}