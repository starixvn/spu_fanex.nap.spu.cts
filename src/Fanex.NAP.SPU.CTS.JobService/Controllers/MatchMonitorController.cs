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
        /// Process Rule GroupBetting
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleGroupBettingLive")]
        public async Task<bool> MatchMonitorRuleGroupBettingLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleGroupBetting(true, numberOfThread);
        }

        /// <summary>
        /// Process Rule GroupBetting
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleGroupBettingNonLive")]
        public async Task<bool> MatchMonitorRuleGroupBettingNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleGroupBetting(false, numberOfThread);
        }

        /// <summary>
        /// Process Rule FixedGame
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleFixedGameLive")]
        public async Task<bool> MatchMonitorRuleFixedGameLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleFixedGame(true, numberOfThread);
        }

        /// <summary>
        /// Insert Rule FixedGame
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleFixedGameNonLive")]
        public async Task<bool> MatchMonitorRuleFixedGameNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleFixedGame(false, numberOfThread);
        }

        /// <summary>
        /// Process Rule Soccer Hedging
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleHedgingLive")]
        public async Task<bool> MatchMonitorRuleHedgingLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleHedging(true, numberOfThread);
        }

        /// <summary>
        /// Process Rule Soccer Hedging
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleHedgingNonLive")]
        public async Task<bool> MatchMonitorRuleHedgingNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleHedging(false, numberOfThread);
        }

        /// <summary>
        /// Process Rule for Soccer Irrigation
        /// </summary>
        /// <param name="numberOfThread"></param>
        /// <returns></returns>
        [HttpPost("MatchMonitorProcessRuleIrrigationNonLive")]
        public async Task<bool> MatchMonitorRuleIrrigationNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleIrrigation(false, numberOfThread);
        }

        /// <summary>
        /// Process Rule Soccer Arbitrage
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleArbitrageNonLive")]
        public async Task<bool> MatchMonitorRuleArbitrageNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleArbitrage(false, numberOfThread);
        }
    }
}