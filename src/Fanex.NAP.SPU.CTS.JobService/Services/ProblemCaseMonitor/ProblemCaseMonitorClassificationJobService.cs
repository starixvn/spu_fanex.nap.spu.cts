namespace Fanex.NAP.SPU.CTS.JobService.Services.ProblemCaseMonitor
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IProblemCaseMonitorClassificationJobService
    {
        Task<bool> Execute();
    }

    public class ProblemCaseMonitorClassificationJobService : IProblemCaseMonitorClassificationJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IProblemCaseMonitorService problemCaseMonitorService;

        public ProblemCaseMonitorClassificationJobService(IProblemCaseMonitorService problemCaseMonitorService, ISentryClient sentryClient)
        {
            this.problemCaseMonitorService = problemCaseMonitorService;
            this.sentryClient = sentryClient;
        }

        public Task<bool> Execute()
        {
            try
            {
                var problemCase = this.problemCaseMonitorService.GetProblemCaseMonitorClassification();

                var updateResult = this.problemCaseMonitorService.InsertProblemCaseMonitorClassification(problemCase);               
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                        .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error($"CTS - Problem Case Monitor Classification job Execute exeption message:  {logMessage}");

                sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);

                return Task.FromResult(false);                
            }

            return Task.FromResult(true);
        }

    }
}
