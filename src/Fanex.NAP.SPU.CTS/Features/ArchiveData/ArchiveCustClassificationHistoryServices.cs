namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Threading;

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
            var isContinue = true;

            while (isContinue)
            {
                isContinue = dataAccess.CleanUpUnChangedCustClassificationHistory();

                Thread.Sleep(Consts.ArchiveCustClassification_SleepTimes);
            }
        }

        public void ArchiveChangedCustClassificationHistory()
        {
            var isContinue = true;

            while (isContinue)
            {
                var data = dataAccess.CleanUpChangedCustClassificationHistory();

                isContinue = data.IsExistData;

                if (data.CustClassificationHistory != null && data.CustClassificationHistory.Any())
                {
                    var custClassHistoryJson = JsonConvert.SerializeObject(data.CustClassificationHistory);

                    dataAccess.ArchiveCustClassificationHistory(custClassHistoryJson);
                }

                Thread.Sleep(Consts.ArchiveCustClassification_SleepTimes);
            }
        }
    }
}