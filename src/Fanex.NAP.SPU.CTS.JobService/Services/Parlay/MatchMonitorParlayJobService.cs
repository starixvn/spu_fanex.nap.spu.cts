namespace Fanex.NAP.SPU.CTS.JobService.Services.Parlay
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMatchMonitorParlayJobService
    {
        Task<bool> InsertParlayTicketDetail(int batchSize, bool isLive);
    }

    public class MatchMonitorParlayJobService : IMatchMonitorParlayJobService
    {
        private readonly ISentryClient _sentryClient;
        private readonly string _serviceName;
        private readonly IServiceProvider _serviceProvider;

        public MatchMonitorParlayJobService(ISentryClient sentryClient, IServiceProvider serviceProvider)
        {
            _serviceName = "Match Monitor Parlay Staging Service";
            _sentryClient = sentryClient;
            _serviceProvider = serviceProvider;
        }

        public Task<bool> InsertParlayTicketDetail(int batchSize, bool isLive)
        {
            try
            {
                _serviceProvider.GetService<IMMParlayStagingService>()
                    .Start(batchSize, isLive)
                    .GetLastScannedSequenceID()
                    .GetParlayTicketStaging()
                    .InsertParlayTicketStaging()
                    .UpdateLastScannedSequenceID();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
