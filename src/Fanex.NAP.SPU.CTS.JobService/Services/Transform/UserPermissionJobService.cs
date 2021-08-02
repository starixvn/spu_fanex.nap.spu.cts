namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Fanex.NAP.SPU.CTS.Features.UserPermission.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly Dictionary<int, string> functions = new Dictionary<int, string> {
                    { FSFunctionKeys.AssociatedAccountMonitorKey, FSFunctionKeys.AssociatedAccountMonitor },
                    { FSFunctionKeys.ProbationManagementKey, FSFunctionKeys.ProbationManagement }
                };

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
                var userPermissions = new List<UserPermissionModel>();

                foreach (var function in functions)
                {
                    var userPolicy = permissionService.GetUserPolicies(function.Value, appSettings.SiteId);

                    if (userPolicy != null && userPolicy.Any())
                    {
                        UpateUserPermission(userPermissions, function, userPolicy);
                    }
                }

                permissionService.InsertUserPermission(userPermissions);
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                      .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                      .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                      .ToString();

                sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void UpateUserPermission(List<UserPermissionModel> userPermissions, KeyValuePair<int, string> function, IEnumerable<UserPolicy> userPolicy)
        {
            var userPermission = permissionService.GetUserPermissionFromNAP(userPolicy.ToList(), function.Key);

            if (userPermission != null && userPermission.Any())
            {
                userPermissions.AddRange(userPermission);
            }
        }
    }
}
