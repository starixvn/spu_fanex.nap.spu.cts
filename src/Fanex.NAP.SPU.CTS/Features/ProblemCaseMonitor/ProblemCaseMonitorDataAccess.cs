namespace Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Criteria;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor.Model;
    using System.Linq;

    public interface IProblemCaseMonitorDataAccess
    {
        ProblemCaseMonitorClassificationModel GetProblemCaseMonitorClassification();

        int InsertProblemCaseMonitorClassification(ulong classificationHistoryID, string problemCaseJson);

        ProblemCaseMonitorAssociationModel GetProblemCaseMonitorAssociation();

        int InsertProblemCaseMonitorAssociation(ulong assDeviceID, ulong assIPID, string problemCaseJson);
    }

    public class ProblemCaseMonitorDataAccess : IProblemCaseMonitorDataAccess
    {
        private readonly IDynamicRepository repository;

        public ProblemCaseMonitorDataAccess()
        {
            repository = new DynamicRepository();
        }

        public ProblemCaseMonitorClassificationModel GetProblemCaseMonitorClassification()
        {
            var model = new ProblemCaseMonitorClassificationModel();
            var criteria = new ProblemCaseMonitorClassificationGetCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                model.ProblemCaseMonitorClassificationList = db.Query<ProblemCaseMonitorClassificationEntity>(criteria);
                model.ClassificationHistoryID = criteria.ClassificationHistoryID;

                return model;
            }
        }

        public int InsertProblemCaseMonitorClassification(ulong classificationHistoryID, string problemCaseJson)
        {
            var criteria = new ProblemCaseMonitorClassificationInsertCriteria
            {
                ClassificationHistoryID = classificationHistoryID,
                ProblemCaseJson = problemCaseJson
            };

            return repository.Execute(criteria);
        }

        public ProblemCaseMonitorAssociationModel GetProblemCaseMonitorAssociation()
        {
            var model = new ProblemCaseMonitorAssociationModel();
            var criteria = new ProblemCaseMonitorAssociationGetCriteria {  AssDeviceID = 0, AssIPID = 0};

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                model.ProblemCaseMonitorAssociationList = db.Query<ProblemCaseMonitorAssociationEntity>(criteria);
                model.AssDeviceID = criteria.AssDeviceID;
                model.AssIPID = criteria.AssIPID;

                return model;
            }
        }

        public int InsertProblemCaseMonitorAssociation(ulong assDeviceID, ulong assIPID, string problemCaseJson)
        {
            var criteria = new ProblemCaseMonitorAssociationInsertCriteria
            {
                AssDeviceID = assDeviceID,
                AssIPID = assIPID,
                ProblemCaseJson = problemCaseJson
            };

            return repository.Execute(criteria);
        }

    }
}
