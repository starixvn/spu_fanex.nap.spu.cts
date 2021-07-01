namespace Fanex.NAP.SPU.CTS.Features.CTSCustomer
{
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.Client.Services;
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
    }

    public class CTSCustomerService : ICTSCustomerService
    {
        private readonly ICTSCustomerDataAccess dataAccess;
        private readonly ISentryClient sentryClient;
        private readonly string serviceName = "CTSCustomerService";

        public CTSCustomerService(ICTSCustomerDataAccess dataAccess, ISentryClient sentryClient)
        {
            this.dataAccess = dataAccess;
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

        public void UpdateCustomerComplete(string customers, DateTime? lastUpdateTime, Int64? lastUpdateCustID, UPDATECUSTOMER sourceType)
        {
            if (sourceType == UPDATECUSTOMER.FROM_CUSTOMER)
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

        private bool InsertNewCustomer(List<CustomerMainDBModel> customers, long? lastCustID, bool isWriteLogInfo, int internalBatchSize)
        {
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
                                    var custToRescan = custIdList.Where(c => !c.IsExisting).Select(c => c.CustID).Distinct().ToList();
                                    //_customerClassificationService.RescanCustomerClassify(custToRescan, true, true, Constants.FLOWTYPERESCANCC.NEWMEMBER);

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
                                    //_dataCenterService.ReTransformAccount(JsonConvert.SerializeObject(custIdList), lastCustID);

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
