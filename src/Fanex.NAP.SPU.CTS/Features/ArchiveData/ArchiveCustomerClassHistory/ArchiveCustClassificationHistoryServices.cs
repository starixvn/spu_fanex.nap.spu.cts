namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData;
    using Newtonsoft.Json;
    using System.Linq;

    public interface IArchiveCustClassificationHistoryServices
    {
        void ArchiveCustClassificationHistory();
    }

    public class ArchiveCustClassificationHistoryServices : IArchiveCustClassificationHistoryServices
    {
        private readonly IArchiveCustClassificationHistoryDataAccess dataAccess;

        public ArchiveCustClassificationHistoryServices(IArchiveCustClassificationHistoryDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void ArchiveCustClassificationHistory()
        {
            var data = dataAccess.ArchiveCustClassificationHistory();

            if (data.CustClassificationHistory != null && data.CustClassificationHistory.Any())
            {
                var custClassHistoryJson = JsonConvert.SerializeObject(data.CustClassificationHistory);

                dataAccess.InsertCustClassificationHistoryToArchiveDB(custClassHistoryJson);
            }
        }
    }
}