namespace Fanex.NAP.SPU.CTS.Features.UserPermission
{
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.Transform.CTSCustomer;
    using Fanex.NAP.SPU.CTS.Features.Transform.CTSCustomer.Entities;
    using Fanex.NAP.SPU.CTS.Features.UserPermission.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICTSCustomerService
    {
    }

    public class CTSCustomerService : ICTSCustomerService
    {
        private readonly ICTSCustomerDataAccess dataAccess;
        private readonly ISentryClient sentryClient;

        public CTSCustomerService(ICTSCustomerDataAccess dataAccess, ISentryClient sentryClient)
        {
            this.dataAccess = dataAccess;
            this.sentryClient = sentryClient;
        }

        public LastScannedCustModel GetLastInsertCust()
        {
            return dataAccess.GetLastInsertCust();
        }

        public UpdateCustomerModel GetCustomerFromMainDB(int numOfAccount, Int64 lastScannedCustID)
        {
            return dataAccess.GetCustomerFromMainDB(numOfAccount, lastScannedCustID);
        }

        public List<CustomerMainDBModel> GetCustSubFromMainDB(int numOfAccount, Int64 lastScannedCustID)
        {
            return dataAccess.GetCustSubFromMainDB(numOfAccount, lastScannedCustID);
        }

        public IEnumerable<InsertCustomerModel> TransformCustomerFromMainDB(List<CustomerMainDBModel> customerList)
        {
            var custInfos = new List<InsertCustomerModel>();
            var customerJson = JsonConvert.SerializeObject(customerList);

            try
            {              
                custInfos = dataAccess.TransformCustomerFromMainDB(customerJson)?.ToList();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError("InsertNewCustomer", "TransformCustomerFromMainDB exception:", customerJson, ex, sentryClient);                
            }

            return custInfos;
        }
    }
}
