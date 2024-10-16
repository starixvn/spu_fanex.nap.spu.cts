namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Fanex.NAP.SPU.CTS.Shares;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging.Entities;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging.Models;

    public interface IMatchMonitorStagingService
    {
        IMatchMonitorStagingService Start(int batchSize, bool isLive);

        IMatchMonitorStagingService GetLastScannedSequenceID();

        IMatchMonitorStagingService GetMatchTrans();

        IMatchMonitorStagingService InsertMatchMonitorStagingWithParallel();

        void UpdateLastScannedSequenceID();
    }

    public class MatchMonitorStagingService : IMatchMonitorStagingService
    {
        private readonly IMatchMonitorStagingDataAccess _dataAccess;
        private readonly ISystemParameterServices _systemParameter;

        private int BatchSize;
        private bool IsLive;

        private long LastScannedSequenceID;
        private IEnumerable<TicketDetailEntity> TicketStagings;
        private SystemParameters LastScannedSequenceID_ParameterID;
        private Type PoolEnumType;

        public MatchMonitorStagingService(IMatchMonitorStagingDataAccess dataAccess, ISystemParameterServices systemParameter)
        {
            _dataAccess = dataAccess;
            _systemParameter = systemParameter;
        }

        public IMatchMonitorStagingService Start(int batchSize, bool isLive)
        {
            BatchSize = batchSize;
            IsLive = isLive;

            TicketStagings = Enumerable.Empty<TicketDetailEntity>();
            LastScannedSequenceID_ParameterID = IsLive ? SystemParameters.MatchMonitorLastScannedTrans : SystemParameters.MatchMonitorLastScannedTransNoneLive;
            PoolEnumType = IsLive ? typeof(MatchMonitorStagingLivePoolType) : typeof(MatchMonitorStagingNonLivePoolType);

            return this;
        }

        public IMatchMonitorStagingService GetLastScannedSequenceID()
        {
            LastScannedSequenceID = _systemParameter.GetLastScannedId(LastScannedSequenceID_ParameterID);

            return this;
        }

        public IMatchMonitorStagingService GetMatchTrans()
        {
            if (BatchSize > 0)
            {
                var betTypeInfos = GetBettypeSettings();

                TicketStagings = GetTicketStaging(LastScannedSequenceID, BatchSize, IsLive, betTypeInfos);
            }

            return this;
        }

        public IMatchMonitorStagingService InsertMatchMonitorStagingWithParallel()
        {
            if (TicketStagings.IsNotNullOrEmpty())
            {
                var ticketStagingJson = JsonConvert.SerializeObject(TicketStagings);

                Enum.GetValues(PoolEnumType)
                    .Cast<int>()
                    .AsParallel()
                    .ForAll(poolType => InsertMatchMonitorStaging(ticketStagingJson, IsLive, poolType));
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
                IsFilterMMParlayDetection = false,
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

        private IEnumerable<TicketDetailEntity> GetTicketStaging(long lastScannedSequenceID, int batchSize, bool isLive, string betTypeInfos)
        {
            return _dataAccess.GetMatchTrans(lastScannedSequenceID, batchSize, isLive, betTypeInfos);
        }

        private void InsertMatchMonitorStaging(string matchTransJson, bool isLive, int poolType)
        {
            _dataAccess.InsertMatchMonitorStaging(matchTransJson, isLive, poolType);
        }
    }
}
