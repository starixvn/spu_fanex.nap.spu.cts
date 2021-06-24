namespace Fanex.NAP.SPU.CTS.Features.UserPermission
{
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
    }

    public class UserPermissionService : IUserPermissionService
    {
        private List<UserPermissionModel> userPermissions;
        private readonly IUserPermissionDataAccess dataAccess;

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

        //public UserPermissionService SyncUserPermission(string functionName, int functionKey)
        //{
        //    var userPolicy = userService.GetUserPolicies(functionName, Int32.Parse(AppSettings.SiteId));

        //    if (AppSettings.IsWriteLogInfo)
        //        LogService.Info($"Get User Permission - {functionName} - {JsonConvert.SerializeObject(userPolicy?.Select(c => c.User.Id))} - {DateTime.Now}");

        //    if (userPolicy != null && userPolicy.Any())
        //    {
        //        var userPermission = userPolicy.Select(x => TransformUserPermission(x, functionKey));

        //        if (userPermission != null && userPermission.Any())
        //        {
        //            userPermissions.AddRange(userPermission);
        //        }
        //    }

        //    return this;
        //}

        //private UserPermissionModel TransformUserPermission(UserPolicy userPolicy, int functionKey)
        //{
        //    return new UserPermissionModel
        //    {

        //        UserID = userPolicy.User.Id,
        //        FuncID = functionKey
        //    };
        //}
    }
}
