namespace Fanex.NAP.SPU.CTS.Features.UserPermission
{
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.UserPermission.Entities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
        private readonly UserService userService;

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

            if (isWriteLogInfo)
                //LogService.Info($"Get User Permission - {functionName} - {JsonConvert.SerializeObject(userPolicy?.Select(c => c.User.Id))} - {DateTime.Now}");

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

                UserID = userPolicy.User.Id,
                FuncID = functionKey
            };
        }
    }
}
