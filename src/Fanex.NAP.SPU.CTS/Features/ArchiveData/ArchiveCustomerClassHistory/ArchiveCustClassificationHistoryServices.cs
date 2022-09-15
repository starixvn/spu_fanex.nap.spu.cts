namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData;
    using Newtonsoft.Json;
    using System.Linq;

    public interface IArchiveCustClassificationHistoryServices
    {
        void CleanUpUnChangedCustClassificationHistory();

        void ArchiveChangedCustClassificationHistory();
    }

    public class ArchiveCustClassificationHistoryServices : IArchiveCustClassificationHistoryServices
    {
        private readonly IArchiveCustClassificationHistoryDataAccess dataAccess;

        public ArchiveCustClassificationHistoryServices(IArchiveCustClassificationHistoryDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void CleanUpUnChangedCustClassificationHistory()
        {
            dataAccess.CleanUpUnChangedCustClassificationHistory();
        }

        public void ArchiveChangedCustClassificationHistory()
        {
            var data = dataAccess.CleanUpChangedCustClassificationHistory();

            if (data.CustClassificationHistory != null && data.CustClassificationHistory.Any())
            {
                var custClassHistoryJson = JsonConvert.SerializeObject(data.CustClassificationHistory);

                dataAccess.ArchiveCustClassificationHistory(custClassHistoryJson);
            }
        }
    }
}