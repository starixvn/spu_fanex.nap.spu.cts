namespace Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class RollbackDeviceAssociationCustomerCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "op_ErrorMessage")]
        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_Arc_CustomerAssociation_Device_Rollback";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
