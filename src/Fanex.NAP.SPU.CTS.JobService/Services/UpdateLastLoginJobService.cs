namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.UpdateLastLogin;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Threading.Tasks;

    public interface IUpdateLastLoginJobService
    {
        Task<bool> UpdateLastLoginTime();
    }

    public class UpdateLastLoginJobService : IUpdateLastLoginJobService
    {
        private readonly IUpdateLastLoginService uptLastLoginService;
        private readonly IAppSettings appSettings;

        public UpdateLastLoginJobService(IUpdateLastLoginService uptLastLoginService, IAppSettings appSettings)
        {
            this.uptLastLoginService = uptLastLoginService;
            this.appSettings = appSettings;
        }


        public Task<bool> UpdateLastLoginTime()
        {
            try
            {
                uptLastLoginService.UpdateLastLoginTime(appSettings.UpdateLastLoginBatchSize);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return Task.FromResult(false);
            }
        }
    }
}
