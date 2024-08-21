namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging
{
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IMMParlayStagingService
    {
        IMMParlayStagingService Start(int batchSize, bool isLive);

        IMMParlayStagingService GetLastScannedSequenceID();

        IMMParlayStagingService GetParlayTicketStaging();

        IMMParlayStagingService InsertParlayTicketStaging();

        void UpdateLastScannedSequenceID();
    }

    public class MMParlayStagingService : IMMParlayStagingService
    {
        private readonly IMMParlayStagingDataAccess _dataAccess;
        private readonly ISystemParameterServices _systemParameter;

        private int BatchSize;
        private bool IsLive;

        private long LastScannedSequenceID;
        private IEnumerable<ParlayTicketDetailEntity> TicketStagings;
        private SystemParameters LastScannedSequenceID_ParameterID;
        private Type PoolEnumType;

        public MMParlayStagingService(IMMParlayStagingDataAccess dataAccess, ISystemParameterServices systemParameter)
        {
            _dataAccess = dataAccess;
            _systemParameter = systemParameter;
        }

        public IMMParlayStagingService Start(int batchSize, bool isLive)
        {
            BatchSize = batchSize;
            IsLive = isLive;
            TicketStagings = Enumerable.Empty<ParlayTicketDetailEntity>();
            LastScannedSequenceID_ParameterID = IsLive ? SystemParameters.MatchMonitorParlayLastScannedTrans : SystemParameters.MatchMonitorParlayLastScannedTransNoneLive;
            PoolEnumType = IsLive ? typeof(MatchMonitorParlayStagingLivePoolType) : typeof(MatchMonitorStagingParlayNonLivePoolType);

            return this;
        }

        public IMMParlayStagingService GetLastScannedSequenceID()
        {
            LastScannedSequenceID = _systemParameter.GetLastScannedId(LastScannedSequenceID_ParameterID);

            return this;
        }

        public IMMParlayStagingService GetParlayTicketStaging()
        {
            if (BatchSize > 0)
            {
                var betTypeInfos = GetBettypeSettings();

                TicketStagings = GetParlayTickets(LastScannedSequenceID, BatchSize, IsLive, betTypeInfos);
            }

            return this;
        }

        public void UpdateLastScannedSequenceID()
        {
            if (TicketStagings.IsNotNullOrEmpty())
            {
                var maxSequenceID = TicketStagings.Max(x => x.SequenceID).ToString();

                _systemParameter.UpdateValueForSystemParamById(maxSequenceID, LastScannedSequenceID_ParameterID);

            }
        }

        private string GetBettypeSettings()
        {
            var paras = new BetTypeSettingParaModel
            {
                FunctionID = (int)BetTypeSettingsFunctionID.MMBetType,
                BettypeIDList = null,
                SportTypeIDList = null,
                IsFilterMMDetection = true,
                IsShowDetailsX = false,
                Status = 1
            };

            var bettypeList = _systemParameter
                                    .GetBetTypeSettings(paras)
                                    .Select(x => new
                                    {
                                        SportTypeID = x.SportTypeID,
                                        BetTypeID = x.BetTypeID,
                                        BetChoiceType = x.BetChoiceType,
                                        BetIDPattern = x.BetIDPattern
                                    })
                                    .Distinct();

            return JsonConvert.SerializeObject(bettypeList);
        }

        public IMMParlayStagingService InsertParlayTicketStaging()
        {
            if (TicketStagings.IsNotNullOrEmpty())
            {
                var jsonTickets = JsonConvert.SerializeObject(TicketStagings);

                Enum.GetValues(PoolEnumType)
                    .Cast<int>()
                    .AsParallel()
                    .ForAll(poolType => InsertParlayTickets(jsonTickets, IsLive, poolType));
            }

            return this;
        }

        private IEnumerable<ParlayTicketDetailEntity> GetParlayTickets(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos)
        {
            return _dataAccess.GetParlayTickets(lastScannedSequenceID, batchSize, isLive, betTypeInfos);
        }

        private void InsertParlayTickets(string jsonTickets, bool isLive, int poolType)
        {
            _dataAccess.InsertParlayTickets(jsonTickets, isLive, poolType);
        }
    }
}
