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
        public async Task<bool> MatchMonitorRuleLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule01(true, numberOfThread);
        }

        /// <summary>
        /// Process Rule1 for GroupBeting, Hedging, Arbitrage
        /// </summary>
        [HttpPost("MatchMonitorInsertRuleNonLive")]
        public async Task<bool> MatchMonitorRuleNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule01(false, numberOfThread);
        }

        /// <summary>
        /// Process Rule2 for FixedGame
        /// </summary>
        [HttpPost("MatchMonitorInsertRule2Live")]
        public async Task<bool> MatchMonitorRule2Live([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule02(true, numberOfThread);
        }

        /// <summary>
        /// Insert Rule2 for FixedGame
        /// </summary>
        [HttpPost("MatchMonitorInsertRule2NonLive")]
        public async Task<bool> MatchMonitorRule2NonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule02(false, numberOfThread);
        }

        /// <summary>
        /// Process Rule3 for Soccer Hedging
        /// </summary>
        [HttpPost("MatchMonitorInsertRule3Live")]
        public async Task<bool> MatchMonitorRule3Live([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule03(true, numberOfThread);
        }

        /// <summary>
        /// Process Rule3 for Soccer Hedging
        /// </summary>
        [HttpPost("MatchMonitorInsertRule3NonLive")]
        public async Task<bool> MatchMonitorRule3NonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule03(false, numberOfThread);
        }
    }
}