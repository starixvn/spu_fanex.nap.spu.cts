namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    public interface IArchiveCustClassificationHistoryServices
    {
        void CleanUpUnChangedCustClassificationHistoryContinuously();

        void ArchiveChangedCustClassificationHistoryContinuously();

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

        public void CleanUpUnChangedCustClassificationHistoryContinuously()
        {
            var isContinue = true;
            var watch = new Stopwatch();
            watch.Start();

            while (isContinue && !Utilities.IsExpireHttpRequest(watch))
            {
                isContinue = dataAccess.CleanUpUnChangedCustClassificationHistory();

                Thread.Sleep(Consts.ArchiveCustClassification_SleepTimes);
            }
        }

        public void ArchiveChangedCustClassificationHistoryContinuously()
        {
            var isContinue = true;
            var watch = new Stopwatch();
            watch.Start();

            while (isContinue && !Utilities.IsExpireHttpRequest(watch))
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