using Fanex.NAP.SPU.CTS.Features.Accounts;
using Fanex.NAP.SPU.CTS.Features.ExistingCustomer;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterTransformAccount(services);

            RegisterExistingCustomer(services);
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
    }
}
