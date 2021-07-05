namespace Fanex.NAP.SPU.CTS.Tests.Features.Transform
{
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Fanex.NAP.SPU.CTS.Shares;
    using NSubstitute;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class UserPermission
    {
        private readonly IUserPermissionService permissionService;
        private readonly IUserPermissionDataAccess dataAccess;
        private readonly ISentryClient sentryClient;

        public UserPermission()
        {
            dataAccess = Substitute.For<IUserPermissionDataAccess>();
            sentryClient = Substitute.For<ISentryClient>();
            permissionService = new UserPermissionService(dataAccess, sentryClient);
        }
    }
}
