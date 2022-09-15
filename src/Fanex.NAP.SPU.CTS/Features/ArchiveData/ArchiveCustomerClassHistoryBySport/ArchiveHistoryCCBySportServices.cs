namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData;
    using Newtonsoft.Json;
    using System.Linq;

    public interface IArchiveHistoryCCBySportServices
    {
        void ArchiveHistoryCCBySport();
    }

    public class ArchiveHistoryCCBySportServices : IArchiveHistoryCCBySportServices
    {
        private readonly IArchiveHistoryCCBySportDataAccess dataAccess;

        public ArchiveHistoryCCBySportServices(IArchiveHistoryCCBySportDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void ArchiveHistoryCCBySport()
        {
            var data = dataAccess.ArchiveHistoryCCBySport();

            if (data.CustClassificationHistory != null && data.CustClassificationHistory.Any())
            {
                var custClassHistoryJson = JsonConvert.SerializeObject(data.CustClassificationHistory);

                dataAccess.InsertHistoryCCBySportToArchiveDB(custClassHistoryJson);
            }
        }
    }
}