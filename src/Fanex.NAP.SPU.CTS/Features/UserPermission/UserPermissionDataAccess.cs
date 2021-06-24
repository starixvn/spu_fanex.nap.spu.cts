using Fanex.Data.Repository;
using Fanex.NAP.SPU.CTS.Features.UserPermission.Criteria;
namespace Fanex.NAP.SPU.CTS.Features.UserPermission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUserPermissionDataAccess
    {
        void InsertUserPermission(string permissionUserIdsJson);
    }

    public class UserPermissionDataAccess : IUserPermissionDataAccess
    {
        readonly IDynamicRepository repository;

        public UserPermissionDataAccess()
        {
            repository = new DynamicRepository();
        }

        public void InsertUserPermission(string permissionUserIdsJson)
        {
            var criteria = new InsertUserPermissionCriteria
            {
                UserIDList = permissionUserIdsJson
            };

            repository.Execute(criteria);
        }
    }
}
