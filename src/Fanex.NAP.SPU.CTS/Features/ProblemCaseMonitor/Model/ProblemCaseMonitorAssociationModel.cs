namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Model
{
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Entities;    
    using System.Collections.Generic;    

    public class ProblemCaseMonitorAssociationModel
    {
        public ProblemCaseMonitorAssociationModel()
        {
            ProblemCaseMonitorAssociationList = new List<ProblemCaseMonitorAssociationEntity>();
        }

        public ulong AssDeviceID { get; set; }

        public ulong AssIPID { get; set; }

        public IEnumerable<ProblemCaseMonitorAssociationEntity> ProblemCaseMonitorAssociationList { get; set; }
    }
}
