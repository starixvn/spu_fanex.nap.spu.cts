using Fanex.NAP.SPU.CTS.Features.UserPermission;
using Fanex.NAP.SPU.CTS.Features.UserPermission.Entities;
using Fanex.NAP.SPU.CTS.Shares;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    public interface IUserPermissionJobService
    {
    }

    public class UserPermissionJobService : IUserPermissionJobService
    {
        private readonly IUserPermissionService permissionService;
        private readonly ISentryClient sentryClient;
        private readonly IAppSettings appSettings;

        public UserPermissionJobService(ISentryClient sentryClient, IUserPermissionService permissionService, IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.permissionService = permissionService;
            this.appSettings = appSettings;

        }


        public Task<bool> SyncUserPermission()
        {
            try
            {
                permissionService
                    .Start()
                    .SyncUserPermission(FSFunctionKeys.AssociatedAccountMonitor, FSFunctionKeys.AssociatedAccountMonitorKey, appSettings.SiteId, appSettings.UserPermissionIsWriteLog)
                    .SyncUserPermission(FSFunctionKeys.ProbationManagement, FSFunctionKeys.ProbationManagementKey, appSettings.SiteId, appSettings.UserPermissionIsWriteLog)
                    .InsertUserPermission();

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                //var logMessage = new LogMessage(ex.Message)
                //    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                //    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                //LogService.Error(logMessage);

                return Task.FromResult(false);
            }
        }
    }
}
