namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory
{
    using Newtonsoft.Json;
    using System.Linq;

    public interface IArchiveHistoryClassificationServices
    {
        void ArchiveHistoryClassification();
    }

    public class ArchiveHistoryClassificationServices : IArchiveHistoryClassificationServices
    {
        private readonly IArchiveHistoryClassificationDataAccess dataAccess;

        public ArchiveHistoryClassificationServices(IArchiveHistoryClassificationDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void ArchiveHistoryClassification()
        {
            var data = dataAccess.ArchiveHistoryClassification();

            if (data.CustClassificationHistory != null && data.CustClassificationHistory.Any())
            {
                var custClassHistoryJson = JsonConvert.SerializeObject(data.CustClassificationHistory);

                dataAccess.InsertHistoryClassificationToArchiveDB(custClassHistoryJson);
            }
        }
    }
}