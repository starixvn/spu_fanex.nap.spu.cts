namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IUserPermissionJobService
    {
        Task<bool> SyncUserPermissionFromNAP();
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


        public Task<bool> SyncUserPermissionFromNAP()
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
                sentryClient.CaptureException(ex);
                return Task.FromResult(false);
            }
        }
    }
}
