namespace Fanex.NAP.SPU.CTS.Features.ArchiveData
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Criteria;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Entities;
    using System;
    using System.Collections.Generic;

    public interface IArchiveCustClassificationHistoryDataAccess
    {
        HistoryClassificationArchiveEntity ArchiveCustClassificationHistory();

        void InsertCustClassificationHistoryToArchiveDB(string historyDataJson);
    }

    public class ArchiveCustClassificationHistoryDataAccess : IArchiveCustClassificationHistoryDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveCustClassificationHistoryDataAccess()
        {
            repository = new DynamicRepository();
        }

        public HistoryClassificationArchiveEntity ArchiveCustClassificationHistory()
        {
            var output = new HistoryClassificationArchiveEntity
            {
                CustClassificationHistory = new List<CustClassificationHistoryEntity>(),
                IsExistData = false
            };

            var criteria = new ChangedCustClassificationHistoryCleanUpCriteria();

            output.CustClassificationHistory = repository.Fetch<CustClassificationHistoryEntity>(criteria);
            output.IsExistData = Convert.ToBoolean(criteria.IsDataExist);

            return output;
        }

        public void InsertCustClassificationHistoryToArchiveDB(string historyDataJson)
        {
            var criteria = new CustClassificationHistoryArchiveCriteria
            {
                HistoryDataJson = historyDataJson
            };

            repository.Execute(criteria);
        }
    }
}