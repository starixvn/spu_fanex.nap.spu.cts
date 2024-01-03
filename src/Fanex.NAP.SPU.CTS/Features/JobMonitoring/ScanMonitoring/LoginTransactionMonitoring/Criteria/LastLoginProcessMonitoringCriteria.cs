namespace Fanex.NAP.SPU.CTS.Features.MonitorJobs.CustomerJobMonitor
{
    using Fanex.Data.Repository;

    public class LastLoginProcessMonitoringCriteria : NonQueryCommand
    {
        public long ProcessNumber { get; set; }

        public override string GetSettingKey()
        {
            return "DCS_DC_Monitoring_LastLoginProcess_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}