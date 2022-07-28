namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Fanex.NAP.SPU.CTS.JobService.Services.AssociatedGroupMonitor;
    using Microsoft.AspNetCore.Mvc;
    using Sentry;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly ICustomerStatusJobService customerStatusJobService;
        private readonly IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService;
        private readonly IArchiveCustClassificationHistoryJobService archiveCustClassHistoryJobService;
        private readonly IAssociatedGroupMonitorJobService associatedGroupMonitorJobService;

        public JobController(ISentryClient sentryClient,
            ICustomerStatusJobService customerStatusJobService,
            IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService,
            IArchiveCustClassificationHistoryJobService archiveCustClassHistoryJobService,
            IAssociatedGroupMonitorJobService associatedGroupMonitorJobService)
        {
            this.sentryClient = sentryClient;
            this.customerStatusJobService = customerStatusJobService;
            this.affectedEvidenceFromQueueJobService = affectedEvidenceFromQueueJobService;
            this.archiveCustClassHistoryJobService = archiveCustClassHistoryJobService;
            this.associatedGroupMonitorJobService = associatedGroupMonitorJobService;
        }

        [HttpPost("initialcustomerstatus")]
        public async Task<bool> InitialCustomerStatus()
        {
            return await this.customerStatusJobService.Execute();
        }

        [HttpPost("AffectedEvidenceInsertFromQueue")]
        public async Task<bool> InsertAffectedEvidenceFromQueue()
        {
            return await this.affectedEvidenceFromQueueJobService.InsertAffectedEvidences();
        }

        [HttpPost("AffectedEvidenceRemoveFromQueue")]
        public async Task<bool> RemoveAffectedEvidenceFromQueue()
        {
            return await this.affectedEvidenceFromQueueJobService.RemoveAffectedEvidences();
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

        [HttpPost("ScanAssociatedAccount")]
        public async Task<bool> ScanAssociatedAccount()
        {
            return await this.associatedGroupMonitorJobService.ScanAssociatedAccount();
        }

        //[HttpPost("updatewritelog")]
        //public async Task<bool> UpdateWriteLog([FromForm] string serviceName, [FromForm] bool isWriteLog)
        //{
        //    return await this.matchMonitorJobService.UpdateWriteLogSetting(serviceName, isWriteLog);
        //}
    }
}