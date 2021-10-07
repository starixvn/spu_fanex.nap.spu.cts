namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor
{
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Model;
    using Newtonsoft.Json;        
    using System.Linq;        

    public interface IProblemCaseClassificationService
    {
        ProblemCaseMonitorClassificationModel GetProblemCaseMonitorClassification();

        int InsertProblemCaseMonitorClassification(ProblemCaseMonitorClassificationModel problemCaseModel);
    }

    public class ProblemCaseClassificationService : IProblemCaseClassificationService
    {
        private readonly IProblemCaseClassificationDataAccess dataAccess;

        public ProblemCaseClassificationService(IProblemCaseClassificationDataAccess dataAccess)
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

            return 0;
        }
    }
}
