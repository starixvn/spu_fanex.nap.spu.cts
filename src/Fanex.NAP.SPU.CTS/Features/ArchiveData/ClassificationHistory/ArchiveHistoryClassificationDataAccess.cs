namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;

    public interface IArchiveHistoryClassificationDataAccess
    {
        HistoryClassificationArchiveEntity ArchiveHistoryClassification();

        void InsertHistoryClassificationToArchiveDB(string historyDataJson);
    }

    public class ArchiveHistoryClassificationDataAccess : IArchiveHistoryClassificationDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveHistoryClassificationDataAccess()
        {
            repository = new DynamicRepository();
        }

        public HistoryClassificationArchiveEntity ArchiveHistoryClassification()
        {
            var output = new HistoryClassificationArchiveEntity
            {
                CustClassificationHistory = new List<HistoryClassificationEntity>(),
                IsExistData = false
            };

            var criteria = new HistoryClassificationArchiveCriteria();

            output.CustClassificationHistory = repository.Fetch<HistoryClassificationEntity>(criteria);
            output.IsExistData = Convert.ToBoolean(criteria.IsDataExist);

            return output;
        }

        public void InsertHistoryClassificationToArchiveDB(string historyDataJson)
        {
            var criteria = new HistoryClassificationInsertCriteria
            {
                HistoryDataJson = historyDataJson
            };

            repository.Execute(criteria);
        }
    }
}