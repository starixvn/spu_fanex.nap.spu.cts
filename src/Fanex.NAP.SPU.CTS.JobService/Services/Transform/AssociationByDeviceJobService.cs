namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice;
    using Sentry;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAssociationByDeviceJobService
    {
        Task<bool> TransformAssociationByDevice(int batchSize, int numOfBatch);
    }

    public class AssociationByDeviceJobService : IAssociationByDeviceJobService
    {
        private readonly IAssociationByDeviceService associationByDeviceService;
        private readonly ISentryClient sentryClient;
        private readonly IAppSettings appSettings;

        public AssociationByDeviceJobService(ISentryClient sentryClient, IAssociationByDeviceService associationByDeviceService, IAppSettings appSettings)
        {
            this.sentryClient = sentryClient;
            this.associationByDeviceService = associationByDeviceService;
            this.appSettings = appSettings;
        }

        public Task<bool> TransformAssociationByDevice(int batchSize, int numOfBatch)
        {
            try
            {
                var rawData = associationByDeviceService.GetRawAssociation(batchSize, numOfBatch);

                if(rawData != null && rawData.Any())
                {
                    var groupTrans = rawData.GroupBy(x => x.BatchId);

                    Parallel.ForEach(groupTrans, trans =>
                    {
                        associationByDeviceService.TransformAssociationByDevice(trans);
                    });                                        
                }                                
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                      .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                      .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                      .ToString();

                sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);
               
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
