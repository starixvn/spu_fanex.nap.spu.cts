namespace Fanex.NAP.SPU.CTS.Features.UserPermission
{
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.UserPermission.Entities;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public interface IUserPermissionService
    {
        void InsertUserPermission(IEnumerable<UserPermissionModel> userPermissions);

        IEnumerable<UserPermissionModel> GetUserPermissionFromNAP(List<UserPolicy> userPolicy, int functionKey);

        IEnumerable<UserPolicy> GetUserPolicies(string functionName, int siteId);
    }

    public class UserPermissionService : IUserPermissionService
    {
        private readonly IUserPermissionDataAccess dataAccess;
        private readonly UserService userService;

        public UserPermissionService(IUserPermissionDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            userService = new UserService();
        }

        public void InsertUserPermission(IEnumerable<UserPermissionModel> userPermissions)
        {
            if (userPermissions != null && userPermissions.Any())
            {
                dataAccess.InsertUserPermission(JsonConvert.SerializeObject(userPermissions));
            }
        }

        private UserPermissionModel TransformUserPermission(UserPolicy userPolicy, int functionKey)
        {
            if(userPolicy != null && userPolicy.User != null)
            {
                return new UserPermissionModel
                {
                    UserID = userPolicy.User.Id,
                    FuncID = functionKey
                };
            }

            return null;
        }

        public IEnumerable<UserPolicy> GetUserPolicies(string functionName, int siteId)
        {
            if(!string.IsNullOrWhiteSpace(functionName) && siteId != 0)
            {
                return userService.GetUserPolicies(functionName, siteId);
            }

            return new List<UserPolicy>();
        }

        public IEnumerable<UserPermissionModel> GetUserPermissionFromNAP(List<UserPolicy> userPolicy, int functionKey)
        {
            if (userPolicy != null && userPolicy.Any())
            {
                var userPermission = userPolicy.Select(x => TransformUserPermission(x, functionKey));

                return userPermission;
            }

            return new List<UserPermissionModel>();
        }
    }
}
