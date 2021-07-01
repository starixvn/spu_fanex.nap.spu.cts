namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProblemAccountInsert;
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.RobotAccountInsert;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.FeedCCToSoon88;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface IRobotAccountInsertJobService
    {
        Task<bool> Execute(int batchSize);
    }

    public class RobotAccountInsertJobService : IRobotAccountInsertJobService
    {
        private readonly string serviceName;
        private readonly ISentryClient sentryClient;
        private readonly IRobotAccountInsertServices robotAccountInsertServices;
        private readonly ISystemParameterServices systemParameterServices;
        private readonly ICustClassToMainDBService custClassToMainDBService;
        private readonly IProblemAccountServices problemAccountServices;
        private readonly IFeedSoon88Services feedSoon88Services;
        private readonly IAppSettings appSettings;
        private readonly bool isWriteLog;

        public RobotAccountInsertJobService(
            ISentryClient sentryClient,
            IRobotAccountInsertServices robotAccountInsertServices,
            ISystemParameterServices systemParameterServices,
            ICustClassToMainDBService custClassToMainDBService,
            IProblemAccountServices problemAccountServices,
            IFeedSoon88Services feedSoon88Services,
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.robotAccountInsertServices = robotAccountInsertServices;
            this.systemParameterServices = systemParameterServices;
            this.custClassToMainDBService = custClassToMainDBService;
            this.problemAccountServices = problemAccountServices;
            this.feedSoon88Services = feedSoon88Services;
            this.appSettings = appSettings;
            this.isWriteLog = appSettings.RobotAccountIsWriteLog;
            this.serviceName = "RobotAccountInsert";
        }

        public Task<bool> Execute(int batchSize)
        {
            try
            {
                if (batchSize > 0)
                {
                    JobRun(batchSize);
                }
                else
                {
                    Utilities.LogSentryError(serviceName, "InValid Batch size", (batchSize), new NullReferenceException(), sentryClient);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        #region Private Methods

        private void JobRun(int batchSize)
        {
            var savedExecutionTime = DateTime.Now.ToString(Constants.CONVERT_TO_SQL_DATETIME);

            var lastScannedTime = systemParameterServices.GetLastScannedDateTime(SystemParameters.RobotAccountLastScannedTime);

            var robotUsers = robotAccountInsertServices.GetRobotUser(lastScannedTime, appSettings.RobotAccountCreditSites);

            if (robotUsers != null && robotUsers.Any())
            {
                var robotUsersChunks = robotUsers.Chunk(batchSize).ToList();

                foreach (var robot in robotUsersChunks)
                {
                    var custCategory = robotAccountInsertServices.MappingRobotCategory(robot);

                    var custIds = problemAccountServices.InsertProblemAccountToCTS(custCategory, false, serviceName, isWriteLog);

                    if (custIds != null && custIds.Any())
                    {
                        PushDataToExternalAsync(custIds);
                    }
                }
            }

            systemParameterServices.UpdateTimingExecution(savedExecutionTime, SystemParameters.RobotAccountLastScannedTime);
        }

        private void PushDataToExternalAsync(IEnumerable<int> custIds)
        {
            if (custIds != null && custIds.Any())
            {
                var lastestClassification = custClassToMainDBService.GetLatestClassification(custIds, serviceName, isWriteLog);

                if (lastestClassification != null && lastestClassification.Any())
                {
                    var tasks = new List<Task>
                        {
                            Task.Run(() =>
                            {
                                custClassToMainDBService.PushCustClassificationToMainDB(lastestClassification, serviceName, isWriteLog);
                            }),

                            Task.Run(() =>
                            {
                                feedSoon88Services.FeedCCToSoon88(lastestClassification, appSettings.Soon88FeedDataAPI, serviceName, isWriteLog);
                            })
                        };

                    Task.WhenAll(tasks.ToArray()).Wait();

                    Thread.Sleep(300);
                }
            }
        }
    }

    #endregion Private Methods
}