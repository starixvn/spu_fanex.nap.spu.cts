namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory;
    using Fanex.NAP.SPU.CTS.Shares;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveHistoryClassificationJobService
    {
        Task<bool> ArchiveHistoryClassification();
    }

    public class ArchiveHistoryClassificationJobService : IArchiveHistoryClassificationJobService
    {
        private readonly IArchiveHistoryClassificationServices archiveHistoryClassificationServices;
        private readonly string serviceName;

        public ArchiveHistoryClassificationJobService(IArchiveHistoryClassificationServices archiveHistoryClassificationServices)
        {
            this.archiveHistoryClassificationServices = archiveHistoryClassificationServices;
            this.serviceName = "ArchiveHistoryClassification";
        }

        public Task<bool> ArchiveHistoryClassification()
        {
            try
            {
                archiveHistoryClassificationServices.ArchiveHistoryClassification();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}