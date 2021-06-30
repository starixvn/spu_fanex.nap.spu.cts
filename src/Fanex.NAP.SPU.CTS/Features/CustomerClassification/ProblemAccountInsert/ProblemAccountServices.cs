namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProblemAccountInsert
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IProblemAccountServices
    {
        IEnumerable<ProblemAccountsEntity> GetProblemAccounts(DateTime? dateTime, string serviceName, bool IsWriteLog);

        IEnumerable<int> InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts, string serviceName, bool isWriteLog);
    }

    public class ProblemAccountServices : IProblemAccountServices
    {
        private readonly IProblemAccountDataAccess problemAccountDataAccess;
        private readonly ISentryClient sentryClient;

        public ProblemAccountServices(IProblemAccountDataAccess problemAccountDataAccess, ISentryClient sentryClient)
        {
            this.problemAccountDataAccess = problemAccountDataAccess;
            this.sentryClient = sentryClient;
        }

        public IEnumerable<ProblemAccountsEntity> GetProblemAccounts(DateTime? dateTime, string serviceName, bool isWriteLog)
        {
            var problemAccounts = new List<ProblemAccountsEntity>();

            string custIDs = (dateTime == null) ? Constants.InitialProblemAccounts : Constants.ProblemAccountByDateTime;

            problemAccounts.AddRange(problemAccountDataAccess.GetProblemAccounts_IBC(custIDs, dateTime));

            problemAccounts.AddRange(problemAccountDataAccess.GetProblemAccounts_CN88(custIDs, dateTime));

            Utilities.LogSentryInfo(serviceName, "Get ProblemAccount", problemAccounts, isWriteLog, sentryClient);
            return problemAccounts;
        }

        public IEnumerable<int> InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts, string serviceName, bool isWriteLog)
        {
            if (problemAccounts != null && problemAccounts.Any())
            {
                var prproblemAccountJson = JsonConvert.SerializeObject(problemAccounts);

                var result = problemAccountDataAccess.InsertProblemAccount(prproblemAccountJson, false);
                Utilities.LogSentryInfo(serviceName, "Insert ProblemAccount", result.CustIds, isWriteLog, sentryClient);

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    Utilities.LogSentryError(serviceName, "InsertProblemAccount throws Exception", problemAccounts, new Exception(result.ErrorMessage), sentryClient);
                }

                return result.CustIds;
            }

            return new List<int>();
        }
    }
}