namespace Fanex.NAP.SPU.CTS.Features.UserPermission
{
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.UserPermission.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System.Collections.Generic;
    using System.Linq;

    public interface IUserPermissionService
    {
        UserPermissionService InsertUserPermission();

        UserPermissionService Start();

        UserPermissionService SyncUserPermission(string functionName, int functionKey, int siteId, bool isWriteLogInfo);
    }

    public class UserPermissionService : IUserPermissionService
    {
        private List<UserPermissionModel> userPermissions;
        private readonly IUserPermissionDataAccess dataAccess;
        private readonly ISentryClient sentryClient;
        private readonly UserService userService;

        public UserPermissionService(IUserPermissionDataAccess dataAccess, ISentryClient sentryClient)
        {
            this.dataAccess = dataAccess;
            this.sentryClient = sentryClient;
            userService = new UserService();
        }

        public UserPermissionService Start()
        {
            userPermissions = new List<UserPermissionModel>();            
            return this;
        }

        public UserPermissionService InsertUserPermission()
        {
            if (userPermissions != null && userPermissions.Any())
            {
                dataAccess.InsertUserPermission(JsonConvert.SerializeObject(userPermissions));
            }

            return this;
        }

        public UserPermissionService SyncUserPermission(string functionName, int functionKey, int siteId, bool isWriteLogInfo)
        {
            var userPolicy = userService.GetUserPolicies(functionName, siteId);
            Utilities.LogSentryInfo("UserPermisison", "Get User Permission", userPolicy, isWriteLogInfo, sentryClient);

            if (userPolicy != null && userPolicy.Any())
            {
                var userPermission = userPolicy.Select(x => TransformUserPermission(x, functionKey));

                if (userPermission != null && userPermission.Any())
                {
                    userPermissions.AddRange(userPermission);
                }
            }

            return this;
        }

        private UserPermissionModel TransformUserPermission(UserPolicy userPolicy, int functionKey)
        {
            return new UserPermissionModel
            {
                UserId = userPolicy.User.Id,
                FuncId = functionKey
            };
        }
    }
}
