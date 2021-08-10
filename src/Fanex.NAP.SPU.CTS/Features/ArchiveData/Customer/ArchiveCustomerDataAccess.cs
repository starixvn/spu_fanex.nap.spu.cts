namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Criteria;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Entities;
    using System;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface IArchiveCustomerDataAccess
    {
        IEnumerable<MainDBArchiveCustomer> GetArchivedCustomerFromMainDb(int lastCustId, DateTime lastArchivedDate, int batchSize, SubscriberType custType);

        void ArchiveCTSCustomer(string customerJsonData, SubscriberType custType);
    }

    public class ArchiveCustomerDataAccess : IArchiveCustomerDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveCustomerDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<MainDBArchiveCustomer> GetArchivedCustomerFromMainDb(int lastCustId, DateTime lastArchivedDate, int batchSize, SubscriberType custType) 
        {
            var criteria = new GetArchivedCustomerFromMainDBCriteria
            {
                LastArchiveDate = lastArchivedDate,
                LastCustId  = lastCustId,
                RowSize = batchSize,
                CustType = (int)custType
            };

            return repository.Fetch<MainDBArchiveCustomer>(criteria);
        }

        public void ArchiveCTSCustomer(string customerJsonData, SubscriberType custType)
        {
            var criteria = new ArchiveCTSCustomerCriteria
            {
                CustomerJsonData = customerJsonData,
                CustType = (int)custType
            };

            repository.Execute(criteria);
        }
    }
}
