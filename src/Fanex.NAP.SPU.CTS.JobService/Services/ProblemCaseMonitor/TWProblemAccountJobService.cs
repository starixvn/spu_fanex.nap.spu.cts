namespace Fanex.NAP.SPU.CTS.JobService.Services.ProblemCaseMonitor
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Entities;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITWProblemAccountJobService
    {
        Task<bool> DailyScan();
    }

    public class TWProblemAccountJobService : ITWProblemAccountJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly ITWProblemAccountService twProblemAccService;

        public TWProblemAccountJobService(ITWProblemAccountService twProblemAccService, ISentryClient sentryClient)
        {
            this.twProblemAccService = twProblemAccService;
            this.sentryClient = sentryClient;
        }

        public Task<bool> DailyScan()
        {
            try
            {
                var scanningCust = twProblemAccService.ScanSystemFormulaBetMonitor();
                var perfCust = twProblemAccService.GetCustomerPerformance(string.Join(",", scanningCust.Select(c => c.CustID)));
                var fullData = (from sCust in scanningCust
                                join pCust in perfCust on sCust.CustID equals pCust.CustID into temp
                                from pCust in temp.DefaultIfEmpty()
                                select new RescanTWProblemAccountModel
                                {
                                    CustID = sCust.CustID,
                                    Username = sCust.Username,
                                    IsMonitorCategoryGroup = (sCust != null) && sCust.IsMonitorCategoryGroup,
                                    Winloss = pCust.Winloss
                                }).ToList();
                var markPAList = fullData.Where(c => c.Winloss >= 0 && c.IsMonitorCategoryGroup);

                //twProblemAccService.InsertProblemAccToNAP(string.Join(",", markPAList.Select(c => c.Username)));
                twProblemAccService.RemoveSystemFormulaBetCustomer(string.Join(",", markPAList.Select(c => c.Username)));
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                        .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error($"CTS - TWProblemAccountService - DailyScan exeption message:  {logMessage}");

                sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
