namespace Fanex.NAP.SPU.CTS.Features.ArchiveData
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Criteria;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Entities;
    using System;
    using System.Collections.Generic;

    public interface IArchiveHistoryCCBySportDataAccess
    {
        HistoryCCBySportArchiveEntity ArchiveHistoryCCBySport();

        void InsertHistoryCCBySportToArchiveDB(string historyDataJson);
    }

    public class ArchiveHistoryCCBySportDataAccess : IArchiveHistoryCCBySportDataAccess
    {
        private readonly IDynamicRepository repository;

        public ArchiveHistoryCCBySportDataAccess()
        {
            repository = new DynamicRepository();
        }

        public HistoryCCBySportArchiveEntity ArchiveHistoryCCBySport()
        {
            var output = new HistoryCCBySportArchiveEntity
            {
                CustClassificationHistory = new List<CustomerClassificationBySportEntity>(),
                IsExistData = false
            };

            var criteria = new HistoryCCBySportArchiveCriteria();

            output.CustClassificationHistory = repository.Fetch<CustomerClassificationBySportEntity>(criteria);
            output.IsExistData = Convert.ToBoolean(criteria.IsDataExist);

            return output;
        }

        public void InsertHistoryCCBySportToArchiveDB(string historyDataJson)
        {
            var criteria = new HistoryCCBySportArchiveDBInsertCriteria
            {
                HistoryDataJson = historyDataJson
            };

            repository.Execute(criteria);
        }
    }
}