using Fanex.Logging.Extensions;
using Fanex.NAP.SPU.CTS.JobService.Services;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Collections.Generic;
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

        public MonitoringController(ISentryClient sentryClient, IExistingCustomerJobService existingCustomerJobService)
        {
            this.sentryClient = sentryClient;
            this.existingCustomerJobService = existingCustomerJobService;
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
                sentryClient.CaptureMessage($"{existingPlaceBetCustomers.Count()}:" + string.Join(",", existingPlaceBetCustomers.Select(x => x.CustId)));

                var foundCustomerNoTrans = existingCustomerJobService.FoundCustomerNoTransInCTS(existingPlaceBetCustomers, fromDate, currentDate);

                if (foundCustomerNoTrans.Any())
                {
                    var customerNoTrans = string.Join(",", foundCustomerNoTrans.Select(x => x.UserName + ":" + x.SubscriberName));

                    sentryClient.CaptureMessage($"The missiing customer transaction. List customer: {customerNoTrans}");
                }
            }

            return await Task.FromResult(true);
        }
    }
}
