namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.RescanCustomerClassification
{
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.DailyClassification;
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProblemAccountInsert;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.FeedCCToSoon88;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface IRescanCustClassificationService
    {
        void RescanCustomerClassify(RescanParametersEntity paras);

        IEnumerable<int> GetAgentCusts(IEnumerable<int> custIds);

        IEnumerable<int> InsertCustEvidenceCategorToCTSy(IEnumerable<int> custIds, FlowTypeRescanCC flowTypeRescanCC, int internalBatchSize, string serviceName);

        IEnumerable<CustEvidenceEntity> GetExistedCustomerEvidences(string custIds);

        IEnumerable<int> SyncCustomerEvidenceToCategory(IEnumerable<CustEvidenceEntity> existedEvidences, bool isWriteLogRescan, int internalBatchSize, string serviceName);

        IEnumerable<ProblemAccountsEntity> GetProblemAccounts(IEnumerable<int> custIds, bool isCheckPAFromNAP);

        IEnumerable<long> SyncRobotUserToUpdateCategory(IEnumerable<int> custIds);

        IEnumerable<ProblemAccountsEntity> GetProblemAccountFromNAPPA(string custIds);

        IEnumerable<int> InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts, RescanParametersEntity paras);

        void PushCustClassificationToMainDB(IEnumerable<int> custIds, RescanParametersEntity paras);

        void RescanNormalAccount(IEnumerable<int> customers, int internalBatchSize);
    }

    public class RescanCustClassificationService : IRescanCustClassificationService
    {
        private readonly IRescanCustClassificationDataAccess rescanCustClassificationDataAccess;
        private readonly IProblemAccountServices problemAccountServices;
        private readonly ICustClassToMainDBService custClassToMainDBService;
        private readonly IFeedSoon88Services feedSoon88Services;
        private readonly ISentryClient sentryClient;
        private readonly IDailyClassificationServices dailyClassificationServices;

        public RescanCustClassificationService(
            IRescanCustClassificationDataAccess rescanCustClassificationDataAccess,
            ISentryClient sentryClient,
            IProblemAccountServices problemAccountServices,
            ICustClassToMainDBService custClassToMainDBService,
            IFeedSoon88Services feedSoon88Services,
            IDailyClassificationServices dailyClassificationServices)
        {
            this.rescanCustClassificationDataAccess = rescanCustClassificationDataAccess;
            this.problemAccountServices = problemAccountServices;
            this.custClassToMainDBService = custClassToMainDBService;
            this.feedSoon88Services = feedSoon88Services;
            this.sentryClient = sentryClient;
            this.dailyClassificationServices = dailyClassificationServices;
        }

        public void RescanCustomerClassify(RescanParametersEntity paras)
        {
            if (paras.CustIds == null || paras.CustIds.Any())
            {
                return;
            }

            var custIdList = new List<int>();

            //Rescan PA - Get SMA CustId
            var roleCusomters = GetAgentCusts(paras.CustIds);
            custIdList.AddRange(roleCusomters);

            // Rescan PA - Get PA From Evidence in CTS
            var problemAccountsFromEvi = InsertCustEvidenceCategorToCTSy(paras.CustIds, paras.FlowTypeRescanCC, paras.InternalBatchSize, paras.ServiceName);
            custIdList.AddRange(problemAccountsFromEvi);

            // Rescan PA - Get PA From RobotUser & NAP PA
            var problemAccounts = GetProblemAccounts(paras.CustIds, paras.IsCheckPAFromNAP);
            if (problemAccounts != null && problemAccounts.Any())
            {
                var custIds = InsertProblemAccountToCTS(problemAccounts, paras);
                custIdList.AddRange(custIds);
            }

            //Rescan PA - Push CC & CN88
            var custIdsToSyncCC = custIdList?.Distinct();

            if (custIdsToSyncCC != null && custIdsToSyncCC.Any())
            {
                PushCustClassificationToMainDB(custIdsToSyncCC, paras);
            }

            // Rescan Normal
            paras.CustIds.ToList().RemoveAll(x => custIdsToSyncCC.Contains(x));

            if (paras.CustIds != null && paras.CustIds.Any())
            {
                RescanNormalAccount(paras.CustIds, paras.InternalBatchSize);
            }
        }

        public IEnumerable<int> GetAgentCusts(IEnumerable<int> custIds)
        {
            var agentIds = new List<int>();

            if (custIds != null && custIds.Any())
            {
                var data = rescanCustClassificationDataAccess.GetRoleCustomer(string.Join(",", custIds));

                if (data != null && data.Any())
                {
                    agentIds = data.Where(c => c.RoleID == (int)CustomerRole.Agent
                                            || c.RoleID == (int)CustomerRole.Master
                                            || c.RoleID == (int)CustomerRole.Super)
                               .Select(c => c.CustID)?.ToList();
                }
            }

            return agentIds;
        }

        public IEnumerable<int> InsertCustEvidenceCategorToCTSy(IEnumerable<int> custIds, FlowTypeRescanCC flowTypeRescanCC, int internalBatchSize, string serviceName)
        {
            var existedCustEvidences = GetExistedCustomerEvidences(string.Join(",", custIds));

            var isWriteLogRescan = flowTypeRescanCC != FlowTypeRescanCC.NewMember;

            var custIdsToSyncCC = SyncCustomerEvidenceToCategory(existedCustEvidences, isWriteLogRescan, internalBatchSize, serviceName);

            return custIdsToSyncCC;
        }

        public IEnumerable<CustEvidenceEntity> GetExistedCustomerEvidences(string custIds)
        {
            IEnumerable<CustEvidenceEntity> existedEvidences = new List<CustEvidenceEntity>();

            if (!string.IsNullOrEmpty(custIds))
            {
                existedEvidences = rescanCustClassificationDataAccess.GetExistedEvidences(custIds);
            }

            return existedEvidences;
        }

        public IEnumerable<int> SyncCustomerEvidenceToCategory(IEnumerable<CustEvidenceEntity> existedEvidences, bool isWriteLogRescan, int internalBatchSize, string serviceName)
        {
            var custIds = new List<int>();

            if (existedEvidences != null && existedEvidences.Any())
            {
                var evidencesChunk = existedEvidences.Chunk(internalBatchSize);

                foreach (var evidences in evidencesChunk)
                {
                    var result = rescanCustClassificationDataAccess.SyncCustomerEvidenceToCategory(JsonConvert.SerializeObject(evidences), isWriteLogRescan);
                    custIds.AddRange((IEnumerable<int>)result.OutParam["custIds"]);

                    if (result.ErrorCode == -1)
                    {
                        Utilities.LogSentryError(serviceName, "RescanCC - SyncCustEvidence fail", evidences, new Exception(result.ErrorMsg), sentryClient);
                    }
                }
            }

            return custIds;
        }

        public IEnumerable<ProblemAccountsEntity> GetProblemAccounts(IEnumerable<int> custIds, bool isCheckPAFromNAP)
        {
            var problemAccounts = new List<ProblemAccountsEntity>();

            if (custIds != null && custIds.Any())
            {
                //PA - RobotUser
                var robotUsers = SyncRobotUserToUpdateCategory(custIds);

                if (robotUsers != null && robotUsers.Any())
                {
                    robotUsers.ToList().ForEach(c =>
                    {
                        problemAccounts.Add(new ProblemAccountsEntity
                        {
                            CustID = c,
                            ReasonID = (int)Category.Robot
                        });
                    });
                }

                //PA - NAP PA
                if (isCheckPAFromNAP)
                {
                    var existedProblemAccounts = GetProblemAccountFromNAPPA(string.Join(",", custIds)).ToList();
                    problemAccounts.AddRange(existedProblemAccounts);
                }
            }

            return problemAccounts;
        }

        public IEnumerable<long> SyncRobotUserToUpdateCategory(IEnumerable<int> custIds)
        {
            var robotUser = new List<long>();

            var custs = rescanCustClassificationDataAccess.GetCustByRobotSiteRule(string.Join(",", custIds)).ToList();

            var getRobotUser = rescanCustClassificationDataAccess.GetExistedRobotUser(string.Join(",", custs));

            if (!string.IsNullOrEmpty(getRobotUser))
            {
                robotUser = getRobotUser.Split(',').Select(long.Parse).ToList();
            }

            return robotUser;
        }

        public IEnumerable<ProblemAccountsEntity> GetProblemAccountFromNAPPA(string custIds)
        {
            var problemAccounts = new List<ProblemAccountsEntity>();

            if (!string.IsNullOrEmpty(custIds))
            {
                var problemAccountsNAPCM = problemAccountServices.GetProblemAccounts_IBC(custIds, null).ToList();
                var problemAccountsCN88 = problemAccountServices.GetProblemAccounts_CN88(custIds, null).ToList();

                problemAccounts.AddRange(problemAccountsNAPCM);
                problemAccounts.AddRange(problemAccountsCN88);
            }

            return problemAccounts?.Distinct();
        }

        public IEnumerable<int> InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts, RescanParametersEntity paras)
        {
            var custIds = new List<int>();

            if (problemAccounts != null && problemAccounts.Any())
            {
                var problemAccountsChunk = problemAccounts.Chunk(paras.InternalBatchSize);

                var isWriteLogRescan = paras.FlowTypeRescanCC != FlowTypeRescanCC.NewMember;

                foreach (var problemAccs in problemAccountsChunk)
                {
                    var problemCustIds = problemAccountServices.InsertProblemAccountToCTS(problemAccs, isWriteLogRescan, paras.ServiceName, paras.IsWriteLog);

                    custIds.AddRange(problemCustIds);
                }
            }

            return custIds;
        }

        public void PushCustClassificationToMainDB(IEnumerable<int> custIds, RescanParametersEntity paras)
        {
            if (custIds == null || !custIds.Any())
            {
                return;
            }

            var custIdsChunk = custIds.Distinct().Chunk(paras.ExternalBatchSize);

            foreach (var custids in custIdsChunk)
            {
                var lastestClassifications = custClassToMainDBService.GetLatestClassification(custids, paras.ServiceName, paras.IsWriteLog);

                if (lastestClassifications != null && lastestClassifications.Any())
                {
                    custClassToMainDBService.PushCustClassificationToMainDB(lastestClassifications, paras.ServiceName, paras.IsWriteLog);

                    if (paras.IsPushCCToSoon88)
                    {
                        feedSoon88Services.FeedCCToSoon88(lastestClassifications, paras.FeedSoon88API, paras.ServiceName, paras.IsWriteLog);
                    }
                }
            }
        }

        public void RescanNormalAccount(IEnumerable<int> customers, int internalBatchSize)
        {
            if (customers == null || !customers.Any())
            {
                return;
            }

            var custIdsChunk = customers.Chunk(internalBatchSize).ToList();

            foreach (var custIds in custIdsChunk)
            {
                dailyClassificationServices.ClassifyCustomerGeneralCategoryDaily(custIds, false);
            }
        }
    }
}