namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.RealtimeClassification
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CustClassificationByCategoryForRealtimeScanningGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CategoryIDList")]
        public string CategoryIdList { get; set; }

        [SpParam(SqlParamName = "ip_CustSport")]
        public string CustSportsJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_GetCustomerClassificationByCategory";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CategoryIdList);
        }
    }
}