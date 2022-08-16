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

        /// <summary>
        /// Process Rule1 for GroupBeting, Hedging, Arbitrage
        /// </summary>
        [HttpPost("MatchMonitorInsertRuleLive")]
        public async Task<bool> MatchMonitorRuleLive([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule01(size, true, numberOfThread);
        }

        /// <summary>
        /// Process Rule1 for GroupBeting, Hedging, Arbitrage
        /// </summary>
        [HttpPost("MatchMonitorInsertRuleNonLive")]
        public async Task<bool> MatchMonitorRuleNonLive([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule01(size, false, numberOfThread);
        }

        /// <summary>
        /// Process Rule2 for FixedGame
        /// </summary>
        [HttpPost("MatchMonitorInsertRule2Live")]
        public async Task<bool> MatchMonitorRule2Live([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule02(size, true, numberOfThread);
        }

        /// <summary>
        /// Insert Rule2 for FixedGame
        /// </summary>
        [HttpPost("MatchMonitorInsertRule2NonLive")]
        public async Task<bool> MatchMonitorRule2NonLive([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule02(size, false, numberOfThread);
        }
    }
}