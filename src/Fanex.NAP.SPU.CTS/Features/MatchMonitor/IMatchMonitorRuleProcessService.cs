namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Sentry;

    public interface IMatchMonitorRuleProcessService
    {

        IMatchMonitorRuleProcessService ProcessRound1();

        IMatchMonitorRuleProcessService ProcessRound2();

        IMatchMonitorRuleProcessService Complete(ulong maxSequenceID);

        void WriteLogSentryIfError(string functionName, ISentryClient sentryClient);
    }

    public interface IMatchMonitorRuleProcessService<T> : IMatchMonitorRuleProcessService
    {
        IMatchMonitorRuleProcessService Start(T matchStaging, bool isLive);
    }
}
