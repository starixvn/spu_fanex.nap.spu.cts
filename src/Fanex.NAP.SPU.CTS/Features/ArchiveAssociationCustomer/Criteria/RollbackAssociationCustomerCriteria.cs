namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer.Criteria
{
    using Fanex.Data.Repository;

    public class RollbackAssociationCustomerCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "CTS_Arc_CustomerAssociation_Rollback";
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}
