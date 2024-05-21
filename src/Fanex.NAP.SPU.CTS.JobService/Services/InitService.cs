namespace Fanex.NAP.SPU.CTS.JobService.Services
{
    using Fanex.NAP.Client.AspNetCore.Session;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
    using Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.ExistingCustomer;
    using Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus;
    using Fanex.NAP.SPU.CTS.Features.JobMonitoring.IncorrectDataMonitoring;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MonitorJobs.CustomerJobMonitor;
    using Fanex.NAP.SPU.CTS.Features.ProblemCaseMonitor;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Features.Transform.Accounts;
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice;
    using Fanex.NAP.SPU.CTS.Features.UpdateLastLogin;
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Fanex.NAP.SPU.CTS.JobService.Services.ArchiveAssociationCustomer;
    using Fanex.NAP.SPU.CTS.JobService.Services.ArchiveData;
    using Fanex.NAP.SPU.CTS.JobService.Services.ProblemCaseMonitor;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterNAPService(services);

            RegisterTransformAccount(services);

            RegisterExistingCustomer(services);

            RegisterMatchMonitor(services);

            RegisterUserPermission(services);

            RegisterAssociationByDevice(services);

            RegisterAffectedEvidence(services);

            RegisterUpdateLastLogin(services);

            RegisterDataAccesLayer(services);

            RegisterArchiveDataService(services);
            
            RegisterInitialCustomerStatus(services);

            RegisterAffectedEvidenceFromQueue(services);

            RegisterArchiveHistoryClassification(services);

            RegisterClassifyProblemCaseMonitor(services);
            
            RegisterArchiveAssociationCustomer(services);      

            RegisterArchiveHistoryClassificationBySport(services);

            RegisterAssociationDetection(services);
            
            RegisterJobMonitoring(services);
        }

        private static void RegisterAssociationDetection(IServiceCollection services)
        {
            services.AddSingleton<IAssociationDetectionDataAccess, AssociationDetectionDataAccess>();
            services.AddSingleton<IAssociationDetectionService, AssociationDetectionService>();
        }

        private static void RegisterArchiveDataService(this IServiceCollection services)
        {
            services.AddSingleton<IArchiveCustomerService, ArchiveCustomerService>();
            services.AddSingleton<IArchiveCustomerDataAccess, ArchiveCustomerDataAccess>();
            services.AddSingleton<IArchiveCustomerJobService, ArchiveCustomerJobService>();

            services.AddSingleton<IArchiveCustomerMissingDataAccess, ArchiveCustomerMissingDataAccess>();
            services.AddSingleton<IArchiveCustomerMissingService, ArchiveCustomerMissingService>();
            services.AddSingleton<IArchiveCustomerMissingJobService, ArchiveCustomerMissingJobService>();
        }

        private static void RegisterNAPService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<INapSession, NapSession>();
            services.AddSingleton<ISecurityService, SecurityService>();
        }

        private static void RegisterTransformAccount(this IServiceCollection services)
        {
            services.AddSingleton<IAccountDataAccess, AccountDataAccess>();
            services.AddSingleton<IAccountServices, AccountServices>();
            services.AddSingleton<IAccountJobService, AccountJobService>();
        }

        private static void RegisterExistingCustomer(this IServiceCollection services)
        {
            services.AddSingleton<IExistingCustomerDataAccess, ExistingCustomerDataAccess>();
            services.AddSingleton<IExistingCustomerServices, ExistingCustomerServices>();
            services.AddSingleton<IExistingCustomerJobService, ExistingCustomerJobService>();
        }

        private static void RegisterMatchMonitor(this IServiceCollection services)
        {
            services.AddSingleton<IMatchMonitorDataAccess, MatchMonitorDataAccess>();
            services.AddSingleton<IMatchMonitorServices, MatchMonitorServices>();
            services.AddSingleton<IMatchMonitorJobService, MatchMonitorJobService>();
        }

        private static void RegisterUserPermission(this IServiceCollection services)
        {
            services.AddSingleton<IUserPermissionService, UserPermissionService>();
            services.AddSingleton<IUserPermissionJobService, UserPermissionJobService>();
            services.AddSingleton<IUserPermissionDataAccess, UserPermissionDataAccess>();
        }

        private static void RegisterAssociationByDevice(this IServiceCollection services)
        {
            services.AddSingleton<IAssociationByDeviceJobService, AssociationByDeviceJobService>();
            services.AddSingleton<IAssociationByDeviceService, AssociationByDeviceService>();
            services.AddSingleton<IAssociationByDeviceDataAccess, AssociationByDeviceDataAccess>();
        }

        private static void RegisterAffectedEvidence(this IServiceCollection services)
        {
            services.AddSingleton<IAffectedEvidenceJobService, AffectedEvidenceJobService>();
            services.AddSingleton<IAffectedEvidenceService, AffectedEvidenceService>();
            services.AddSingleton<IAffectedEvidenceDataAccess, AffectedEvidenceDataAccess>();
        }

        private static void RegisterUpdateLastLogin(this IServiceCollection services)
        {
            services.AddSingleton<IUpdateLastLoginJobService, UpdateLastLoginJobService>();
            services.AddSingleton<IUpdateLastLoginService, UpdateLastLoginService>();
            services.AddSingleton<IUpdateLastLoginDataAccess, UpdateLastLoginDataAccess>();
        }

        private static void RegisterDataAccesLayer(this IServiceCollection services)
        {
            services.AddSingleton<ISystemParameterServices, SystemParameterServices>();
            services.AddSingleton<ISystemParameterDataAccess, SystemParameterDataAccess>();
        }

        private static void RegisterInitialCustomerStatus(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerStatusDataAccess, CustomerStatusDataAccess>();
            services.AddSingleton<ICustomerStatusServices, CustomerStatusServices>();
            services.AddSingleton<ICustomerStatusJobService, CustomerStatusJobService>();
        }

        private static void RegisterAffectedEvidenceFromQueue(this IServiceCollection services)
        {
            services.AddSingleton<IAffectedEvidenceFromQueueDataAccess, AffectedEvidenceFromQueueDataAccess>();
            services.AddSingleton<IAffectedEvidenceFromQueueService, AffectedEvidenceFromQueueService>();
            services.AddSingleton<IAffectedEvidenceFromQueueJobService, AffectedEvidenceFromQueueJobService>();
        }

        private static void RegisterArchiveHistoryClassification(this IServiceCollection services)
        {
            services.AddSingleton<IArchiveHistoryClassificationDataAccess, ArchiveHistoryClassificationDataAccess>();
            services.AddSingleton<IArchiveHistoryClassificationServices, ArchiveHistoryClassificationServices>();
            services.AddSingleton<IArchiveHistoryClassificationJobService, ArchiveHistoryClassificationJobService>();
        }

        private static void RegisterClassifyProblemCaseMonitor(this IServiceCollection services)
        {
            services.AddSingleton<IProblemCaseMonitorDataAccess, ProblemCaseMonitorDataAccess>();
            services.AddSingleton<IProblemCaseMonitorService, ProblemCaseMonitorService>();
            services.AddSingleton<IProblemCaseMonitorClassificationJobService, ProblemCaseMonitorClassificationJobService>();
            services.AddSingleton<IProblemCaseMonitorAssociationJobService, ProblemCaseMonitorAssociationJobService>();
        }
       
        private static void RegisterArchiveAssociationCustomer(this IServiceCollection services)
        {
            services.AddSingleton<IArchiveAssociationCustomerDataAccess, ArchiveAssociationCustomerDataAccess>();
            services.AddSingleton<IRollbackAssociationCustomerDataAccess, RollbackAssociationCustomerDataAccess>();
            services.AddSingleton<IArchiveAssociationCustomerService, ArchiveAssociationCustomerService>();
            services.AddSingleton<IRollbackAssociationCustomerService, RollbackAssociationCustomerService>();
            services.AddSingleton<IArchiveAssociationCustomerJobService, ArchiveAssociationCustomerJobService>();
            services.AddSingleton<IRollbackAssociationCustomerJobService, RollbackAssociationCustomerJobService>();
        }       

        private static void RegisterArchiveHistoryClassificationBySport(this IServiceCollection services)
        {
            services.AddSingleton<IArchiveHistoryClassificationBySportDataAccess, ArchiveHistoryClassifcationBySportDataAccess>();
            services.AddSingleton<IArchiveHistoryClassificationBySportServices, ArchiveHistoryClassificationBySportServices>();
            services.AddSingleton<IArchiveHistoryClassificationBySportJobService, ArchiveHistoryClassificationBySportJobService>();
        }

        private static void RegisterJobMonitoring(this IServiceCollection services)
        {
            services.AddSingleton<IIncorrectDataMonitoringDataAccess, IncorrectDataMonitoringDataAccess>();
            services.AddSingleton<IIncorrectDataMonitoringService, IncorrectDataMonitoringService>();

            services.AddSingleton<ILoginTransactionMonitoringDataAccess, LoginTransactionMonitoringDataAccess>();
            services.AddSingleton<ILoginTransactionMonitoringService, LoginTransactionMonitoringService>();
            services.AddSingleton<ILoginTransactionMonitoringJobService, LoginTransactionMonitoringJobService>();
        }
    }
}