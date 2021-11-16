namespace Fanex.NAP.SPU.CTS.JobService.Services.ProblemCaseMonitor
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Criteria;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Entities;
    using Sentry;
    using System;
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
                                    IsMonitorCategoryGroup = sCust.IsMonitorCategoryGroup,
                                    IsMonitorSite = sCust.IsMonitorSite,
                                    Winloss = pCust == null ? 0 : pCust.Winloss,
                                    RoleId = sCust.RoleId,
                                    ReasonId = sCust.ReasonId,
                                    PreventActionId = sCust.PreventActionId,
                                    Comment = sCust.Comment,
                                    VoidTime = sCust.VoidTime,
                                    LimitAmount = sCust.LimitAmount,
                                    UserId = sCust.UserId,
                                    Ip = sCust.Ip
                                }).ToList();

                var markPAList = fullData.Where(c => c.Winloss >= 0 && !c.IsMonitorCategoryGroup);
                var markPAListGroupByInsertTime = markPAList.GroupBy(c => c.InsertTime);
                foreach (var paList in markPAListGroupByInsertTime)
                {
                    if (paList.Any())
                    {
                        var firtItem = paList.FirstOrDefault();
                        var param = new NapProblemAccInsertCriteria
                        {
                            RoleId = firtItem.RoleId,
                            ReasonId = firtItem.ReasonId,
                            PreventActionId = firtItem.PreventActionId,
                            Comment = firtItem.Comment,
                            VoidTime = firtItem.VoidTime,
                            LimitAmount = firtItem.LimitAmount,
                            UserId = firtItem.UserId,
                            IP = firtItem.Ip
                        };

                        twProblemAccService.InsertProblemAccToNAP(string.Join(",", paList.Select(c => c.Username)), param);
                        twProblemAccService.RemoveSystemFormulaBetCustomer(string.Join(",", paList.Select(c => c.CustID)));
                    }
                }
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
