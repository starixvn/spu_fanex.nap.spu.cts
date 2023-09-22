namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly ICustomerStatusJobService customerStatusJobService;
        private readonly IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService;
       
        public JobController(ISentryClient sentryClient,
            ICustomerStatusJobService customerStatusJobService,
            IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService)
        {
            this.sentryClient = sentryClient;
            this.customerStatusJobService = customerStatusJobService;
            this.affectedEvidenceFromQueueJobService = affectedEvidenceFromQueueJobService;
        }

        [HttpGet("health")]
        public string CheckHealth()
        {
            sentryClient.CaptureMessage($"Check health CTS service {DateTime.Now}", Sentry.Protocol.SentryLevel.Info);
            return JsonConvert.SerializeObject(DateTime.Now.ToString());
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

        //[HttpPost("updatewritelog")]
        //public async Task<bool> UpdateWriteLog([FromForm] string serviceName, [FromForm] bool isWriteLog)
        //{
        //    return await this.matchMonitorJobService.UpdateWriteLogSetting(serviceName, isWriteLog);
        //}
    }
}