namespace Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CustomerIDGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_Size")]
        public int BatchSize { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustInfo_Healing_GetCustIDs";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}