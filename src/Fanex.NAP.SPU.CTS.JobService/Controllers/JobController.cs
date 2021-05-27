using Fanex.NAP.SPU.DCS.JobService.Services;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.DCS.JobService.Controllers
{
    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IAccountJobService accountJobService;       

        public JobController(ISentryClient sentryClient,
            IAccountJobService accountJobService)
        {
            this.sentryClient = sentryClient;
            this.accountJobService = accountJobService;
            
        }

        [HttpPost("transformaccount")]
        public async Task<bool> Account(string jobName, int size, int numberOfBatch)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            //return await this.accountJobService.Execute(size, numberOfBatch);

            return await Task.FromResult(true);

        }        
    }
}
