namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.RealtimeClassification
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public interface IRealtimeClassificationServices
    {
        public RealtimeClassificationServices GetCustomerGeneralCategoryHasChangeForRunRealtime();

        RealtimeClassificationServices ScanningRealtimeClassification(int batchSize, string categoryIds);

        IEnumerable<CustClassificationRealtimeEntity> GetCustomerGeneralCategoryForRunRealtime(IEnumerable<CustClassificationRealtimeEntity> customerHasChanges, string categoryIds);

        void ClassifyCustomerGeneralCategoryRealtime(IEnumerable<CustClassificationRealtimeEntity> custClassification);

        RealtimeClassificationServices RealtimeUpdateLastScanTime(bool isGeneralCategory);
    }

    public class RealtimeClassificationServices : IRealtimeClassificationServices
    {
        private readonly IRealtimeClassificationDataAccess realtimeClassificationDataAccess;
        private IEnumerable<CustClassificationRealtimeEntity> customerClassification;

        public RealtimeClassificationServices(IRealtimeClassificationDataAccess realtimeClassificationDataAccess)
        {
            this.realtimeClassificationDataAccess = realtimeClassificationDataAccess;
            customerClassification = new List<CustClassificationRealtimeEntity>();
        }

        public RealtimeClassificationServices GetCustomerGeneralCategoryHasChangeForRunRealtime()
        {
            this.customerClassification = realtimeClassificationDataAccess.GetCustomerGeneralCategoryHasChangeForRunRealtime();

            return this;
        }

        public RealtimeClassificationServices ScanningRealtimeClassification(int batchSize, string categoryIds)
        {
            if (customerClassification != null && customerClassification.Any())
            {
                var custChunks = customerClassification.Chunk(batchSize).ToList();

                foreach (var item in custChunks)
                {
                    var custClassification = GetCustomerGeneralCategoryForRunRealtime(item, categoryIds);

                    ClassifyCustomerGeneralCategoryRealtime(custClassification);

                    Thread.Sleep(Constants.Realtime_SleepTimes);
                }
            }

            return this;
        }

        public IEnumerable<CustClassificationRealtimeEntity> GetCustomerGeneralCategoryForRunRealtime(IEnumerable<CustClassificationRealtimeEntity> customerHasChanges, string categoryIds)
        {
            if (customerHasChanges != null && customerHasChanges.Any())
            {
                var custSportJson = JsonConvert.SerializeObject(customerHasChanges);

                return realtimeClassificationDataAccess.GetCustomerGeneralCategoryForRunRealtime(custSportJson, categoryIds);
            }

            return new List<CustClassificationRealtimeEntity>();
        }

        public void ClassifyCustomerGeneralCategoryRealtime(IEnumerable<CustClassificationRealtimeEntity> custClassification)
        {
            if (custClassification != null && custClassification.Any())
            {
                realtimeClassificationDataAccess.ClassifyCustomerGeneralCategoryRealtime(custClassification);
            }
        }

        public RealtimeClassificationServices RealtimeUpdateLastScanTime(bool isGeneralCategory)
        {
            realtimeClassificationDataAccess.RealtimeUpdateLastScanTime(isGeneralCategory);

            return this;
        }
    }
}