using Fanex.NAP.SPU.CTS.JobService.Services;
using Fanex.NAP.SPU.CTS.Shares;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    [ApiController]
    [Route("api/monitoring")]
    public class MonitoringController : ControllerBase
    {
        private readonly IExistingCustomerJobService existingCustomerJobService;
        private readonly ILoginTransactionMonitoringJobService loginTransactionMonitoringJobService;

        public MonitoringController(
            IExistingCustomerJobService existingCustomerJobService,
            ILoginTransactionMonitoringJobService loginTransactionMonitoringJobService)
        {
            this.existingCustomerJobService = existingCustomerJobService;
            this.loginTransactionMonitoringJobService = loginTransactionMonitoringJobService;           
        }

        [HttpPost("existingcustomer")]
        public async Task<bool> ExistingCustomer(string jobName)
        {
            LogExtension.LogInfo($"{jobName} is running at {DateTime.Now}");

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
                        LogExtension.LogInfo($"{x.Key}: {string.Join(", ", x.Select(a => a.CustId + ":" + a.UserName))}");
                    });
                }
            }

            return await Task.FromResult(true);
        }

        [HttpPost("LastLoginTransactionMonitoring")]
        public async Task<bool> LastLoginTransactionMonitoring([FromForm] int thresHold)
        {
            return await this.loginTransactionMonitoringJobService.LastLoginProcessMonitoring(thresHold);
        }
    }
}
