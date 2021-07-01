namespace Fanex.NAP.SPU.CTS.Shares
{
    public static class Constants
    {
        public const int RetryPushCCToExternal = 3;

        public const string InitialProblemAccounts = "0";

        public const string ProblemAccountByDateTime = "";

        public const string FeedSoon88_Salt = "!%HGDwweHqw#";

        public const string CONVERT_TO_SQL_DATETIME = "yyyy-MM-dd HH:mm:ss";

        public const int Realtime_SleepTimes = 3000;

        public enum UPDATECUSTOMER : int
        {
            FROM_CUSTOMER = 1,
            FROM_CUSTINFO = 2
        };

        public enum MatchMonitorQueryTypes
        {
            Service = 1,
            Report = 2,
        };

        public enum SystemParameters
        {
            ProblemAccountLastScannedTime = 2,
            RobotAccountLastScannedTime = 3,
            MatchMonitorLastScannedTrans = 20
        };

        public enum DangerLevel
        {
            ORIGINAL = 1,
            ABI = 2,
            OTHER = 3
        }

        public enum Category
        {
            NEWMEMBER = 201,
            NORMALMEMBER = 202,
            SMARTPUNTER = 203,
            HIGHRISKPUNTER = 204,
            VIP = 205,
            PROBATION = 206,
            VVIP = 213,
            ROBOT = 61
        };
    }
}