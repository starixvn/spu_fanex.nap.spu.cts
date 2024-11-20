namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor.Parlay
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayStaging;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.Tests.Infrastructure;
    using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public class MMParlayStagingServiceTest
    {
        private readonly IMMParlayStagingDataAccess _dataAccess;
        private readonly ISystemParameterServices _systemParameter;
        private readonly IMMParlayStagingService _service;

        public MMParlayStagingServiceTest()
        {
            _dataAccess = Substitute.For<IMMParlayStagingDataAccess>();
            _systemParameter = Substitute.For<ISystemParameterServices>();

            _service = new MMParlayStagingService(_dataAccess, _systemParameter);
        }

        [Theory]
        [InlineData(1, 10, true)]     
        [InlineData(10, 100, false)]
        public void GetMatchTrans_WhenSuccess_ReturnMatch(long lastScannedSequenceID, int batchSize, bool isLive)
        {
            var expectedData = new List<ParlayTicketDetailEntity>
            {
                new ParlayTicketDetailEntity
                {
                    SequenceID = 1,
                    TransID = 1,                   
                    Refno = 1,
                    BetTypeID = 1,
                    BetID = 0,
                    CustID = 1,
                    HomeID = 1,
                    LeagueID = 1,
                    LiveAwayScore = 1,
                    LiveHomeScore = 1,                                   
                    EventDate = DateTime.Now,
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Now,
                    TransDate = DateTime.Now,
                },
                new ParlayTicketDetailEntity
                {
                    SequenceID = 2,
                    TransID = 2,
                    Refno = 2,
                    BetTypeID = 2,
                    BetID = 0,
                    CustID = 2,
                    HomeID = 2,
                    LeagueID = 2,
                    LiveAwayScore = 2,
                    LiveHomeScore = 2,
                    EventDate = DateTime.Now,
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Now,
                    TransDate = DateTime.Now,
                }
            };

            _dataAccess.GetParlayTickets(lastScannedSequenceID, batchSize, isLive, string.Empty).Returns((1, expectedData));

            var getMatchTrans = TestExtension.GetPrivateMethod(typeof(MMParlayStagingService), "GetParlayTickets");

            var result = getMatchTrans.Invoke(_service, new object[] { lastScannedSequenceID, batchSize, isLive, "" });

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo((1, expectedData));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void InsertMatchMonitorStaging_WhenSuccess_ReturnNonException(bool isLive)
        {
            var expectedData = new List<ParlayTicketDetailEntity>
            {
                new ParlayTicketDetailEntity
                {
                    SequenceID = 1,
                    TransID = 1,
                    Refno = 1,
                    BetTypeID = 1,
                    BetID = 0,
                    CustID = 1,
                    HomeID = 1,
                    LeagueID = 1,
                    LiveAwayScore = 1,
                    LiveHomeScore = 1,
                    EventDate = DateTime.Now,
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Now,
                    TransDate = DateTime.Now,
                },
                new ParlayTicketDetailEntity
                {
                    SequenceID = 2,
                    TransID = 2,
                    Refno = 2,
                    BetTypeID = 2,
                    BetID = 0,
                    CustID = 2,
                    HomeID = 2,
                    LeagueID = 2,
                    LiveAwayScore = 2,
                    LiveHomeScore = 2,
                    EventDate = DateTime.Now,
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Now,
                    TransDate = DateTime.Now,
                }
            };

            var ticketDetailToJson = JsonConvert.SerializeObject(expectedData);

            _dataAccess.InsertParlayTickets(ticketDetailToJson, isLive, Arg.Any<int>());

            var insertParlayTickets = TestExtension.GetPrivateMethod(typeof(MMParlayStagingService), "InsertParlayTickets");

            Action action = () => insertParlayTickets.Invoke(_service, new object[] { ticketDetailToJson, isLive, MatchMonitorStagingParlayNonLivePoolType.MatchMonitorStagingGroupBettingNonLive });

            action.Should().NotThrow<Exception>();
        }
    }
}
