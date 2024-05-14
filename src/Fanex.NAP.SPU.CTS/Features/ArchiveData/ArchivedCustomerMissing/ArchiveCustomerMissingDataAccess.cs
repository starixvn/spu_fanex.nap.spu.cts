namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Criteria;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Entities;
    using System;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IArchiveCustomerMissingDataAccess
    {
        IEnumerable<MainDBArchiveCustomer> GetArchivedCustomerMissing(int lastCustId, DateTime lastArchivedDate, DateTime maxArchivedDate, int batchSize, SubscriberType custType);

        void ArchiveCustomerMissing(string customerJsonData, SubscriberType custType);
    }

    public class ArchiveCustomerMissingDataAccess : IArchiveCustomerMissingDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveCustomerMissingDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<MainDBArchiveCustomer> GetArchivedCustomerMissing(int lastCustId, DateTime lastArchivedDate, DateTime maxArchivedDate, int batchSize, SubscriberType custType)
        {
            var criteria = new ArchivedCustomerMissingGetCriteria
            {
                LastArchiveDate = lastArchivedDate,
                MaxArchiveDate = maxArchivedDate,
                LastCustId = lastCustId,
                RowSize = batchSize,
                CustType = (int)custType
            };

            return repository.Fetch<MainDBArchiveCustomer>(criteria);
        }

        public void ArchiveCustomerMissing(string customerJsonData, SubscriberType custType)
        {
            var criteria = new ArchiveCustomerCorrectDataCriteria
            {
                CustomerJsonData = customerJsonData,
                CustType = (int)custType
            };

            repository.Execute(criteria);
        }
    }
}