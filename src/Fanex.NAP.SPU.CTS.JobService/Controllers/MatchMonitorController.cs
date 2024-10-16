namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Fanex.NAP.SPU.CTS.JobService.Services.Parlay;
    using Microsoft.AspNetCore.Mvc;
    using Sentry;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/matchmonitor")]
    public class MatchMonitorController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IMatchMonitorJobService matchMonitorJobService;
        private readonly IMatchMonitorParlayJobService matchMonitorParlayJobService;

        public MatchMonitorController(ISentryClient sentryClient,
            IMatchMonitorJobService matchMonitorJobService, 
            IMatchMonitorParlayJobService matchMonitorParlayJobService)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorJobService = matchMonitorJobService;
            this.matchMonitorParlayJobService = matchMonitorParlayJobService;
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
        /// Process Rule Saba GroupBetting
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleSabaGroupBettingLive")]
        public async Task<bool> MatchMonitorRuleSabaGroupBettingLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleSabaGroupBetting(true, numberOfThread);
        }

        /// <summary>
        /// Process Rule Saba GroupBetting
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleSabaGroupBettingNonLive")]
        public async Task<bool> MatchMonitorRuleSabaGroupBettingNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleSabaGroupBetting(false, numberOfThread);
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
        public async Task<bool> MatchMonitorRuleHedgingLive([FromForm] int numberInternalOfThread, [FromForm] int numberExternalOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleHedging(true, numberInternalOfThread, numberExternalOfThread);
        }

        /// <summary>
        /// Process Rule Soccer Hedging
        /// </summary>
        [HttpPost("MatchMonitorProcessRuleHedgingNonLive")]
        public async Task<bool> MatchMonitorRuleHedgingNonLive([FromForm] int numberInternalOfThread, [FromForm] int numberExternalOfThread)
        {
            return await this.matchMonitorJobService.ProcessRuleHedging(false, numberInternalOfThread, numberExternalOfThread);
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

        #region Mix Parlay
        [HttpPost("MatchMonitorParlayInsertTicketDetailLive")]
        public async Task<bool> MatchMonitorParlayInsertTicketDetailLive([FromForm] int size)
        {
            return await this.matchMonitorParlayJobService.InsertParlayTicketDetail(size, true);
        }

        [HttpPost("MatchMonitorParlayInsertTicketDetailNonLive")]
        public async Task<bool> MatchMonitorParlayInsertTicketDetailNonLive([FromForm] int size)
        {
            return await this.matchMonitorParlayJobService.InsertParlayTicketDetail(size, false);
        }

        [HttpPost("MatchMonitorParlayProcessRuleGroupBettingLive")]
        public async Task<bool> MatchMonitorParlayProcessRuleGroupBettingLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorParlayJobService.ProcessParlayRuleGroupBetting(true, numberOfThread);
        }

        [HttpPost("MatchMonitorParlayProcessRuleGroupBettingNonLive")]
        public async Task<bool> MatchMonitorParlayProcessRuleGroupBettingNonLive([FromForm] int numberOfThread)
        {
            return await this.matchMonitorParlayJobService.ProcessParlayRuleGroupBetting(false, numberOfThread);
        }
        #endregion

    }
}