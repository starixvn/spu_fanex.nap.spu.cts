using System;

namespace Fanex.NAP.SPU.CTS.Features.Notification.Entities
{
    public class DBAMonitoringEntity
    {
        public string IssueType { get; set; }

        public DateTime? IssueDateTime { get; set; }

        public string IssueBrief { get; set; }

        public string IssueDetailedInfo { get; set; }
    }
}