namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProblemAccountInsert;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.FeedCCToSoon88;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface IProblemAccountJobService
    {
        Task<bool> Execute(int batchExternalSize, int batchInternalSize);

        Task<bool> UpdateWriteLogSetting(bool isWriteLog);
    }

    public class ProblemAccountJobService : IProblemAccountJobService
    {
        private readonly string serviceName;
        private readonly ISentryClient sentryClient;
        private readonly IProblemAccountServices problemAccountServices;
        private readonly ISystemParameterServices systemParameterServices;
        private readonly IPushDataToExternalService pushDataToExternalService;
        private readonly IFeedSoon88Services feedSoon88Services;
        private readonly IAppSettings appSettings;

        public ProblemAccountJobService(
            ISentryClient sentryClient,
            IProblemAccountServices problemAccountServices,
            ISystemParameterServices systemParameterServices,
            IPushDataToExternalService pushDataToExternalService,
            IFeedSoon88Services feedSoon88Services,
            IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.problemAccountServices = problemAccountServices;
            this.systemParameterServices = systemParameterServices;
            this.pushDataToExternalService = pushDataToExternalService;
            this.feedSoon88Services = feedSoon88Services;
            this.appSettings = appSettings;
            this.serviceName = "ProblemAccountInsert";
        }

        public Task<bool> Execute(int batchExternalSize, int batchInternalSize)
        {
            try
            {
                if (batchExternalSize > 0 && batchInternalSize > 0)
                {
                    JobRun(batchExternalSize, batchInternalSize);
                }
                else
                {
                    Utilities.LogSentryError(serviceName, "InValid Batch size", (batchExternalSize, batchInternalSize), new NullReferenceException(), sentryClient);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> UpdateWriteLogSetting(bool isWriteLog)
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

        #region Private Methods
        private void JobRun(int batchExternalSize, int batchInternalSize)
        {
            var lastScannedTime = systemParameterServices.GetLastScannedDateTime(SystemParameters.ProblemAccountLastScannedTime);
            Utilities.LogSentryInfo(serviceName, "Get LastScannedTransId", lastScannedTime, appSettings.ProblemAccountIsWriteLog, sentryClient);

            var savedDataTime = DateTime.Now;

            var problemAccounts = problemAccountServices.GetProblemAccounts(lastScannedTime);
            Utilities.LogSentryInfo(serviceName, "Get ProblemAccount", lastScannedTime, appSettings.ProblemAccountIsWriteLog, sentryClient);

            if (problemAccounts != null && problemAccounts.Any())
            {
                var problemAccountsChunk = problemAccounts.Distinct().Chunk(batchInternalSize); 

                foreach (var items in problemAccountsChunk)
                {
                    var custIds = InsertProblemAccountToCTS(items);

                    if (custIds != null && custIds.Any())
                    {
                        PushDataToExternal(custIds, batchExternalSize);
                    }
                }
            }

            systemParameterServices.UpdateTimingExecution(savedDataTime.ToString(Constants.CONVERT_TO_SQL_DATETIME), (int)SystemParameters.ProblemAccountLastScannedTime);
            Utilities.LogSentryInfo(serviceName, "Update TimingExecution", savedDataTime.ToString(Constants.CONVERT_TO_SQL_DATETIME), appSettings.ProblemAccountIsWriteLog, sentryClient);
        }

        private void PushDataToExternal(IEnumerable<int> custIds, int batchExternalSize)
        {
            var custIdsChunk = custIds.Chunk(batchExternalSize);

            foreach (var items in custIdsChunk)
            {
                var lastestClassifications = pushDataToExternalService.GetLatestClassification(string.Join(",", items));
                Utilities.LogSentryInfo(serviceName, "Get lastestCC", lastestClassifications, appSettings.ProblemAccountIsWriteLog, sentryClient);

                if (lastestClassifications != null && lastestClassifications.Any())
                {
                    bool isSyncMainSuccessfully = PushCustomerClassificationToMain(lastestClassifications);

                    if (isSyncMainSuccessfully)
                    {
                        UpdateAppliedCC(lastestClassifications);
                    }

                    feedSoon88Services.FeedCCToSoon88(lastestClassifications, appSettings.Soon88FeedData_URL);
                    Utilities.LogSentryInfo(serviceName, "FeedSoon88 done", lastestClassifications, appSettings.ProblemAccountIsWriteLog, sentryClient);

                }
            }
        }

        private IEnumerable<int> InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts)
        {
            var result = problemAccountServices.InsertProblemAccountToCTS(problemAccounts, false);
            Utilities.LogSentryInfo(serviceName, "Insert ProblemAccount", problemAccounts, appSettings.ProblemAccountIsWriteLog, sentryClient);

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                Utilities.LogSentryError(serviceName, "InsertProblemAccount throws Exception", problemAccounts, new Exception(result.ErrorMessage), sentryClient);

            }

            return result.CustIds;

        }

        private void UpdateAppliedCC(IEnumerable<LatestClassificationEntity> lastestClassifications)
        {
            var rs = pushDataToExternalService.UpdateAppliedCustomerClassification(lastestClassifications);
            Utilities.LogSentryInfo(serviceName, "Update Apllied CC", lastestClassifications, appSettings.ProblemAccountIsWriteLog, sentryClient);

            if (rs.ErrorCode == -1)
            {
                Utilities.LogSentryError(serviceName, "Update Apllied CC throw exception", lastestClassifications, new Exception(rs.ErrorMsg), sentryClient);
            }
        }

        private bool PushCustomerClassificationToMain(IEnumerable<LatestClassificationEntity> lastestClassifications)
        {
            var result = pushDataToExternalService.PushCustomerClassificationToMainDB(lastestClassifications, appSettings.ProblemAccountRetryTimes);
            Utilities.LogSentryInfo(serviceName, "PushCustomerClassificationToMain", lastestClassifications, appSettings.ProblemAccountIsWriteLog, sentryClient);

            if (result.ErrorCode == -1)
            {
                Utilities.LogSentryError(serviceName, "PushCustomerClassificationToMain throws Exception", lastestClassifications, new Exception(result.ErrorMsg), sentryClient);
            }

            return result.ErrorCode == 0;
        }

        #endregion
    }
}

