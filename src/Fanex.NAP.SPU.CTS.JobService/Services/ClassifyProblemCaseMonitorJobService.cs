namespace Fanex.NAP.SPU.CTS.JobService.Services
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

    public interface IClassifyProblemCaseMonitorJobService
    {
        Task<bool> Execute();
    }

    public class ClassifyProblemCaseMonitorJobService : IClassifyProblemCaseMonitorJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IProblemCaseClassificationService problemCaseClassificationService;

        public ClassifyProblemCaseMonitorJobService(IProblemCaseClassificationService problemCaseClassificationService, ISentryClient sentryClient)
        {
            this.problemCaseClassificationService = problemCaseClassificationService;
            this.sentryClient = sentryClient;
        }

        public Task<bool> Execute()
        {
            try
            {
                var problemCase = this.problemCaseClassificationService.GetProblemCaseMonitorClassification();

                var updateResult = this.problemCaseClassificationService.InsertProblemCaseMonitorClassification(problemCase);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                var logMessage = new LogMessage(ex.Message)
                        .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error($"CTS - Classify Problem Case Monitor job Execute exeption message:  {logMessage}");

                return Task.FromResult(false);                
            }

            return Task.FromResult(true);
        }

    }
}
