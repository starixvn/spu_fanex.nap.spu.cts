namespace Fanex.NAP.SPU.CTS.Tests.Infrastructure
{
    public struct MMConsts
    {
        public struct JsonString
        {
        }

        public struct PrivateMethodName
        {
            public const string GetTicketStaging = "GetTicketStaging";

            public const string InsertMatchMonitorStaging = "InsertMatchMonitorStaging";

            public const string ProcessRuleSabaGB = "ProcessRuleSabaGB";

            public const string CompleteTransRuleSabaGB = "Complete";

            public const string ProcessRuleHedging = "ProcessRuleHedging";

            public const string ProcessRuleArbitrage = "ProcessRuleArbitrage";
        }

        public struct PrivateFieldName
        {
            public const string MatchProcesses = "MatchProcesses";

            public const string IsLive = "IsLive";

            public const string MatchStaging = "MatchStaging";
        }
    }
}
