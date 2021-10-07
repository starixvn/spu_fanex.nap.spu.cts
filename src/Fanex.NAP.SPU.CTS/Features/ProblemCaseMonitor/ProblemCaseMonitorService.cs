namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor
{
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Model;
    using Newtonsoft.Json;        
    using System.Linq;        

    public interface IProblemCaseMonitorService
    {
        ProblemCaseMonitorClassificationModel GetProblemCaseMonitorClassification();

        int InsertProblemCaseMonitorClassification(ProblemCaseMonitorClassificationModel problemCaseModel);

        ProblemCaseMonitorAssociationModel GetProblemCaseMonitorAssociation();

        int InsertProblemCaseMonitorAssociation(ProblemCaseMonitorAssociationModel problemCaseModel);
    }

    public class ProblemCaseMonitorService : IProblemCaseMonitorService
    {
        private readonly IProblemCaseMonitorDataAccess dataAccess;

        public ProblemCaseMonitorService(IProblemCaseMonitorDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public ProblemCaseMonitorClassificationModel GetProblemCaseMonitorClassification()
        {
            return this.dataAccess.GetProblemCaseMonitorClassification();
        }

        public int InsertProblemCaseMonitorClassification(ProblemCaseMonitorClassificationModel problemCaseModel)
        {
            if (problemCaseModel != null && problemCaseModel.ProblemCaseMonitorClassificationList.Any())
            {                
                var problemCaseJson = JsonConvert.SerializeObject(problemCaseModel);

                return this.dataAccess.InsertProblemCaseMonitorClassification(problemCaseModel.ClassificationHistoryID, problemCaseJson);
            }

            return 1;
        }

        public ProblemCaseMonitorAssociationModel GetProblemCaseMonitorAssociation()
        {
            return this.dataAccess.GetProblemCaseMonitorAssociation();
        }

        public int InsertProblemCaseMonitorAssociation(ProblemCaseMonitorAssociationModel problemCaseModel)
        {
            if (problemCaseModel != null && problemCaseModel.ProblemCaseMonitorAssociationList.Any())
            {
                var problemCaseJson = JsonConvert.SerializeObject(problemCaseModel);

                return this.dataAccess.InsertProblemCaseMonitorAssociation(problemCaseModel.AssDeviceID, problemCaseModel.AssIPID, problemCaseJson);
            }

            return 1;
        }
    }
}
