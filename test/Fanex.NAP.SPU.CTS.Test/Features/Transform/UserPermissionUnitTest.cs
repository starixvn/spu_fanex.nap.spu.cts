namespace Fanex.NAP.SPU.CTS.Tests.Features.Transform
{
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using NSubstitute;
    using Sentry;

    public class CTSCustomerUnitTest
    {
        private readonly i permissionService;
        private readonly IUserPermissionDataAccess dataAccess;
        private readonly ISentryClient sentryClient;

        public CTSCustomerUnitTest()
        {
            dataAccess = Substitute.For<IUserPermissionDataAccess>();
            sentryClient = Substitute.For<ISentryClient>();
            permissionService = new UserPermissionService(dataAccess, sentryClient);
        }
    }
}