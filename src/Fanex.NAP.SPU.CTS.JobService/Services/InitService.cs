using Fanex.NAP.SPU.CTS.Features.Accounts;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.NAP.SPU.DCS.JobService.Services
{
    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterTransformAccount(services);            
        }

        private static void RegisterTransformAccount(this IServiceCollection services)
        {
            services.AddSingleton<IAccountDataAccess, AccountDataAccess>();
            services.AddSingleton<IAccountServices, AccountServices>();
            services.AddSingleton<IAccountJobService, AccountJobService>();
        }
    }
}
