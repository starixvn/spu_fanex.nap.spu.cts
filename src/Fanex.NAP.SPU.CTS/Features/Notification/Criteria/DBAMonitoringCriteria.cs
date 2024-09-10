namespace Fanex.NAP.SPU.CTS.Features.Notification.Criteria
{
    using Fanex.Data.Repository;

    public class DBAMonitoringCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CTS_DC_DBAAdmin_Monitoring";

        public override bool IsValid() => true;
    }
}
