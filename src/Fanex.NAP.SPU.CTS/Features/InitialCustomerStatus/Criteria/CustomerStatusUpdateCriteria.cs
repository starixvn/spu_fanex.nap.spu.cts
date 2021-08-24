namespace Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CustomerStatusUpdateCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_CustInfo")]
        public string CustInfoJson { get; set; }

        [SpParam(SqlParamName = "ip_LastCustID")]
        public long? LastCustId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustInfo_Healing_Update";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}