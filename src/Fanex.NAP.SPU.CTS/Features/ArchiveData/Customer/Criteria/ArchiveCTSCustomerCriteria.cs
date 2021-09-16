namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ArchiveCTSCustomerCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_CustomerList")]
        public string CustomerJsonData { get; set; }

        [SpParam(SqlParamName = "ip_CustType")]
        public int CustType { get; set; }

        public override string GetSettingKey() => "CTS_DC_ArchiveCustomer";

        public override bool IsValid() => !string.IsNullOrEmpty(CustomerJsonData);
    }
}
