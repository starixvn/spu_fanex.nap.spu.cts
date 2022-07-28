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
        [HttpPost("MatchMonitorRuleLive")]
        public async Task<bool> MatchMonitorRuleLive([FromForm] int size, [FromForm] int numberOfThread, [FromForm] bool isProccessFromMaster = true)
        {
            return await this.matchMonitorJobService.ProcessRule01(size, true, numberOfThread, isProccessFromMaster);
        }

        /// <summary>
        /// Process Rule1 for GroupBeting, Hedging, Arbitrage
        /// </summary>
        [HttpPost("MatchMonitorRuleNonLive")]
        public async Task<bool> MatchMonitorRuleNonLive([FromForm] int size, [FromForm] int numberOfThread, [FromForm] bool isProccessFromMaster = true)
        {
            return await this.matchMonitorJobService.ProcessRule01(size, false, numberOfThread, isProccessFromMaster);
        }

        /// <summary>
        /// Process Rule2 for FixedGame
        /// </summary>
        [HttpPost("MatchMonitorRule2Live")]
        public async Task<bool> MatchMonitorRule2Live([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule02(size, true, numberOfThread);
        }

        /// <summary>
        /// Insert Rule2 for FixedGame
        /// </summary>
        [HttpPost("MatchMonitorRule2NonLive")]
        public async Task<bool> MatchMonitorRule2NonLive([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule02(size, false, numberOfThread);
        }
    }
}