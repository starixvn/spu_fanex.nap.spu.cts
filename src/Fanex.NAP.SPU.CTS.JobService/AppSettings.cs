using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;

namespace Fanex.NAP.SPU.CTS.JobService
{
    public interface IAppSettings
    {
        int Environment { get; }                

        string SPName { get; }           
        
        int NumberOfThread { get; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        private readonly string environment;        
        private readonly string spName;
        private readonly int numberOfThread;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            environment = GetValue<string>(nameof(Environment));                       

            spName = GetValue<string>(nameof(SPName));

            numberOfThread = GetValue<int>(nameof(NumberOfThread));
        }

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

        public int NumberOfThread => numberOfThread;

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
            var schedulerSection = configuration.GetSection(key);

            return schedulerSection.Get<T>();
        }
    }
}
