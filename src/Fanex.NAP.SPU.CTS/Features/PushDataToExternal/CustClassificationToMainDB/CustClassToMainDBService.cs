namespace Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICustClassToMainDBService
    {
        IEnumerable<LatestClassificationEntity> GetLatestClassification(string custIds, string serviceName, bool isWriteLog);

        bool RetryPushCCToMainDB(IEnumerable<LatestClassificationEntity> customerClassifications, int retryTime, string serviceName);

        void UpdateAppliedCustClassification(IEnumerable<LatestClassificationEntity> customerClassifications, string serviceName);

        bool PushCustClassificationToMainDB(IEnumerable<LatestClassificationEntity> lastestClassification, string serviceName, bool isWriteLog);
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

        public bool PushCustClassificationToMainDB(IEnumerable<LatestClassificationEntity> lastestClassification, string serviceName, bool isWriteLog)
        {
            bool isSuccess = false;

            if (lastestClassification != null && !lastestClassification.Any())
            {
                isSuccess = RetryPushCCToMainDB(lastestClassification, Constants.RetryPushCCToExternal, serviceName);

                if (isSuccess)
                {
                    UpdateAppliedCustClassification(lastestClassification, serviceName);
                }
            }

            Utilities.LogSentryInfo(serviceName, "PushCCToMainDB done", (isSuccess, lastestClassification), isWriteLog, sentryClient);
            return isSuccess;
        }

        public IEnumerable<LatestClassificationEntity> GetLatestClassification(string custIds, string serviceName, bool isWriteLog)
        {
            var lastestClassification = new List<LatestClassificationEntity>();

            if (!string.IsNullOrEmpty(custIds))
            {
                lastestClassification = custClassToMainDBDataAccess.GetLatestClassification(custIds)?.ToList();
            }

            Utilities.LogSentryInfo(serviceName, "Get lastestCC", lastestClassification, isWriteLog, sentryClient);
            return lastestClassification;
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

        public void UpdateAppliedCustClassification(IEnumerable<LatestClassificationEntity> customerClassifications, string serviceName)
        {
            if (customerClassifications != null && customerClassifications.Any())
            {
                var jsonData = JsonConvert.SerializeObject(customerClassifications);

                var result = custClassToMainDBDataAccess.UpdateAppliedCustomerClassification(jsonData);

                if (result.ErrorCode == -1)
                {
                    Utilities.LogSentryError(serviceName, "Update Apllied CC throw exception", customerClassifications, new Exception(result.ErrorMsg), sentryClient);
                }
            }
        }
    }
}
