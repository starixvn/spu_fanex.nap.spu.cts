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

        public enum MatchMonitorQueryTypes
        {
            Service = 1,
            Report = 2,
        };

        public enum SystemParameters
        {
            ProblemAccountLastScannedTime = 2,
            RobotAccountLastScannedTime = 3,
            MatchMonitorLastScannedTrans = 20,
            MatchMonitorLastScannedTransNoneLive = 29,
            ArchiveCustomerLastTime_Credit = 24,
            ArchiveCustomerLastCustID_Credit = 25,
            ArchiveCustomerLastTime_Deposit = 26,
            ArchiveCustomerLastCustID_Deposit = 27
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
            FromCustomer = 1,
            FromCustInfo = 2,
            FromProductStatus = 3
        };
        
        public enum SubscriberType
        {
            Credit = 0,
            Deposit = 1
        };
    }
}