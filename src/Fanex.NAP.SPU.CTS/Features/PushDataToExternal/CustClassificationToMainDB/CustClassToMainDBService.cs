namespace Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface ICustClassToMainDBService
    {
        IEnumerable<LatestClassificationEntity> GetLatestClassification(IEnumerable<int> custIds, string serviceName, bool isWriteLog);

        bool PushCustClassificationToMainDB(IEnumerable<LatestClassificationEntity> lastestClassifications, string serviceName, bool isWriteLog);

        bool RetryPushCCToMainDB(IEnumerable<LatestClassificationEntity> customerClassifications, int retryTime, string serviceName);

        void UpdateAppliedCustClassification(IEnumerable<LatestClassificationEntity> lastestClassifications, string serviceName);

        void UpdateDangerLevelToMainDB(IEnumerable<LatestClassificationEntity> lastestClassifications, string serviceName, bool isWriteLog);

        bool RetryUpdateDangerLevelToMainDB(IEnumerable<DangerLevelEntity> dangerLevels, DangerLevel typeId, int retryTime, string serviceName);
    }

    public class CustClassToMainDBService : ICustClassToMainDBService
    {
        private readonly ICustClassToMainDBDataAccess custClassToMainDBDataAccess;
        private readonly ISentryClient sentryClient;

        public CustClassToMainDBService(ICustClassToMainDBDataAccess custClassToMainDBDataAccess, ISentryClient sentryClient)
        {
            this.custClassToMainDBDataAccess = custClassToMainDBDataAccess;
            this.sentryClient = sentryClient;
        }

        public bool PushCustClassificationToMainDB(IEnumerable<LatestClassificationEntity> lastestClassifications, string serviceName, bool isWriteLog)
        {
            bool isSuccess = false;

            if (lastestClassifications != null && !lastestClassifications.Any())
            {
                isSuccess = RetryPushCCToMainDB(lastestClassifications, Constants.RetryPushCCToExternal, serviceName);

                if (isSuccess)
                {
                    UpdateAppliedCustClassification(lastestClassifications, serviceName);
                }
            }

            Utilities.LogSentryInfo(serviceName, "PushCCToMainDB done", (isSuccess, lastestClassifications), isWriteLog, sentryClient);
            return isSuccess;
        }

        public IEnumerable<LatestClassificationEntity> GetLatestClassification(IEnumerable<int> custIds, string serviceName, bool isWriteLog)
        {
            var lastestClassification = new List<LatestClassificationEntity>();

            if (custIds != null && custIds.Any())
            {
                string custIdsToString = string.Join(",", custIds);

                lastestClassification = custClassToMainDBDataAccess.GetLatestClassification(custIdsToString)?.ToList();
            }

            Utilities.LogSentryInfo(serviceName, "Get lastestClassifications", lastestClassification, isWriteLog, sentryClient);
            return lastestClassification;
        }

        public void UpdateAppliedCustClassification(IEnumerable<LatestClassificationEntity> lastestClassifications, string serviceName)
        {
            if (lastestClassifications != null && lastestClassifications.Any())
            {
                var jsonData = JsonConvert.SerializeObject(lastestClassifications);

                var result = custClassToMainDBDataAccess.UpdateAppliedCustomerClassification(jsonData);

                if (result.ErrorCode == -1)
                {
                    Utilities.LogSentryError(serviceName, "Update Apllied CC throw exception", lastestClassifications, new Exception(result.ErrorMsg), sentryClient);
                }
            }
        }

        public void UpdateDangerLevelToMainDB(IEnumerable<LatestClassificationEntity> lastestClassifications, string serviceName, bool isWriteLog)
        {
            if (lastestClassifications == null || !lastestClassifications.Any())
            {
                return;
            }

            var dangerLevels = custClassToMainDBDataAccess.GetDangerLevel(string.Join(",", lastestClassifications.Select(c => c.CustId)), DangerLevel.Original);
            Utilities.LogSentryInfo(serviceName, "Get GetDangerLevel", dangerLevels, isWriteLog, sentryClient);

            if (dangerLevels != null && dangerLevels.Any())
            {
                var isUpdateSuccessful = RetryUpdateDangerLevelToMainDB(dangerLevels, DangerLevel.Original, Constants.RetryPushCCToExternal, serviceName);

                if (isUpdateSuccessful)
                {
                    var jsonUpdateApplyCCs = JsonConvert.SerializeObject(dangerLevels);

                    custClassToMainDBDataAccess.UpdateAppliedDangerLevel(jsonUpdateApplyCCs, DangerLevel.Original);
                }

                Utilities.LogSentryInfo(serviceName, "UpdateDangerLevelToMainDB done", (isUpdateSuccessful, lastestClassifications), isWriteLog, sentryClient);
            }
        }

        public bool RetryPushCCToMainDB(IEnumerable<LatestClassificationEntity> customerClassifications, int retryTime, string serviceName)
        {
            if (customerClassifications == null || !customerClassifications.Any())
            {
                return true;
            }

            while (retryTime >= 0)
            {
                try
                {
                    var result = custClassToMainDBDataAccess.UpdateCustomerGeneralClassificationtoMainDB(customerClassifications);

                    if (result.ErrorCode != 0 && retryTime == 0)
                    {
                        Utilities.LogSentryError(serviceName, "PushCustomerClassificationToMain fail", customerClassifications, new Exception(result.ErrorMsg), sentryClient);
                        return false;
                    }
                    else if (result.ErrorCode != 0)
                    {
                        retryTime--;
                    }
                    else
                    {
                        retryTime = -1;
                    }
                }
                catch (Exception ex)
                {
                    if (retryTime == 0)
                    {
                        Utilities.LogSentryError(serviceName, "PushCustomerClassificationToMain throw exception", customerClassifications, ex, sentryClient);
                        return false;
                    }

                    retryTime--;
                }
            }

            return true;
        }

        public bool RetryUpdateDangerLevelToMainDB(IEnumerable<DangerLevelEntity> dangerLevels, DangerLevel typeId, int retryTime, string serviceName)
        {
            if (dangerLevels == null || !dangerLevels.Any())
            {
                return true;
            }

            while (retryTime >= 0)
            {
                try
                {
                    var result = custClassToMainDBDataAccess.UpdateDangerLevelToMainDB(dangerLevels, typeId);

                    if (result.ErrorCode != 0 && retryTime == 0)
                    {
                        Utilities.LogSentryError(serviceName, "UpdateDangerLevelToMain fail", (dangerLevels, typeId), new Exception(result.ErrorMsg), sentryClient);
                        return false;
                    }
                    else if (result.ErrorCode != 0)
                    {
                        retryTime--;
                    }
                    else
                    {
                        retryTime = -1;
                    }
                }
                catch (Exception ex)
                {
                    if (retryTime == 0)
                    {
                        Utilities.LogSentryError(serviceName, "UpdateDangerLevelToMain thrown exception", (dangerLevels, typeId), ex, sentryClient);
                        return false;
                    }

                    retryTime--;
                }
            }

            return true;
        }
    }
}