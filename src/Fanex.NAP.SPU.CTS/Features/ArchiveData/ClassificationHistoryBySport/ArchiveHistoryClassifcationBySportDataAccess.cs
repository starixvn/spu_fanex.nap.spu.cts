namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport
{
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;

    public interface IArchiveHistoryClassificationBySportDataAccess
    {
        HistoryClassificationBySportArchiveEntity ArchiveHistoryClassificationBySport();

        void InsertHistoryClassificationBySportToArchiveDB(string historyDataJson);
    }

    public class ArchiveHistoryClassifcationBySportDataAccess : IArchiveHistoryClassificationBySportDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveHistoryClassifcationBySportDataAccess()
        {
            repository = new DynamicRepository();
        }

        public HistoryClassificationBySportArchiveEntity ArchiveHistoryClassificationBySport()
        {
            var output = new HistoryClassificationBySportArchiveEntity
            {
                CustClassificationHistory = new List<HistoryClassificationBySportEntity>(),
                IsExistData = false
            };

            var criteria = new HistoryClassificationBySportArchiveCriteria();

            output.CustClassificationHistory = repository.Fetch<HistoryClassificationBySportEntity>(criteria);
            output.IsExistData = Convert.ToBoolean(criteria.IsDataExist);

            return output;
        }

        public void InsertHistoryClassificationBySportToArchiveDB(string historyDataJson)
        {
            var criteria = new HistoryClassificationBySportInsertCriteria
            {
                HistoryDataJson = historyDataJson
            };

            repository.Execute(criteria);
        }
    }
}