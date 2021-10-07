namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Model
{
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Entities;  
    using System.Collections.Generic;

    public class ProblemCaseMonitorClassificationModel
    {
        public ProblemCaseMonitorClassificationModel()
        {
            ProblemCaseMonitorClassificationList = new List<ProblemCaseMonitorClassificationEntity>();
        }

        public long ClassificationHistoryID { get; set; }

        public IEnumerable<ProblemCaseMonitorClassificationEntity> ProblemCaseMonitorClassificationList { get; set; }
    }
}
