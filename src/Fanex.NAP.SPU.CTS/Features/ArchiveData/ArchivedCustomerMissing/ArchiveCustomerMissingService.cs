namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer
{
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IArchiveCustomerMissingService
    {
        void ArchiveCreditCustomerMissing(int batchSize);

        void ArchiveDepositCustomerMissing(int batchSize);
    }

    public class ArchiveCustomerMissingService : IArchiveCustomerMissingService
    {
        private readonly ISystemParameterServices sysParamService;
        private readonly IArchiveCustomerMissingDataAccess archiveCustMissingDataAccess;

        public ArchiveCustomerMissingService(ISystemParameterServices sysParamService
                                            , IArchiveCustomerMissingDataAccess archiveCustMissingDataAccess)
        {
            this.sysParamService = sysParamService;
            this.archiveCustMissingDataAccess = archiveCustMissingDataAccess;
        }

        public void ArchiveCreditCustomerMissing(int batchSize)
        {
            var lastTimeArchive_Credit = sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_LastArchiveDate_Credit);
            var lastCustIdArchive_Credit = sysParamService.GetSystemParamById<int>(SystemParameters.CorrectArchiveCustomer_LastCustID_Credit);
            var maxCustIdArchive_Credit = sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_MaxArchiveDate_Credit);

            var archivedCusts_Credit = archiveCustMissingDataAccess.GetArchivedCustomerMissing(lastCustIdArchive_Credit, lastTimeArchive_Credit, maxCustIdArchive_Credit, batchSize, SubscriberType.Credit);

            if (archivedCusts_Credit != null && archivedCusts_Credit.Any())
            {
                archiveCustMissingDataAccess.ArchiveCustomerMissing(JsonConvert.SerializeObject(archivedCusts_Credit), SubscriberType.Credit);
            }
        }

        public void ArchiveDepositCustomerMissing(int batchSize)
        {
            var lastTimeArchive_Deposit = sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_LastArchiveDate_Deposit);
            var lastCustIdArchive_Deposit = sysParamService.GetSystemParamById<int>(SystemParameters.CorrectArchiveCustomer_LastCustID_Deposit);
            var maxCustIdArchive_Deposit = sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_MaxArchiveDate_Deposit);

            var archivedCusts_Deposit = archiveCustMissingDataAccess.GetArchivedCustomerMissing(lastCustIdArchive_Deposit, lastTimeArchive_Deposit, maxCustIdArchive_Deposit, batchSize, SubscriberType.Deposit);

            if (archivedCusts_Deposit != null && archivedCusts_Deposit.Any())
            {
                archiveCustMissingDataAccess.ArchiveCustomerMissing(JsonConvert.SerializeObject(archivedCusts_Deposit), SubscriberType.Deposit);
            }
        }
    }
}