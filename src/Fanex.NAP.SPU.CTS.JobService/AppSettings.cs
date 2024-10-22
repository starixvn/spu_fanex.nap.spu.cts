namespace Fanex.NAP.SPU.CTS.JobService
{
    using Fanex.NAP.SPU.CTS.Shares.Models;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.ComponentModel;

    public interface IAppSettings
    {
        int Environment { get; }

        string EvironmentName { get; }

        string SPName { get; }

        bool MatchMonitorIsWriteLog { get; }

        int SiteId { get; }

        bool UserPermissionIsWriteLog { get; }

        int UpdateLastLoginBatchSize { get; }

        bool TransformCTSCustomerIsWriteLog { get; }

        int TransformCTSCustomerBatchSize { get; }

        bool UpdateIsWriteLogSetting(string serviceName, bool isWriteLog);

        bool TransformAssByDeviceIsWriteLog { get; }

        int CustomerStatusBatchSize { get; }

        string SentryDsn { get; }

        APIHelperConfig APIHelperConfig { get; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        private readonly string environment;
        private readonly string evironmentName;
        private readonly string spName;
        private readonly string sentryDsn;
        private readonly APIHelperConfig apiHelperConfig;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            environment = GetValue<string>(nameof(Environment));
            evironmentName = GetValue<string>(nameof(Environment));
            spName = GetValue<string>(nameof(SPName));
            sentryDsn = GetValue<string>("Sentry:Dsn");
            apiHelperConfig = new APIHelperConfig
            {
                Proxy = GetValue<string>("APIHelperConfig:Proxy")
            };
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

        public string EvironmentName => evironmentName;

        public string SPName => spName;

        public bool MatchMonitorIsWriteLog => GetValue<bool>("MatchMonitor:IsWriteLog");

        public int SiteId => GetValue<int>("UserPermission:SiteId");

        public int UpdateLastLoginBatchSize => GetValue<int>("UpdateLastLoginTime:BatchSize");

        public bool UserPermissionIsWriteLog => GetValue<bool>("UserPermission:IsWriteLog");

        public bool TransformCTSCustomerIsWriteLog => GetValue<bool>("TransformCTSCustomer:IsWriteLog");

        public int TransformCTSCustomerBatchSize => GetValue<int>("TransformCTSCustomer:BatchSize");

        public bool TransformAssByDeviceIsWriteLog => GetValue<bool>("TransformAssociationByDevice:IsWriteLog");

        public int CustomerStatusBatchSize => GetValue<int>("CustomerStatus:BatchSize");

        public string SentryDsn => sentryDsn;

        public APIHelperConfig APIHelperConfig => apiHelperConfig;
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

        #endregion Methods
    }
}