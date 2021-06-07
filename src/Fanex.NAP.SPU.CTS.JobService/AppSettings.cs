namespace Fanex.NAP.SPU.CTS.JobService
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.ComponentModel;

    public interface IAppSettings
    {
        int Environment { get; }                

        string SPName { get; }

        int MatchMonitorNumberOfThread { get; }

        bool MatchMonitorIsWriteLog { get; }

        bool UpdateIsWriteLogSetting(string serviceName, bool isWriteLog);
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        private readonly string environment;
        private readonly string spName;
        private readonly int matchMonitorNumberOfThread;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            environment = GetValue<string>(nameof(Environment));
            spName = GetValue<string>(nameof(SPName));

            matchMonitorNumberOfThread = GetValue<int>("MatchMonitor:NumberOfThread");
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

        public int MatchMonitorNumberOfThread => matchMonitorNumberOfThread;

        public bool MatchMonitorIsWriteLog => GetValue<bool>("MatchMonitor:IsWriteLog");
      
        #endregion

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
            var key = serviceName + ":isWriteLog";

            if (configuration[key] == null)  return false;

            try
            {
                SetValue<bool>(serviceName + ":isWriteLog", isWriteLog);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
