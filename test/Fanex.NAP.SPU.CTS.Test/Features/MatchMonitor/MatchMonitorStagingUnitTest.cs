using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorStaging.Entities;
using Fanex.NAP.SPU.CTS.Features.SystemParameter;
using Fanex.NAP.SPU.CTS.Tests.Infrastructure;
using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.Shares.Consts;

namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    public class MatchMonitorStagingUnitTest
    {
        private readonly IMatchMonitorStagingDataAccess _dataAccess;
        private readonly ISystemParameterServices _systemParameter;
        private readonly IMatchMonitorStagingService _service;

        public MatchMonitorStagingUnitTest()
        {
            _dataAccess = Substitute.For<IMatchMonitorStagingDataAccess>();
            _systemParameter = Substitute.For<ISystemParameterServices>();

            _service = new MatchMonitorStagingService(_dataAccess, _systemParameter);
        }

        [Theory]
        [InlineData(1, 10, true)]
        [InlineData(10, 100, true)]
        [InlineData(1, 10, false)]
        [InlineData(10, 100, false)]
        public void GetMatchTrans_WhenSuccess_ReturnMatch(long lastScannedSequenceID, int batchSize, bool isLive)
        {
            var output = new List<TicketDetailEntity>
            {
                new TicketDetailEntity
                {
                    CustId = 1,
                    AwayId = 1,
                    BetTeam = string.Empty,
                    BetType = 1,
                    Hdp1 = 1,
                    Hdp2 = 1,
                    MatchId = 1,
                    HomeId = 1,
                    LeagueId = 1,
                    LiveAwayScore = 1,
                    LiveHomeScore = 1,
                    LiveIndicator = true,
                    Odds = 1,
                    Stake = 1,
                    TransId = 1,
                    EventDate = DateTime.Parse("2021-07-05 22:50:55"),
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Parse("2021-07-05 22:50:55"),
                    TransDate = DateTime.Parse("2021-07-05 22:50:55")
                },
                new TicketDetailEntity
                {
                    CustId = 2,
                    AwayId = 2,
                    BetTeam = string.Empty,
                    BetType = 2,
                    Hdp1 = 2,
                    Hdp2 = 2,
                    MatchId = 2,
                    HomeId = 2,
                    LeagueId = 2,
                    LiveAwayScore = 2,
                    LiveHomeScore = 2,
                    LiveIndicator = true,
                    Odds = 2,
                    Stake = 2,
                    TransId = 2,
                    EventDate = DateTime.Parse("2021-07-05 22:50:55"),
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Parse("2021-07-05 22:50:55"),
                    TransDate = DateTime.Parse("2021-07-05 22:50:55")
                }
            };

            _dataAccess.GetMatchTrans(lastScannedSequenceID, batchSize, isLive, string.Empty).Returns(output);

            var getMatchTrans = TestExtension.GetPrivateMethod(typeof(MatchMonitorStagingService), MMConsts.PrivateMethodName.GetTicketStaging);

            var result = getMatchTrans.Invoke(_service, new object[] { lastScannedSequenceID, batchSize, isLive, "" });

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(output);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void InsertMatchMonitorStaging_WhenSuccess_ReturnNonException(bool isLive)
        {
            var ticketDetails = new List<TicketDetailEntity>
            {
                new TicketDetailEntity
                {
                    CustId = 1,
                    AwayId = 1,
                    BetTeam = string.Empty,
                    BetType = 1,
                    Hdp1 = 1,
                    Hdp2 = 1,
                    MatchId = 1,
                    HomeId = 1,
                    LeagueId = 1,
                    LiveAwayScore = 1,
                    LiveHomeScore = 1,
                    LiveIndicator = true,
                    Odds = 1,
                    Stake = 1,
                    TransId = 1,
                    EventDate = DateTime.Parse("2021-07-05 22:50:55"),
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Parse("2021-07-05 22:50:55"),
                    TransDate = DateTime.Parse("2021-07-05 22:50:55")
                },
                new TicketDetailEntity
                {
                    CustId = 2,
                    AwayId = 2,
                    BetTeam = string.Empty,
                    BetType = 2,
                    Hdp1 = 2,
                    Hdp2 = 2,
                    MatchId = 2,
                    HomeId = 2,
                    LeagueId = 2,
                    LiveAwayScore = 2,
                    LiveHomeScore = 2,
                    LiveIndicator = true,
                    Odds = 2,
                    Stake = 2,
                    TransId = 2,
                    EventDate = DateTime.Parse("2021-07-05 22:50:55"),
                    EventStatus = string.Empty,
                    KickOffTime = DateTime.Parse("2021-07-05 22:50:55"),
                    TransDate = DateTime.Parse("2021-07-05 22:50:55")
                }
            };

            var ticketDetailToJson = JsonConvert.SerializeObject(ticketDetails);

            _dataAccess.InsertMatchMonitorStaging(ticketDetailToJson, isLive, Arg.Any<int>());

            var insertMatchMonitorStaging = TestExtension.GetPrivateMethod(typeof(MatchMonitorStagingService), MMConsts.PrivateMethodName.InsertMatchMonitorStaging);

            Action action = () => insertMatchMonitorStaging.Invoke(_service, new object[] { ticketDetailToJson, isLive, MatchMonitorStagingLivePoolType.MatchMonitorStagingFixedGameLive });
            
            action.Should().NotThrow<Exception>();
        }
    }
}
