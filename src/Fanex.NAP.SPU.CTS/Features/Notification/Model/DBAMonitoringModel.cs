namespace Fanex.NAP.SPU.CTS.Features.Notification.Model
{
    using Fanex.NAP.SPU.CTS.Features.Notification.Entities;
    using System.Collections.Generic;

    public class DBAMonitoringModel
    {
        public IEnumerable<DBAMonitoringEntity> Data { get; set; }

        public string Status { get; set; }
    }
}
