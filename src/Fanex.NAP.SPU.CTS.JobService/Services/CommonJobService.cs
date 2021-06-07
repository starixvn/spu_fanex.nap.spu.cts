namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface ICommonJobServices
    {
        Task<bool> UpdateWriteLogSetting(string serviceName, bool isWriteLog);

        void LogSentryInfo<T>(string serviceName, string message, T data);

        void LogSentryEror<T>(string serviceName, string message, T data, Exception ex);
    }

    public class CommonJobServices : ICommonJobServices
    {
        private readonly IAppSettings appSettings;
        private readonly ISentryClient sentryClient;

        public CommonJobServices(ISentryClient sentryClient, IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.appSettings = appSettings;
        }

        public Task<bool> UpdateWriteLogSetting(string serviceName, bool isWriteLog)
        {
            try
            {
                bool result = appSettings.UpdateIsWriteLogSetting(serviceName, isWriteLog);

                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                sentryClient.CaptureException(e);
                return Task.FromResult(false);
            }
        }

        public void LogSentryInfo<T>(string serviceName, string message, T data)
        {
            if(appSettings.MatchMonitorIsWriteLog)
            {
                var logMessage = $"{serviceName} - {message} {(data != null ? " - Data:" + JsonConvert.SerializeObject(data) : "")} - {DateTime.Now}";

                sentryClient.CaptureMessage(logMessage);
            }
        }

        public void LogSentryEror<T>(string serviceName, string message, T data, Exception ex)
        {
            var logMessage = new LogMessage(ex.Message)
                .AddMessage($"{serviceName}")
                .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                .AddMessage($"{message} - Data {JsonConvert.SerializeObject(data)} - { DateTime.Now}").ToString();

            sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);
        }
    }
}
