namespace Fanex.NAP.SPU.CTS.Features.Notification.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class DBAMonitoringCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_Status")]
        public string Status { get; set; }

        public override string GetSettingKey() => "CTS_DC_DataConsistency_CustomerCategory_Get";

        public override bool IsValid() => true;
    }
}
