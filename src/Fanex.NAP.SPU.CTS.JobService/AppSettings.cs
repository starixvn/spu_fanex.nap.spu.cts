namespace Fanex.NAP.SPU.CTS.JobService
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.ComponentModel;

    public interface IAppSettings
    {
        int Environment { get; }

        string SPName { get; }

        string Soon88FeedDataAPI { get; }

        int MatchMonitorNumberOfThread { get; }

        bool MatchMonitorIsWriteLog { get; }

        bool ProblemAccountIsWriteLog { get; }

        bool NormalAccountIsWriteLog { get; }

        int SiteId { get; }

        bool UserPermissionIsWriteLog { get; }

        int UpdateLastLoginBatchSize { get; }

        bool IsDailyScanningInterrupt { get; }

        bool RobotAccountIsWriteLog { get; }

        string RobotAccountCreditSites { get; }

        bool RemoveAccountEvidenceIsWriteLog { get; }

        bool UpdateIsWriteLogSetting(string serviceName, bool isWriteLog);

        bool InterruptDailyScanningJob(bool isInterrupted);
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        private readonly string environment;
        private readonly string spName;
        private readonly int matchMonitorNumberOfThread;
        private readonly string soon88FeedDataAPI;
        private readonly bool isDailyScanningInterrupt;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            environment = GetValue<string>(nameof(Environment));
            spName = GetValue<string>(nameof(SPName));
            soon88FeedDataAPI = GetValue<string>(nameof(Soon88FeedDataAPI));
            matchMonitorNumberOfThread = GetValue<int>("MatchMonitor:NumberOfThread");
            isDailyScanningInterrupt = GetValue<bool>("DailyClassification:IsInterruptJob");
        }

        #region Variables

        public int Environment
        {
            get
            {
                switch (environment)
                {
                    case "LOCAL":
                        return 1;

                    case "UAT":
                        return 2;

                    default:
                        return 3;
                }
            }
        }

        public string SPName => spName;

        public string Soon88FeedDataAPI => soon88FeedDataAPI;

        public int MatchMonitorNumberOfThread => matchMonitorNumberOfThread;

        public bool IsDailyScanningInterrupt => isDailyScanningInterrupt;

        public bool MatchMonitorIsWriteLog => GetValue<bool>("MatchMonitor:IsWriteLog");

        public bool ProblemAccountIsWriteLog => GetValue<bool>("ProblemAccountInsert:IsWriteLog");

        public int SiteId => GetValue<int>("UserPermission:SiteId");

        public int UpdateLastLoginBatchSize => GetValue<int>("UpdateLastLoginTime:BatchSize");

        public bool UserPermissionIsWriteLog => GetValue<bool>("UserPermission:IsWriteLog");

        public bool NormalAccountIsWriteLog => GetValue<bool>("NormalAccountInsert:IsWriteLog");

        public bool RobotAccountIsWriteLog => GetValue<bool>("RobotAccountInsert:IsWriteLog");

        public string RobotAccountCreditSites => GetValue<string>("RobotAccountInsert:Credit-Sites");

        public bool RemoveAccountEvidenceIsWriteLog => GetValue<bool>("RemoveAccountEvidence:IsWriteLog");

        #endregion Variables

        #region Methods

        public T GetValue<T>(string key)
        {
            try
            {
                var value = configuration[key];

                var typeConverter = TypeDescriptor.GetConverter(typeof(T));

                return (T)typeConverter.ConvertFromString(value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Key: {key}", ex);
            }
        }

        public T GetSection<T>(string key)
        {
            var section = configuration.GetSection(key);

            return section.Get<T>();
        }

        public void SetValue<T>(string key, T value)
        {
            try
            {
                var typeConverter = TypeDescriptor.GetConverter(typeof(T));

                configuration[key] = typeConverter.ConvertToString(value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Key: {key}", ex);
            }
        }

        public bool UpdateIsWriteLogSetting(string serviceName, bool isWriteLog)
        {
            var key = serviceName + ":IsWriteLog";

            if (configuration[key] == null)
            {
                return false;
            }

            try
            {
                SetValue<bool>(serviceName + ":IsWriteLog", isWriteLog);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool InterruptDailyScanningJob(bool isInterrupted)
        {
            var key = "DailyClassification:IsInterruptJob";

            if (configuration[key] == null)
            {
                return false;
            }

            try
            {
                SetValue<bool>(key, isInterrupted);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}