namespace Fanex.NAP.SPU.CTS.Features.PushDataToExternal.FeedCCToSoon88
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class AssociationWithCategoryGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CustIDList")]
        public string CustIds { get; set; }

        [SpParam(SqlParamName = "ip_IsDelete")]
        public bool IsDelete { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_Association_GetWithCategory";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CustIds);
        }
    }
}