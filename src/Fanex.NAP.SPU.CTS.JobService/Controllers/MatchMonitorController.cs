namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using Sentry;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/matchmonitor")]
    public class MatchMonitorController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorJobService matchMonitorJobService;

        public MatchMonitorController(ISentryClient sentryClient,
            IMatchMonitorJobService matchMonitorJobService)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorJobService = matchMonitorJobService;
        }

        [HttpPost("MatchMonitorInsertTicketDetailLive")]
        public async Task<bool> MatchMonitorInsertTicketDetailLive([FromForm] int size)
        {
            return await this.matchMonitorJobService.InsertTicketDetail(size, true);
        }

        [HttpPost("MatchMonitorInsertTicketDetailNonLive")]
        public async Task<bool> MatchMonitorInsertTicketDetailNonLive([FromForm] int size)
        {
            return await this.matchMonitorJobService.InsertTicketDetail(size, false);
        }

        [HttpPost("MatchMonitorRuleLive")]
        public async Task<bool> MatchMonitorRuleLive([FromForm] int size, [FromForm] int numberOfThread, [FromForm] bool isProccessFromMaster = true)
        {
            return await this.matchMonitorJobService.ProcessRule(size, true, numberOfThread, isProccessFromMaster);
        }

        [HttpPost("MatchMonitorRuleNonLive")]
        public async Task<bool> MatchMonitorRuleNonLive([FromForm] int size, [FromForm] int numberOfThread, [FromForm] bool isProccessFromMaster = true)
        {
            return await this.matchMonitorJobService.ProcessRule(size, false, numberOfThread, isProccessFromMaster);
        }
    }
}