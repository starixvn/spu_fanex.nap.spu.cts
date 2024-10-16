namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay
{
    using Sentry;
    using System.Collections.Generic;

    public interface IMMParlayRuleProcessService
    {
        IMMParlayRuleProcessService ProcessRound1();

        IMMParlayRuleProcessService ProcessRound2();

        IMMParlayRuleProcessService Complete(ulong maxSequenceID);

        void WriteLogSentryIfError(string functionName, ISentryClient sentryClient);
    }

    public interface IMMParlayRuleProcessService<T> : IMMParlayRuleProcessService
    {
        virtual IMMParlayRuleProcessService PrepareProcess(T matchStaging, bool isLive, IReadOnlyDictionary<int, int> mappingCustDictionary = null) => this;

        virtual IMMParlayRuleProcessService PrepareProcessWithThread(T matchStaging, bool isLive, int numberInternalOfThread) => this;
    }
}
