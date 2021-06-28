namespace Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Shares;
    using System.Collections.Generic;

    public interface ICustClassToMainDBDataAccess
    {
        IEnumerable<LatestClassificationEntity> GetLatestClassification(string custIds);

        DBResult UpdateCustomerGeneralClassificationtoMainDB(IEnumerable<LatestClassificationEntity> custClassification);

        DBResult UpdateAppliedCustomerClassification(string problemCustomerJson);
    }

    public class CustClassToMainDBDataAccess : ICustClassToMainDBDataAccess
    {
        private readonly IDynamicRepository repository;

        public CustClassToMainDBDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<LatestClassificationEntity> GetLatestClassification(string custIds)
        {
            var criteria = new LastestClassificationGetCriteria
            {
                CustIds = custIds
            };

            return repository.Fetch<LatestClassificationEntity>(criteria);
        }

        public DBResult UpdateCustomerGeneralClassificationtoMainDB(IEnumerable<LatestClassificationEntity> custClassification)
        {
            var criteria = new CustomerClassificationUpdateCriteria
            {
                custList = custClassification.ToXmlString()
            };

            repository.Execute(criteria);

            return new DBResult
            { 
                ErrorCode = criteria.ErrorCode, 
                ErrorMsg = criteria.ErrorMessage 
            };
        }

        public DBResult UpdateAppliedCustomerClassification(string problemCustomerJson)
        {
            var criteria = new AppliedCustomerClassificationUpdateCriteria
            {
                ProblemCustomerJson = problemCustomerJson
            };

            repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = !string.IsNullOrEmpty(criteria.ErrorMessage) ? -1 : 0,
                ErrorMsg = criteria.ErrorMessage
            };
        }
    }
}
