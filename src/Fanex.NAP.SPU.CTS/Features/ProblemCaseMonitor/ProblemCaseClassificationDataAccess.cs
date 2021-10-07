namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Criteria;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Model;
    using System.Linq;

    public interface IProblemCaseClassificationDataAccess
    {
        ProblemCaseMonitorClassificationModel GetProblemCaseMonitorClassification();

        int InsertProblemCaseMonitorClassification(long classificationHistoryID, string problemCaseJson);
    }

    public class ProblemCaseClassificationDataAccess : IProblemCaseClassificationDataAccess
    {
        private readonly IDynamicRepository repository;

        public ProblemCaseClassificationDataAccess()
        {
            repository = new DynamicRepository();
        }

        public ProblemCaseMonitorClassificationModel GetProblemCaseMonitorClassification()
        {
            var model = new ProblemCaseMonitorClassificationModel();
            var criteria = new ProblemCaseMonitorClassificationGetCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var result = db.QueryMultiple(criteria))
                {
                    var classificationHistoryID = result.Read<long>();

                    model.ProblemCaseMonitorClassificationList = result.Read<ProblemCaseMonitorClassificationEntity>();

                    if (classificationHistoryID != null && classificationHistoryID.Any())
                    {
                        model.ClassificationHistoryID = classificationHistoryID.ToList()[0];
                    }

                    return model;
                }
            }
        }

        public int InsertProblemCaseMonitorClassification(long classificationHistoryID, string problemCaseJson)
        {
            var criteria = new ProblemCaseMonitorClassificationInsertCriteria
            {
                ClassificationHistoryID = classificationHistoryID,
                ProblemCaseJson = problemCaseJson
            };

            return repository.Execute(criteria);
        }
    }
}
