namespace Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICustClassToMainDBService
    {
        IEnumerable<LatestClassificationEntity> GetLatestClassification(string custIds);

        DBResult PushCustomerClassificationToMainDB(IEnumerable<LatestClassificationEntity> customerClassifications, int retryTime);

        DBResult UpdateAppliedCustomerClassification(IEnumerable<LatestClassificationEntity> customerClassifications);
    }

    public class CustClassToMainDBService : ICustClassToMainDBService
    {
        private readonly ICustClassToMainDBDataAccess custClassToMainDBDataAccess;

        public CustClassToMainDBService(ICustClassToMainDBDataAccess custClassToMainDBDataAccess)
        {
            this.custClassToMainDBDataAccess = custClassToMainDBDataAccess;
        }

        public IEnumerable<LatestClassificationEntity> GetLatestClassification(string custIds)
        {
            if(!string.IsNullOrEmpty(custIds))
            {
                return custClassToMainDBDataAccess.GetLatestClassification(custIds);
            }

            return new List<LatestClassificationEntity>();
        }

        public DBResult PushCustomerClassificationToMainDB(IEnumerable<LatestClassificationEntity> customerClassifications, int retryTime)
        {
            var rs = new DBResult
            {
                ErrorCode = 0,
                ErrorMsg = string.Empty
            };

            if (customerClassifications == null || !customerClassifications.Any())
            {
                return rs;
            }

            while (retryTime >= 0)
            {
                try
                {
                    var result = custClassToMainDBDataAccess.UpdateCustomerGeneralClassificationtoMainDB(customerClassifications);

                    if (result.ErrorCode != 0 && retryTime == 0)
                    {
                        rs.ErrorCode = -1;
                        rs.ErrorMsg = $"PushCustomerClassificationToMain fail with listCust: {JsonConvert.SerializeObject(customerClassifications)}";
                        return rs;
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
                        rs.ErrorCode = -1;
                        rs.ErrorMsg = new LogMessage(ex.Message)
                                        .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                                        .AddMessage($"Data input: {JsonConvert.SerializeObject(customerClassifications)} - { DateTime.Now}").ToString();
                        return rs;
                    }

                    retryTime--;
                }
            }

            return rs;
        }

        public DBResult UpdateAppliedCustomerClassification(IEnumerable<LatestClassificationEntity> customerClassifications)
        {
            if (customerClassifications != null && customerClassifications.Any())
            {
                var jsonData = JsonConvert.SerializeObject(customerClassifications);

                return custClassToMainDBDataAccess.UpdateAppliedCustomerClassification(jsonData);
            }

            return new DBResult
            {
                ErrorCode = 0,
                ErrorMsg = string.Empty
            };
        }
    }
}
