namespace Fanex.NAP.SPU.CTS.Shares
{
    public static class Consts
    {
        public const int RetryPushCCToExternal = 3;

        public const string InitialProblemAccounts = "0";

        public const string ProblemAccountByDateTime = "";

        public const string FeedSoon88_Salt = "!%HGDwweHqw#";

        public const string CONVERT_TO_SQL_DATETIME = "yyyy-MM-dd HH:mm:ss";

        public const int Realtime_SleepTimes = 3000;

        public const int RemoveEvidences_ClearActionId = 3;

        public const int MatchMonitor_SleepTimes = 5;

        public const int AffectedEvidence_SleepTimes = 1;

        public const int ArchiveCustClassification_SleepTimes = 10000;

        public const int ExpiredHttpRequest = 85500000; //23h 45p

        public const string MicrosoftTeams_GroupId_Local = "19:00af2ff331b64d39adf13b428aa49c6e@thread.tacv2";

        public const string MicrosoftTeams_Notification_DBAMonitoring = "19:8716ea65006c45f8a7061db1cc5acd86@thread.tacv2";

        public enum MatchMonitorQueryTypes
        {
            Service = 1
        };

        public enum SystemParameters
        {
            ProblemAccountLastScannedTime = 2,
            RobotAccountLastScannedTime = 3,
            MatchMonitorLastScannedTrans = 20,
            MatchMonitorLastScannedTransNoneLive = 29,
            MatchMonitorLastScannedRuleProcessLive = 47,
            MatchMonitorLastScannedRuleProcessNonLive = 48,
            ArchiveCustomerLastTime_Credit = 24,
            ArchiveCustomerLastCustID_Credit = 25,
            ArchiveCustomerLastTime_Deposit = 26,
            ArchiveCustomerLastCustID_Deposit = 27,
            AssociatedGroupLastGroupID = 98,
            AssociatedGroupLastCTSCustID = 99,
            CorrectArchiveCustomer_LastArchiveDate_Credit = 1001,
            CorrectArchiveCustomer_LastCustID_Credit = 1002,
            CorrectArchiveCustomer_MaxArchiveDate_Credit = 1003,
            CorrectArchiveCustomer_LastArchiveDate_Deposit = 1004,
            CorrectArchiveCustomer_LastCustID_Deposit = 1005,
            CorrectArchiveCustomer_MaxArchiveDate_Deposit = 1006,
        };

        public enum DangerLevel
        {
            Original = 1,
            ABI = 2,
            Other = 3
        }

        public enum Category
        {
            Newmember = 201,
            NormalMember = 202,
            SmartPunter = 203,
            HighRishPunter = 204,
            VIP = 205,
            Probation = 206,
            VVIP = 213,
            Robot = 61
        };

        public enum CustomerRole
        {
            Member = 1,
            Agent = 2,
            Master = 3,
            Super = 4
        };

        public enum FlowTypeRescanCC
        {
            NewMember,
            RemoveMultipleEvidence,
            Other
        };

        public enum UpdateCustomer
        {
            ForNewCustomer = 0,
            FromCustomer = 1,
            FromCustInfo = 2,
            FromProductStatus = 3
        };

        public enum SubscriberType
        {
            Credit = 0,
            Deposit = 1
        };

        public enum DetectAssociation_QueryType
        {
            DetectAss = 1,
            GetTicketDetail = 2,
            GetAssByGroup = 3,
            GetTicketDetailByListCust = 4
        }

        public enum MatchMonitor_SportType
        {
            Soccer = 1,
            Basketball = 2,
            Esport = 43
        }

        public enum BetTypeSettingsFunctionID
        {
            MMBetType = 1
        }

        public enum JobMonitor_IssueType
        {
            NewCustomer_NullCategory = 1,
            NewCustomer_Missing = 2,
            Realtime_NewPlaceBetInactiveCategory = 100,
            Realtime_NullCategory = 101,
            Classification_GeneralPoolStatus = 200,
            Classification_BySportPoolStatus = 201,
            DCSTransaction_LastLoginProcessStatus = 500
        };

        public enum AssociationDetectionType
        {
            Device = 1,
            AI = 2,
            IP = 4
        }

        public enum MatchMonitorStagingLivePoolType
        {
            MatchMonitorStagingGroupBettingLive         = 1001,
            MatchMonitorStagingGroupBettingSabaLive     = 1002,
            MatchMonitorStagingHedgingLive              = 1003,
            MatchMonitorStagingFixedGameLive            = 1004
        }

        public enum MatchMonitorStagingNonLivePoolType
        {
            MatchMonitorStagingGroupBettingNonLive      = 2001,
            MatchMonitorStagingGroupBettingSabaNonLive  = 2002,
            MatchMonitorStagingHedgingNonLive           = 2003,
            MatchMonitorStagingArbitrageNonLive         = 2004,
            MatchMonitorStagingFixedGameNonLive         = 2005,
            MatchMonitorStagingIrrigationNonLive        = 2006
        }
    }
}