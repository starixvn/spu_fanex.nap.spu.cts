namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer.Criteria;

    public interface IArchiveAssociationCustomerDataAccess
    {
        void ArchiveAssociationCustomer();
    }

    public class ArchiveAssociationCustomerDataAccess : IArchiveAssociationCustomerDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveAssociationCustomerDataAccess()
        {
            repository = new DynamicRepository();
        }

        public void ArchiveAssociationCustomer()
        {
            var criteria = new ArchiveAssociationCustomerCriteria();

            repository.Execute(criteria);
        }
    }
}
