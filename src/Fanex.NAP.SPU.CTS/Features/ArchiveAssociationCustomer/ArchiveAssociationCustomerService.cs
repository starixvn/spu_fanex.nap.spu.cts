namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IArchiveAssociationCustomerService
    {
        bool ArchiveAssociationCustomer();
    }

    public class ArchiveAssociationCustomerService : IArchiveAssociationCustomerService
    {
        private readonly IArchiveAssociationCustomerDataAccess dataAccess;

        public ArchiveAssociationCustomerService(IArchiveAssociationCustomerDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public bool ArchiveAssociationCustomer()
        {
            return dataAccess.ArchiveAssociationCustomer();
        }
    }
}
