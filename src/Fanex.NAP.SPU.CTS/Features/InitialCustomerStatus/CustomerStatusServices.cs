namespace Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus
{
    using Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus.Entities;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface ICustomerStatusServices
    {
        ICustomerStatusServices Start();

        ICustomerStatusServices GetCustIds(int batchSize);

        ICustomerStatusServices GetCustomerFromMainDBForUpdate(UpdateCustomer sourceType);

        ICustomerStatusServices UpdateCustomerStatus();
    }

    public class CustomerStatusServices : ICustomerStatusServices
    {
        private readonly ICustomerStatusDataAccess customerStatusDataAccess;
        private IEnumerable<CustomerMainDBModel> custStatusInfo;
        private long? lastCustId;
        private string custIdsToString;

        public CustomerStatusServices(ICustomerStatusDataAccess customerStatusDataAccess)
        {
            this.customerStatusDataAccess = customerStatusDataAccess;
        }

        public ICustomerStatusServices Start()
        {
            custStatusInfo = new List<CustomerMainDBModel>();
            lastCustId = null;
            custIdsToString = string.Empty;

            return this;
        }

        public ICustomerStatusServices GetCustIds(int batchSize)
        {
            var custIds = customerStatusDataAccess.GetCustIds(batchSize);

            if (custIds != null && custIds.Any())
            {
                lastCustId = custIds.Min();
                custIdsToString = string.Join(",", custIds);
            }

            return this;
        }

        public ICustomerStatusServices GetCustomerFromMainDBForUpdate(UpdateCustomer sourceType)
        {
            if (!string.IsNullOrEmpty(custIdsToString))
            {
                custStatusInfo = customerStatusDataAccess.GetCustomerFromMainDBForUpdate(custIdsToString, sourceType);
            }

            return this;
        }

        public ICustomerStatusServices UpdateCustomerStatus()
        {
            var custInfoJson = JsonConvert.SerializeObject(string.Empty);

            if (custStatusInfo != null && custStatusInfo.Any())
            {
                var custStatus = custStatusInfo.Where(c => c.CustStatusID != 0);

                if (custStatus != null && custStatus.Any())
                {
                    custInfoJson = JsonConvert.SerializeObject(custStatus.Select(c => new
                    {
                        CustID = c.CustID,
                        CustStatusID = c.CustStatusID
                    }));
                }
            }

            if (lastCustId != null)
            {
                customerStatusDataAccess.UpdateCustomerStatus(custInfoJson, lastCustId);
            }

            return this;
        }
    }
}