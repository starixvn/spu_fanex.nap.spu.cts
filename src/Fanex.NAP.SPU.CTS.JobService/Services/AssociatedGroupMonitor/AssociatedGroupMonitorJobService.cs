namespace Fanex.NAP.SPU.CTS.JobService.Services.AssociatedGroupMonitor
{
    using Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAssociatedGroupMonitorJobService
    {
        Task<bool> ScanAssociatedAccount();
    }

    public class AssociatedGroupMonitorJobService : IAssociatedGroupMonitorJobService
    {
        private readonly ISentryClient sentryClient;
        private const string serviceName = "ArchiveAssociationCustomer";
        private const int DangerLevelABI = 2;
        private readonly IAssociatedGroupMonitorService associatedGroupService;

        public AssociatedGroupMonitorJobService(ISentryClient sentryClient, IAssociatedGroupMonitorService associatedGroupService)
        {
            this.sentryClient = sentryClient;
            this.associatedGroupService = associatedGroupService;
        }

        public Task<bool> ScanAssociatedAccount()
        {
            try
            {
                var custList = this.associatedGroupService.ScanAssociatedAccount();

                if (custList.ScanAssociatedAccountList != null && custList.ScanAssociatedAccountList.Any())
                {
                    var custIDs = string.Join(",", custList.ScanAssociatedAccountList.Select(x => x.CustID).Distinct());

                    var ctsCustIDs = string.Join(",", custList.ScanAssociatedAccountList.Select(x => x.CTSCustID).Distinct());

                    var custInfoMainDBs = this.associatedGroupService.GetCustomerInfoMainDB(custIDs, DangerLevelABI);

                    var updateDangerResult = this.associatedGroupService.UpdateDangerLevelToMainDB(custInfoMainDBs, custList.ABI);

                    if (updateDangerResult.ErrorCode == 0)
                    {
                        this.associatedGroupService.AddAssociatedAccount(new AddAssociatedAccountModel
                        {
                            CustIDs = custIDs,
                            CTSCustIDs = ctsCustIDs,
                            IsAuto = true,
                            ABI = custList.ABI,
                            GroupID = custList.GroupID
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
