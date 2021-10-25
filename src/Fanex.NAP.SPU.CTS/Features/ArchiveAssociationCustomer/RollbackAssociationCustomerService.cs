namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer
{
    public interface IRollbackAssociationCustomerService
    {
        int RollbackAssociationCustomer();
    }

    public class RollbackAssociationCustomerService : IRollbackAssociationCustomerService
    {
        private readonly IRollbackAssociationCustomerDataAccess dataAccess;

        public RollbackAssociationCustomerService(IRollbackAssociationCustomerDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public int RollbackAssociationCustomer()
        {
            return dataAccess.RollbackAssociationCustomer();
        }
        
    }

}
