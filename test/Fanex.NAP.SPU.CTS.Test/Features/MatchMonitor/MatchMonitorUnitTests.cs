namespace Fanex.NAP.SPU.CTS.Tests.Features.Monitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public class MatchMonitorUnitTests
    {
        private readonly IMatchMonitorDataAccess matchMonitorDataAccess;
        private readonly IMatchMonitorServices matchMonitorServices;

        private readonly ISystemParameterServices commonServices;
        private readonly ISystemParameterDataAccess commonDataAccess;

        public MatchMonitorUnitTests()
        {
            matchMonitorDataAccess = Substitute.For<IMatchMonitorDataAccess>();
            matchMonitorServices = new MatchMonitorServices(matchMonitorDataAccess);

            commonDataAccess = Substitute.For<ISystemParameterDataAccess>();
            commonServices = new SystemParameterServices(commonDataAccess);
        }

        [Fact]
        public void GetLastScannedTransID_WhenSuccess_ReturnTransID()
        {
            var result = commonServices.GetLastScannedId(SystemParameters.MatchMonitorLastScannedTrans);

            result.Should().NotBe(null);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(10, 100)]
        public void GetMatchTrans_WhenSuccess_ReturnMat(Int64 LastScannedTransID, int batchSize)
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
                    TicketStatus = 1,
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
                    TicketStatus = 2,
                    TransDate = DateTime.Parse("2021-07-05 22:50:55")
                }
            };

            matchMonitorDataAccess.GetMatchTrans(LastScannedTransID, batchSize).Returns(output);

            var result = matchMonitorServices.GetMatchTrans(LastScannedTransID, batchSize);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(output);
        }

        [Fact]
        public void InsertMatchMonitorStaging_WhenSuccess_ReturnGroupIds()
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
                    TicketStatus = 1,
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
                    TicketStatus = 2,
                    TransDate = DateTime.Parse("2021-07-05 22:50:55")
                }
            };

            var groupIds = new List<int> { 1, 2, 3 };

            var ticketDetailToJson = JsonConvert.SerializeObject(ticketDetails);

            matchMonitorDataAccess.InsertMatchMonitorStaging(ticketDetailToJson).Returns(groupIds);

            var result = matchMonitorServices.InsertMatchMonitorStaging(ticketDetailToJson);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(groupIds);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void InsertMatchMonitorRule_WhenSuccess_ReturnNotException(int tempGroupId)
        {
            matchMonitorDataAccess.InsertMatchMonitorRule(tempGroupId);

            Action action = () => matchMonitorServices.InsertMatchMonitorRule(tempGroupId);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void UpdateMatchTransComplete_WhenSuccess_ReturnNotException()
        {
            matchMonitorDataAccess.UpdateMatchTransComplete();

            Action action = () => matchMonitorServices.UpdateMatchTransComplete();

            action.Should().NotThrow<Exception>();
        }
    }
}