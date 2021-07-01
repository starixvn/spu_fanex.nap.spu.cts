namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/classificationjob")]
    public class ClassificationJobController : ControllerBase
    {
        private readonly IProblemAccountJobService problemAccountInsertJobService;
        private readonly INormalAccountJobService normalAccountInsertJobService;
        private readonly IDailyClassificationJobService dailyClassificationJobService;
        private readonly IRealtimeClassificationJobService realtimeClassificationJobService;
        private readonly IProbationClassificationJobService probationClassificationJobService;

        public ClassificationJobController(
            IProblemAccountJobService problemAccountInsertJobService,
            INormalAccountJobService normalAccountInsertJobService,
            IDailyClassificationJobService dailyClassificationJobService,
            IRealtimeClassificationJobService realtimeClassificationJobService,
            IProbationClassificationJobService probationClassificationJobService)
        {
            this.problemAccountInsertJobService = problemAccountInsertJobService;
            this.normalAccountInsertJobService = normalAccountInsertJobService;
            this.dailyClassificationJobService = dailyClassificationJobService;
            this.realtimeClassificationJobService = realtimeClassificationJobService;
            this.probationClassificationJobService = probationClassificationJobService;
        }

        [HttpPost("problemAccountInsert")]
        public async Task<bool> InsertProblemAccount([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await this.problemAccountInsertJobService.Execute(batchExternalSize, batchInternalSize);
        }

        [HttpPost("insertnormalaccount")]
        public async Task<bool> InsertNormalAccount([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await this.normalAccountInsertJobService.Execute(batchExternalSize, batchInternalSize);
        }

        [HttpPost("scanningdailyclassification")]
        public async Task<bool> DailyScanningClassification([FromForm] int batchSize)
        {
            return await this.dailyClassificationJobService.Execute(batchSize);
        }

        [HttpPost("interruptdailyClassification")]
        public async Task<bool> DailyScanningClassification([FromForm] bool isInterruptDailyClassificationJob)
        {
            return await this.dailyClassificationJobService.InterruptDailyScanningJob(isInterruptDailyClassificationJob);
        }

        [HttpPost("scanningrealtimeclassification")]
        public async Task<bool> RealtimeScanningClassification([FromForm] int batchSize)
        {
            return await this.realtimeClassificationJobService.Execute(batchSize);
        }

        [HttpPost("scanningrealtimeclassification")]
        public async Task<bool> ProbationScanningClassification([FromForm] int batchSize)
        {
            return await this.probationClassificationJobService.Execute(batchSize);
        }
    }
}