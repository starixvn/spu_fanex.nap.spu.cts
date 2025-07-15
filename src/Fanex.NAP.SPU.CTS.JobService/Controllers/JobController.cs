namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Fanex.NAP.SPU.CTS.Shares;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ICustomerStatusJobService customerStatusJobService;
        private readonly IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService;
       
        public JobController(
            ICustomerStatusJobService customerStatusJobService,
            IAffectedEvidenceFromQueueJobService affectedEvidenceFromQueueJobService)
        {
            this.customerStatusJobService = customerStatusJobService;
            this.affectedEvidenceFromQueueJobService = affectedEvidenceFromQueueJobService;
        }

        [HttpPost("Test")]
        public bool Test()
        {
            LogExtension.LogInfo($"Test CTS service {DateTime.Now}");

            return true;
        }

        [HttpGet("health")]
        public string CheckHealth()
        {
            LogExtension.LogError($"Check health CTS service {DateTime.Now}");
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