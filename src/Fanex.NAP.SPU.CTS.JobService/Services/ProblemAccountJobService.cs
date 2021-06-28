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
        private readonly ISentryClient _sentryClient;
        private readonly IProblemAccountServices _problemAccountServices;
        private readonly ISystemParameterServices _systemParameterServices;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IFeedSoon88Services _feedSoon88Services;
        private readonly IAppSettings _appSettings;
        private IEnumerable<ProblemAccountsEntity> logData;

        public ProblemAccountJobService(
            ISentryClient _sentryClient,
            IProblemAccountServices _problemAccountServices,
            ISystemParameterServices _systemParameterServices,
            ICustClassToMainDBService _pushDataToExternalService,
            IFeedSoon88Services _feedSoon88Services,
            IAppSettings _appSettings)
        {
            this._sentryClient = _sentryClient;
            this._problemAccountServices = _problemAccountServices;
            this._systemParameterServices = _systemParameterServices;
            this._custClassToMainDBService = _pushDataToExternalService;
            this._feedSoon88Services = _feedSoon88Services;
            this._appSettings = _appSettings;
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
                    Utilities.LogSentryError(serviceName, "InValid Batch size", (batchExternalSize, batchInternalSize), new NullReferenceException(), _sentryClient);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, logData, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> UpdateWriteLogSetting(bool isWriteLog)
        {
            try
            {
                bool result = _appSettings.UpdateIsWriteLogSetting(serviceName, isWriteLog);

                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                _sentryClient.CaptureException(e);
                return Task.FromResult(false);
            }
        }

        #region Private Methods
        private void JobRun(int batchExternalSize, int batchInternalSize)
        {
            var lastScannedTime = _systemParameterServices.GetLastScannedDateTime(SystemParameters.ProblemAccountLastScannedTime);
            Utilities.LogSentryInfo(serviceName, "Get LastScannedTransId", lastScannedTime, _appSettings.ProblemAccountIsWriteLog, _sentryClient);

            var savedDataTime = DateTime.Now;

            var problemAccounts = _problemAccountServices.GetProblemAccounts(lastScannedTime).ToList();
            logData = problemAccounts;
            Utilities.LogSentryInfo(serviceName, "Get ProblemAccount", problemAccounts, _appSettings.ProblemAccountIsWriteLog, _sentryClient);

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

            _systemParameterServices.UpdateTimingExecution(savedDataTime.ToString(Constants.CONVERT_TO_SQL_DATETIME), (int)SystemParameters.ProblemAccountLastScannedTime);
            Utilities.LogSentryInfo(serviceName, "Update TimingExecution", savedDataTime.ToString(Constants.CONVERT_TO_SQL_DATETIME), _appSettings.ProblemAccountIsWriteLog, _sentryClient);
        }

        private void PushDataToExternal(IEnumerable<int> custIds, int batchExternalSize)
        {
            var custIdsChunk = custIds.Chunk(batchExternalSize);

            foreach (var items in custIdsChunk)
            {
                var lastestClassifications = _custClassToMainDBService.GetLatestClassification(string.Join(",", items));
                Utilities.LogSentryInfo(serviceName, "Get lastestCC", lastestClassifications, _appSettings.ProblemAccountIsWriteLog, _sentryClient);

                if (lastestClassifications != null && lastestClassifications.Any())
                {
                    bool isSyncMainSuccessfully = PushCustomerClassificationToMain(lastestClassifications);

                    if (isSyncMainSuccessfully)
                    {
                        UpdateAppliedCC(lastestClassifications);
                    }

                    _feedSoon88Services.FeedCCToSoon88(lastestClassifications, _appSettings.Soon88FeedData_URL);
                    Utilities.LogSentryInfo(serviceName, "FeedSoon88 done", lastestClassifications, _appSettings.ProblemAccountIsWriteLog, _sentryClient);
                }
            }
        }

        private IEnumerable<int> InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts)
        {
            var result = _problemAccountServices.InsertProblemAccountToCTS(problemAccounts, false);
            Utilities.LogSentryInfo(serviceName, "Insert ProblemAccount", result, _appSettings.ProblemAccountIsWriteLog, _sentryClient);

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                Utilities.LogSentryError(serviceName, "InsertProblemAccount throws Exception", problemAccounts, new Exception(result.ErrorMessage), _sentryClient);

            }

            return result.CustIds;

        }

        private void UpdateAppliedCC(IEnumerable<LatestClassificationEntity> lastestClassifications)
        {
            var rs = _custClassToMainDBService.UpdateAppliedCustomerClassification(lastestClassifications);
            Utilities.LogSentryInfo(serviceName, "Update Apllied CC", lastestClassifications, _appSettings.ProblemAccountIsWriteLog, _sentryClient);

            if (rs.ErrorCode == -1)
            {
                Utilities.LogSentryError(serviceName, "Update Apllied CC throw exception", lastestClassifications, new Exception(rs.ErrorMsg), _sentryClient);
            }
        }

        private bool PushCustomerClassificationToMain(IEnumerable<LatestClassificationEntity> lastestClassifications)
        {
            var result = _custClassToMainDBService.PushCustomerClassificationToMainDB(lastestClassifications, _appSettings.ProblemAccountRetryTimes);
            Utilities.LogSentryInfo(serviceName, "PushCustomerClassificationToMain", lastestClassifications, _appSettings.ProblemAccountIsWriteLog, _sentryClient);

            if (result.ErrorCode == -1)
            {
                Utilities.LogSentryError(serviceName, "PushCustomerClassificationToMain throws Exception", lastestClassifications, new Exception(result.ErrorMsg), _sentryClient);
            }

            return result.ErrorCode == 0;
        }

        #endregion
    }
}

