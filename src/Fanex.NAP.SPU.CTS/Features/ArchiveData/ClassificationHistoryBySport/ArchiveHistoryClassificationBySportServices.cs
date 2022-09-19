namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport
{
    using Newtonsoft.Json;
    using System.Linq;

    public interface IArchiveHistoryClassificationBySportServices
    {
        void ArchiveHistoryClassificationBySport();
    }

    public class ArchiveHistoryClassificationBySportServices : IArchiveHistoryClassificationBySportServices
    {
        private readonly IArchiveHistoryClassificationBySportDataAccess dataAccess;

        public ArchiveHistoryClassificationBySportServices(IArchiveHistoryClassificationBySportDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void ArchiveHistoryClassificationBySport()
        {
            var data = dataAccess.ArchiveHistoryClassificationBySport();

            if (data.CustClassificationHistory != null && data.CustClassificationHistory.Any())
            {
                var custClassHistoryJson = JsonConvert.SerializeObject(data.CustClassificationHistory);

                dataAccess.InsertHistoryClassificationBySportToArchiveDB(custClassHistoryJson);
            }
        }
    }
}