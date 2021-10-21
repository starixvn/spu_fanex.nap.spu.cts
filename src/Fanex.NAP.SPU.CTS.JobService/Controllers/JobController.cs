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
        private readonly IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService;
        private readonly IArchiveCustClassificationHistoryJobService archiveCustClassHistoryJobService;

        public JobController(ISentryClient sentryClient,
            IMatchMonitorJobService matchMonitorJobService,
            ICustomerStatusJobService customerStatusJobService,
            IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService,
            IArchiveCustClassificationHistoryJobService archiveCustClassHistoryJobService)
        {
            this.sentryClient = sentryClient;
            this.matchMonitorJobService = matchMonitorJobService;
            this.customerStatusJobService = customerStatusJobService;
            this.affectedEvidenceFromQueueJobService = affectedEvidenceFromQueueJobService;
            this.archiveCustClassHistoryJobService = archiveCustClassHistoryJobService;
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
        public async Task<bool> MatchMonitorRuleLive([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule(size, true, numberOfThread);
        }

        [HttpPost("MatchMonitorRuleNonLive")]
        public async Task<bool> MatchMonitorRuleNonLive([FromForm] int size, [FromForm] int numberOfThread)
        {
            return await this.matchMonitorJobService.ProcessRule(size, false, numberOfThread);
        }

        [HttpPost("initialcustomerstatus")]
        public async Task<bool> InitialCustomerStatus()
        {
            return await this.customerStatusJobService.Execute();
        }

        [HttpPost("AffectedEvidenceFromQueue")]
        public async Task<bool> PlaceAffectedEvidenceFromQueue()
        {
            return await this.affectedEvidenceFromQueueJobService.Execute();
        }

        [HttpPost("CleanUpUnChangedCCHistory")]
        public async Task<bool> CleanUpUnchangedCustClassificationHistory()
        {
            return await this.archiveCustClassHistoryJobService.CleanUpUnChangedCustClassHistory();
        }

        [HttpPost("ArchiveChangedCCHistory")]
        public async Task<bool> ArchiveChangedCustClassificationHistory()
        {
            return await this.archiveCustClassHistoryJobService.ArchiveChangedCustClassHistory();
        }

        //[HttpPost("updatewritelog")]
        //public async Task<bool> UpdateWriteLog([FromForm] string serviceName, [FromForm] bool isWriteLog)
        //{
        //    return await this.matchMonitorJobService.UpdateWriteLogSetting(serviceName, isWriteLog);
        //}
    }
}