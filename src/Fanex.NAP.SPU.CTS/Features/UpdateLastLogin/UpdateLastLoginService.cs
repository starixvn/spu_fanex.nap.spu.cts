namespace Fanex.NAP.SPU.CTS.Features.UpdateLastLogin
{
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUpdateLastLoginService
    {
        void UpdateLastLoginTime(int batchSize);
    }

    public class UpdateLastLoginService : IUpdateLastLoginService
    {
        private readonly IUpdateLastLoginDataAccess dataAccess;
        private readonly ISentryClient sentryClient;

        public UpdateLastLoginService(IUpdateLastLoginDataAccess dataAccess, ISentryClient sentryClient)
        {
            this.dataAccess = dataAccess;
            this.sentryClient = sentryClient;
        }

        public void UpdateLastLoginTime(int batchSize)
        {
            dataAccess.TransformUpdatingLastLoginTime(batchSize);
        }
    }
}
