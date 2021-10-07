using Fanex.NAP.SPU.CTS.JobService.Services;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    [ApiController]
    [Route("api/monitoring")]
    public class MonitoringController : ControllerBase
    {
        private readonly ISentryClient sentryClient;

        private readonly IExistingCustomerJobService existingCustomerJobService;
        private readonly IClassifyProblemCaseMonitorJobService classifyProblemCaseMonitorJobService;


        public MonitoringController(ISentryClient sentryClient
            , IExistingCustomerJobService existingCustomerJobService
            , IClassifyProblemCaseMonitorJobService classifyProblemCaseMonitorJobService)
        {
            this.sentryClient = sentryClient;
            this.existingCustomerJobService = existingCustomerJobService;
            this.classifyProblemCaseMonitorJobService = classifyProblemCaseMonitorJobService;
        }

        [HttpPost("existingcustomer")]
        public async Task<bool> ExistingCustomer(string jobName)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            var currentDate = DateTime.Now;

            var fromDate = currentDate.AddHours(-1 * 24);

            var existingPlaceBetCustomers = existingCustomerJobService.GetPlaceBetCustomer(fromDate, currentDate);

            if (existingPlaceBetCustomers.Any())
            {
                var foundCustomerNoTrans = existingCustomerJobService.FoundCustomerNoTransInCTS(existingPlaceBetCustomers, fromDate, currentDate);

                if (foundCustomerNoTrans.Any())
                {                    
                    var groupBySubscriber = foundCustomerNoTrans.GroupBy(x => x.SubscriberId).ToList();

                    groupBySubscriber.ForEach(x =>
                    {
                        sentryClient.CaptureMessage($"{x.Key}: {string.Join(", ", x.Select(a => a.CustId + ":" + a.UserName))}");
                    });
                }
            }

            return await Task.FromResult(true);
        }

        [HttpPost("classifyproblemcasemonitor")]
        public async Task<bool> ClassifyProblemCaseMonitor()
        {
            return await this.classifyProblemCaseMonitorJobService.Execute();
        }


    }
}
