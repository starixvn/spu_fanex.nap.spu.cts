namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.Accounts;
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProblemAccountInsert;
    using Fanex.NAP.SPU.CTS.Features.ExistingCustomer;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Microsoft.Extensions.DependencyInjection;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterTransformAccount(services);

            RegisterExistingCustomer(services);

            RegisterMatchMonitor(services);

            RegisterProblemAccount(services);

            RegisterUserPermission(services);
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
    }
}
