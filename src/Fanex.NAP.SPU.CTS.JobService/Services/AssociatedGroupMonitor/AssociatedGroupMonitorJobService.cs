namespace Fanex.NAP.SPU.CTS.JobService.Services.AssociatedGroupMonitor
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Shares.Models;
    using Newtonsoft.Json;
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
        private readonly ISystemParameterServices systemParameterServices;

        public AssociatedGroupMonitorJobService(ISentryClient sentryClient
            , IAssociatedGroupMonitorService associatedGroupService
            , ISystemParameterServices systemParameterServices)
        {
            this.sentryClient = sentryClient;
            this.associatedGroupService = associatedGroupService;
            this.systemParameterServices = systemParameterServices;
        }

        public Task<bool> ScanAssociatedAccount()
        {
            try
            {
                var dbResult = new DBResult { ErrorCode = 0 };

                var custInfo = this.associatedGroupService.ScanAssociatedAccount();

                if (custInfo.ScanAssociatedAccountList != null && custInfo.ScanAssociatedAccountList.Any())
                {
                    var custIDs = string.Join(",", custInfo.ScanAssociatedAccountList.Select(x => x.CustID).Distinct());

                    var ctsCustIDs = string.Join(",", custInfo.ScanAssociatedAccountList.Select(x => x.CTSCustID).Distinct());

                    var custInfoMainDBs = this.associatedGroupService.GetCustomerInfoMainDB(custIDs, DangerLevelABI);
                    
                    dbResult = this.associatedGroupService.UpdateDangerLevelToMainDB(custInfoMainDBs, custInfo.ABI, DangerLevelABI);

                    if (dbResult.ErrorCode == 0)
                    {
                        dbResult = this.associatedGroupService.AddAssociatedAccount(new AddAssociatedAccountModel
                        {
                            CTSCustIDs = ctsCustIDs,
                            IsAuto = true,
                            GroupID = custInfo.GroupID
                        });
                    }                                       
                }

                if (dbResult.ErrorCode == 0)
                {
                    this.systemParameterServices.UpdateValueForSystemParamById(custInfo.GroupID.ToString(), Consts.SystemParameters.AssociatedGroupLastGroupID);
                    this.systemParameterServices.UpdateValueForSystemParamById(custInfo.LastCTSCustID.ToString(), Consts.SystemParameters.AssociatedGroupLastCTSCustID);
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
