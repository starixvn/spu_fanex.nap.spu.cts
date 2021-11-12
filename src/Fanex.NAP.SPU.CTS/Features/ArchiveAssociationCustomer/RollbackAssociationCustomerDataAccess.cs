namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer.Criteria;
    using Fanex.NAP.SPU.CTS.Shares.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRollbackAssociationCustomerDataAccess
    {
        int RollbackAssociationCustomer();        
    }

    public class RollbackAssociationCustomerDataAccess : IRollbackAssociationCustomerDataAccess
    {
        private readonly IDynamicRepository repository;

        public RollbackAssociationCustomerDataAccess()
        {
            repository = new DynamicRepository();
        }

        public int RollbackAssociationCustomer()
        {
            var criteria = new RollbackAssociationCustomerCriteria();

            return repository.Execute(criteria);
        }
    }
}
