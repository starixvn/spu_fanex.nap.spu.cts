namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.Client.AspNetCore.Session;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.Accounts;
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.NormalAccountInsert;
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProblemAccountInsert;
    using Fanex.NAP.SPU.CTS.Features.ExistingCustomer;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.FeedCCToSoon88;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Features.UpdateLastLogin;
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterNAPService(services);

            RegisterTransformAccount(services);

            RegisterExistingCustomer(services);

            RegisterMatchMonitor(services);

            RegisterProblemAccount(services);

            RegisterUserPermission(services);

            RegisterUpdateLastLogin(services);

            RegisterDataAccesLayer(services);

            RegisterNormalAccount(services);
        }

        private static void RegisterNAPService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<INapSession, NapSession>();
            services.AddSingleton<ISecurityService, SecurityService>();
        }

        private static void RegisterTransformAccount(this IServiceCollection services)
        {
            services.AddSingleton<IAccountDataAccess, AccountDataAccess>();
            services.AddSingleton<IAccountServices, AccountServices>();
            services.AddSingleton<IAccountJobService, AccountJobService>();
        }

        private static void RegisterExistingCustomer(this IServiceCollection services)
        {
            services.AddSingleton<IExistingCustomerDataAccess, ExistingCustomerDataAccess>();
            services.AddSingleton<IExistingCustomerServices, ExistingCustomerServices>();
            services.AddSingleton<IExistingCustomerJobService, ExistingCustomerJobService>();
        }

        private static void RegisterMatchMonitor(this IServiceCollection services)
        {
            services.AddSingleton<IMatchMonitorDataAccess, MatchMonitorDataAccess>();
            services.AddSingleton<IMatchMonitorServices, MatchMonitorServices>();
            services.AddSingleton<IMatchMonitorJobService, MatchMonitorJobService>();
        }

        private static void RegisterProblemAccount(this IServiceCollection services)
        {
            services.AddSingleton<IProblemAccountDataAccess, ProblemAccountDataAccess>();
            services.AddSingleton<IProblemAccountServices, ProblemAccountServices>();
            services.AddSingleton<IProblemAccountJobService, ProblemAccountJobService>();
        }

        private static void RegisterUserPermission(this IServiceCollection services)
        {
            services.AddSingleton<IUserPermissionService, UserPermissionService>();
            services.AddSingleton<IUserPermissionJobService, UserPermissionJobService>();
            services.AddSingleton<IUserPermissionDataAccess, UserPermissionDataAccess>();
        }

        private static void RegisterUpdateLastLogin(this IServiceCollection services)
        {
            services.AddSingleton<IUpdateLastLoginJobService, UpdateLastLoginJobService>();
            services.AddSingleton<IUpdateLastLoginService, UpdateLastLoginService>();
            services.AddSingleton<IUpdateLastLoginDataAccess, UpdateLastLoginDataAccess>();
        }
        
        private static void RegisterNormalAccount(this IServiceCollection services)
        {
            services.AddSingleton<INormalAccountDataAccess, NormalAccountDataAccess>();
            services.AddSingleton<INormalAccountServices, NormalAccountServices>();
            services.AddSingleton<INormalAccountJobService, NormalAccountJobService>();
        }

        private static void RegisterDataAccesLayer(this IServiceCollection services)
        {
            services.AddSingleton<ISystemParameterServices, SystemParameterServices>();
            services.AddSingleton<IPreviousParameterDataAccess, PreviousParameterDataAccess>();

            services.AddSingleton<ICustClassToMainDBDataAccess, CustClassToMainDBDataAccess>();
            services.AddSingleton<ICustClassToMainDBService, CustClassToMainDBService>();

            services.AddSingleton<IFeedSoon88DataAccess, FeedSoon88DataAccess>();
            services.AddSingleton<IFeedSoon88Services, FeedSoon88Services>();
        }
    }
}