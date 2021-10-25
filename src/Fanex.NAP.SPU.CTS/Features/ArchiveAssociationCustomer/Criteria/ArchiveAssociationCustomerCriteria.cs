namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ArchiveAssociationCustomerCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "op_IsDataExist")]
        public int IsDataExist { get; set; }

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
