namespace Fanex.NAP.SPU.CTS.Features.CTSCustomer
{
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.CustomerClassification.RescanCustomerClassification;
    using Fanex.NAP.SPU.CTS.Features.Transform.Accounts;
    using Fanex.NAP.SPU.CTS.Features.Transform.CTSCustomer;
    using Fanex.NAP.SPU.CTS.Features.Transform.CTSCustomer.Entities;
    using Fanex.NAP.SPU.CTS.Features.UserPermission.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface ICTSCustomerService
    {
        LastScannedCustModel GetLastInsertCust();

        UpdateCustomerModel GetCustomerFromMainDB(int numOfAccount, Int64 lastScannedCustID);

        List<CustomerMainDBModel> GetCustSubFromMainDB(int numOfAccount, Int64 lastScannedCustID);

        UpdateCustomerModel PrepareData(UpdateCustomerModel customers, List<CustomerMainDBModel> custSubs);

        bool InsertNewCustomer(UpdateCustomerModel data, bool isWriteLogInfo, int internalBatchSize, string soon88Url);
    }

    public class CTSCustomerService : ICTSCustomerService
    {
        private readonly ICTSCustomerDataAccess dataAccess;
        private readonly ISentryClient sentryClient;
        private readonly IRescanCustClassificationService rescanClassificationService;
        private readonly IAccountServices accountService;
        private readonly string serviceName = "CTSCustomerService";

        public CTSCustomerService(ICTSCustomerDataAccess dataAccess, 
            ISentryClient sentryClient,
            IRescanCustClassificationService rescanClassificationService,
            IAccountServices accountService)
        {
            this.dataAccess = dataAccess;
            this.rescanClassificationService = rescanClassificationService;
            this.accountService = accountService;
            this.sentryClient = sentryClient;
        }

        public LastScannedCustModel GetLastInsertCust()
        {
            return dataAccess.GetLastInsertCust();
        }

        public UpdateCustomerModel GetCustomerFromMainDB(int numOfAccount, Int64 lastScannedCustID)
        {
            return dataAccess.GetCustomerFromMainDB(numOfAccount, lastScannedCustID);
        }

        public List<CustomerMainDBModel> GetCustSubFromMainDB(int numOfAccount, Int64 lastScannedCustID)
        {
            return dataAccess.GetCustSubFromMainDB(numOfAccount, lastScannedCustID);
        }

        public IEnumerable<InsertCustomerModel> TransformCustomerFromMainDB(List<CustomerMainDBModel> customerList)
        {
            var custInfos = new List<InsertCustomerModel>();
            var customerJson = JsonConvert.SerializeObject(customerList);

            try
            {
                custInfos = dataAccess.TransformCustomerFromMainDB(customerJson)?.ToList();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError("InsertNewCustomer", "TransformCustomerFromMainDB exception:", customerJson, ex, sentryClient);
            }

            return custInfos;
        }

        public void UpdateCustomerComplete(string customers, DateTime? lastUpdateTime, Int64? lastUpdateCustID, UpdateCustomer sourceType)
        {
            if (sourceType == UpdateCustomer.FromCustomer)
            {
                dataAccess.UpdateCustomerComplete(customers, lastUpdateTime, lastUpdateCustID);
            }
            else
            {
                dataAccess.UpdateCustInfoComplete(customers, lastUpdateTime, lastUpdateCustID);
            }
        }

        public UpdateCustomerModel PrepareData(UpdateCustomerModel customers, List<CustomerMainDBModel> custSubs)
        {
            var custsToCTS = new UpdateCustomerModel()
            {
                CustomerInfos = new List<CustomerMainDBModel>(),
                LastInsertCustID = null
            };

            custsToCTS.CustomerInfos.AddRange(customers?.CustomerInfos);
            custsToCTS.CustomerInfos.AddRange(custSubs);
            custsToCTS.LastInsertCustID = customers?.LastInsertCustID;

            return custsToCTS;
        }

        public bool InsertNewCustomer(UpdateCustomerModel data, bool isWriteLogInfo, int internalBatchSize, string soon88Url)
        {
            var customers = data.CustomerInfos;
            var lastCustID = data.LastInsertCustID;

            try
            {
                Utilities.LogSentryInfo(serviceName, "InsertNewCustomer start", customers, isWriteLogInfo, sentryClient);
                if (customers != null && customers.Any())
                {
                    var customersChunk = customers.Distinct().Chunk(internalBatchSize);

                    foreach (var custs in customersChunk)
                    {
                        var custIdList = TransformCustomerFromMainDB(custs.ToList());

                        Utilities.LogSentryInfo(serviceName, "Insert", custs, isWriteLogInfo, sentryClient);

                        if (custIdList != null && custIdList.Any())
                        {
                            var tasks = new List<Task>();

                            tasks.Add(Task.Run(() =>
                            {
                                try
                                {
                                    var custToRescan = custIdList.Where(c => !c.IsExisting).Select(c => c.CustID).Distinct();
                                    var rescanParam = new RescanParametersEntity
                                    {
                                        CustIds = custToRescan,
                                        FlowTypeRescanCC = FlowTypeRescanCC.NewMember,
                                        IsCheckPAFromNAP = true,
                                        IsPushCCToSoon88 = true,
                                        ServiceName = serviceName,
                                        FeedSoon88API = soon88Url,
                                        IsWriteLog = isWriteLogInfo,
                                        InternalBatchSize = internalBatchSize,
                                        ExternalBatchSize = internalBatchSize
                                    };
                                    rescanClassificationService.RescanCustomerClassify(rescanParam);

                                    Utilities.LogSentryInfo(serviceName, "InsertNewCustomer - RescanCustomerClassify done", custToRescan, isWriteLogInfo, sentryClient);
                                }
                                catch (Exception ex)
                                {
                                    Utilities.LogSentryError(serviceName, "InsertNewCustomer - RescanCustomerClassify done", custIdList, ex, sentryClient);
                                }
                            }));

                            tasks.Add(Task.Run(() =>
                            {
                                try
                                {
                                    accountService.ReTransformAccount(JsonConvert.SerializeObject(custIdList), lastCustID);

                                    Utilities.LogSentryInfo(serviceName, "InsertNewCustomer - Insert complete", custIdList, isWriteLogInfo, sentryClient);
                                }
                                catch (Exception ex)
                                {
                                    Utilities.LogSentryError(serviceName, "InsertNewCustomer - RReTransformAccount throws exception", custIdList, ex, sentryClient);
                                }
                            }));

                            Task.WhenAll(tasks.ToArray()).Wait();
                        }
                    }
                }

                if (isWriteLogInfo)
                    sentryClient.CaptureMessage(serviceName + " - InsertNewCustomer done");

                return true;
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, "InsertNewCustomer throws exception", customers, ex, sentryClient);                
            }

            return false;
        }
    }
}
