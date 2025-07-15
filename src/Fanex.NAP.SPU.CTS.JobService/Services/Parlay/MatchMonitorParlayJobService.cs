namespace Fanex.NAP.SPU.CTS.JobService.Services.Parlay
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMatchMonitorParlayJobService
    {
        Task<bool> InsertParlayTicketDetail(int batchSize, bool isLive);

        Task<bool> ProcessParlayRuleGroupBetting(bool isLive, int numberOfThread);
    }

    public class MatchMonitorParlayJobService : IMatchMonitorParlayJobService
    {
        private readonly string _serviceName;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMMParlayRuleGroupBettingService _mmParlayRuleGBService;

        public MatchMonitorParlayJobService(
            IServiceProvider serviceProvider,
            IMMParlayRuleGroupBettingService mmParlayRuleGBService)
        {
            _serviceName = "Match Monitor Parlay Staging Service";
            _serviceProvider = serviceProvider;
            _mmParlayRuleGBService = mmParlayRuleGBService;
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
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> ProcessParlayRuleGroupBetting(bool isLive, int numberOfThread)
        {
            try
            {
                var mmParlayRuleGB = _mmParlayRuleGBService.GetMMParlayRuleGroupBetting(isLive);

                if (mmParlayRuleGB != null && mmParlayRuleGB.MaxSequenceID.HasValue)
                {
                    mmParlayRuleGB.MatchStagingList?
                        .AsParallel()
                        .WithDegreeOfParallelism(numberOfThread)
                        .ForAll(matchInfo =>
                        {
                            _mmParlayRuleGBService.GetInstanceParlayProcessGBService()
                                .PrepareProcess(matchInfo, isLive)
                                .ProcessRound1()
                                .ProcessRound2()
                                .Complete(mmParlayRuleGB.MaxSequenceID.Value)
                                .WriteLogSentryIfError(nameof(ProcessParlayRuleGroupBetting));

                            Thread.Sleep(Consts.MatchMonitor_SleepTimes);
                        });

                    _mmParlayRuleGBService.CleanTransRuleGroupBetting(isLive, mmParlayRuleGB.MaxSequenceID.Value);
                }

            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
