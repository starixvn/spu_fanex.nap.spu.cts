namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using Sentry;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorJobService matchMonitorJobService;
        private readonly ICustomerStatusJobService customerStatusJobService;

        public JobController(ISentryClient sentryClient,
            IMatchMonitorJobService matchMonitorJobService,
            ICustomerStatusJobService customerStatusJobService)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorJobService = matchMonitorJobService;
            this.customerStatusJobService = customerStatusJobService;
        }

        [HttpPost("matchmonitor")]
        public async Task<bool> MatchMonitor([FromForm] int size)
        {
            return await this.matchMonitorJobService.Execute(size);
        }

        [HttpPost("initialcustomerstatus")]
        public async Task<bool> InitialCustomerStatus()
        {
            return await this.customerStatusJobService.Execute();
        }

        [HttpPost("updatewritelog")]
        public async Task<bool> UpdateWriteLog([FromForm] string serviceName, [FromForm] bool isWriteLog)
        {
            return await this.matchMonitorJobService.UpdateWriteLogSetting(serviceName, isWriteLog);
        }
    }
}