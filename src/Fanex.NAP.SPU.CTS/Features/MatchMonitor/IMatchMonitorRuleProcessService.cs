namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Models;
    using System.Collections.Generic;

    public interface IMatchMonitorRuleProcessService
    {
        IMatchMonitorRuleProcessService ProcessRound1();

        IMatchMonitorRuleProcessService ProcessRound2();

        IMatchMonitorRuleProcessService Complete(ulong maxSequenceID);

        void WriteLogSentryIfError(string functionName);
    }

    public interface IMatchMonitorRuleProcessService<T> : IMatchMonitorRuleProcessService
    {
        virtual IMatchMonitorRuleProcessService PrepareProcess(T matchStaging, bool isLive, IReadOnlyDictionary<int, int> mappingCustDictionary = null) => this;

        virtual IMatchMonitorRuleProcessService PrepareProcess(T matchStaging, bool isLive, MatchDetectAssociationSettingModel assDetectSetting) => this;

        virtual IMatchMonitorRuleProcessService PrepareProcessWithThread(T matchStaging, bool isLive, int numberInternalOfThread) => this;
    }
}
