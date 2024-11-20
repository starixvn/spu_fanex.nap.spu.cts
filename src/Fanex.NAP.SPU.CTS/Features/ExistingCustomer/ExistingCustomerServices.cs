namespace Fanex.NAP.SPU.CTS.Features.ExistingCustomer
{
    using Fanex.NAP.SPU.CTS.Features.ExistingCustomer.Entities;
    using System;
    using System.Collections.Generic;

    public interface IExistingCustomerServices
    {
        IEnumerable<CustomerEntity> GetPlaceBetCustomer(DateTime fromDate, DateTime toDate);

        IEnumerable<CTSCustomerEntity> CheckCustomerNotExisted(string custIds, DateTime fromDate, DateTime toDate);
    }
    public class ExistingCustomerServices : IExistingCustomerServices
    {
        private readonly IExistingCustomerDataAccess existingCustomerDataAccess;

        public ExistingCustomerServices(IExistingCustomerDataAccess existingCustomerDataAccess)
        {
            this.existingCustomerDataAccess = existingCustomerDataAccess;
        }

        public IEnumerable<CTSCustomerEntity> CheckCustomerNotExisted(string custIds, DateTime fromDate, DateTime toDate)
        {
            return this.existingCustomerDataAccess.CheckCustomerNotExisted(custIds, fromDate, toDate);
        }

        public IEnumerable<CustomerEntity> GetPlaceBetCustomer(DateTime fromDate, DateTime toDate)
        {
            return existingCustomerDataAccess.GetPlaceBetCustomers(fromDate, toDate);
        }
    }
}
