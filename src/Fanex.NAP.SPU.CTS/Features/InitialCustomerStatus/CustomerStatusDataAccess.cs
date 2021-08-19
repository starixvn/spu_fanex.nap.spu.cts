namespace Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus.Criteria;
    using Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus.Entities;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface ICustomerStatusDataAccess
    {
        IEnumerable<long> GetCustIds(int batchSize);

        IEnumerable<CustomerMainDBModel> GetCustomerFromMainDBForUpdate(string custIds, UpdateCustomer sourceType);

        void UpdateCustomerStatus(string custInfoJson, long lastCustId);
    }

    public class CustomerStatusDataAccess : ICustomerStatusDataAccess
    {
        private readonly IDynamicRepository repository;

        public CustomerStatusDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<long> GetCustIds(int batchSize)
        {
            var criteria = new CustomerIDGetCriteria
            {
                BatchSize = batchSize
            };

            return repository.Fetch<long>(criteria);
        }

        public IEnumerable<CustomerMainDBModel> GetCustomerFromMainDBForUpdate(string custIds, UpdateCustomer sourceType)
        {
            var criteria = new GetCustomerInfoFromMainDBCriteria
            {
                CustIds = custIds,
                SourceType = (int)sourceType
            };

            return repository.Fetch<CustomerMainDBModel>(criteria);
        }

        public void UpdateCustomerStatus(string custInfoJson, long lastCustId)
        {
            var criteria = new CustomerStatusUpdateCriteria
            {
                CustInfoJson = custInfoJson,
                LastCustId = lastCustId
            };

            repository.Execute(criteria);
        }
    }
}