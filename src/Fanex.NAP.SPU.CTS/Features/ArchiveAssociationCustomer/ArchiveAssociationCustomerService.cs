namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer
{
    public interface IArchiveAssociationCustomerService
    {
        void ArchiveAssociationCustomer();
    }

    public class ArchiveAssociationCustomerService : IArchiveAssociationCustomerService
    {
        private readonly IArchiveAssociationCustomerDataAccess dataAccess;

        public ArchiveAssociationCustomerService(IArchiveAssociationCustomerDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void ArchiveAssociationCustomer()
        {
            dataAccess.ArchiveAssociationCustomer();
        }
    }
}
