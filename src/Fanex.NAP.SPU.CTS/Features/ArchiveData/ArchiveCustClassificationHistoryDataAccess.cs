namespace Fanex.NAP.SPU.CTS.Features.ArchiveData
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Criteria;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Entities;
    using System.Collections.Generic;

    public interface IArchiveCustClassificationHistoryDataAccess
    {
        bool CleanUpUnChangedCustClassificationHistory();

        HistoryClassificationArchiveEntity CleanUpChangedCustClassificationHistory();

        void ArchiveCustClassificationHistory(string historyDataJson);
    }

    public class ArchiveCustClassificationHistoryDataAccess : IArchiveCustClassificationHistoryDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveCustClassificationHistoryDataAccess()
        {
            repository = new DynamicRepository();
        }

        public bool CleanUpUnChangedCustClassificationHistory()
        {
            var criteria = new UnChangedCustClassificationHistoryCleanUpCriteria();

            repository.Execute(criteria);

            return criteria.IsDataExist;
        }

        public HistoryClassificationArchiveEntity CleanUpChangedCustClassificationHistory()
        {
            var output = new HistoryClassificationArchiveEntity
            {
                CustClassificationHistory = new List<CustClassificationHistoryEntity>(),
                IsExistData = false
            };

            var criteria = new ChangedCustClassificationHistoryCleanUpCriteria();

            output.CustClassificationHistory = repository.Fetch<CustClassificationHistoryEntity>(criteria);
            output.IsExistData = criteria.IsDataExist;

            return output;
        }

        public void ArchiveCustClassificationHistory(string historyDataJson)
        {
            var criteria = new CustClassificationHistoryArchiveCriteria
            {
                HistoryDataJson = historyDataJson
            };

            repository.Execute(criteria);
        }
    }
}