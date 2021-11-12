namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer.Criteria
{
    using Fanex.Data.Repository;

    public class ArchiveAssociationCustomerCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "CTS_Arc_CustomerAssociation_Archive";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
