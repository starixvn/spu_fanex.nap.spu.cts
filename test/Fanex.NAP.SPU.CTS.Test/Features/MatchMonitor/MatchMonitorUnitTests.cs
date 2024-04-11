namespace Fanex.NAP.SPU.CTS.Tests.Features.Monitor
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class MatchMonitorUnitTests
    {
        private readonly IMatchMonitorDataAccess matchMonitorDataAccess;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly IAssociationDetectionService associationDetectionService;

        public MatchMonitorUnitTests()
        {
            matchMonitorDataAccess = Substitute.For<IMatchMonitorDataAccess>();
            associationDetectionService = Substitute.For<IAssociationDetectionService>();
            matchMonitorServices = new MatchMonitorServices(matchMonitorDataAccess, associationDetectionService);
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

            matchMonitorDataAccess.GetMatchTrans(lastScannedSequenceID, batchSize, isLive, string.Empty).Returns(output);

            var result = matchMonitorServices.GetMatchTrans(lastScannedSequenceID, batchSize, isLive, string.Empty);

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

            matchMonitorDataAccess.InsertMatchMonitorStaging(ticketDetailToJson, isLive);

            Action action = () => matchMonitorServices.InsertMatchMonitorStaging(ticketDetailToJson, isLive);

            action.Should().NotThrow<Exception>();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetMatchRuleArbitrage_WhenSuccess_ReturnMatches(bool isLive)
        {
            var output = new MatchMonitorRuleArbitrageModel
            {
                MaxSequenceID = 1,
                MatchStagingList = new List<MatchStagingRuleArbitrageModel>
                {
                    new MatchStagingRuleArbitrageModel
                    {
                        MatchId = 1,
                        IsMajorLeague = true,
                        BetTypeID = 1,
                        BetID = 1,
                        ScoreDiff = -1,
                        HDP = 1.00m
                    }
                }
            };

            matchMonitorDataAccess.GetMatchRuleArbitrage(isLive).Returns(output);

            var result = matchMonitorServices.GetMatchRuleArbitrage(isLive);

            result.Should().BeEquivalentTo(output);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void ProcessRuleArbitrage_WhenSuccess_ReturnMatchDetectionResults(bool isLive, UInt64 maxSequenceID)
        {
            var input = new MatchStagingRuleArbitrageModel
            {
                MatchId = 1,
                IsMajorLeague = true,
                BetTypeID = 1,
                BetID = 1,
                ScoreDiff = -1,
                HDP = 1.00m
            };

            var output = new List<MatchCompleteRuleArbitrageModel>
            {
                new MatchCompleteRuleArbitrageModel
                {
                    CustIdList = "1,2,3",
                    TransIDList = "111,222",
                    GroupId = 1,
                    Reason = 1,
                    OldGroupIdList = "1",
                    HighStakeLicCustList = "1"
                }
            };

            matchMonitorDataAccess.ProcessRuleArbitrage(isLive, maxSequenceID, input).Returns(output);

            var result = matchMonitorServices.ProcessRuleArbitrage(isLive, maxSequenceID, input);

            result.Should().BeEquivalentTo(output);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void CompleteTransRuleArbitrage_WhenSuccess_ReturnNonException(bool isLive, UInt64 maxSequenceID)
        {
            var matchInfo = new MatchStagingRuleArbitrageModel
            {
                MatchId = 1,
                IsMajorLeague = true,
                BetTypeID = 1,
                BetID = 1,
                ScoreDiff = -1,
                HDP = 1.00m
            };

            var input = new MatchCompleteRuleArbitrageParameterModel
            {
                IsLive = isLive,
                MatchInfo = matchInfo,
                TransGroupJson = JsonConvert.SerializeObject(""),
                MaxSequenceID = maxSequenceID
            };

            matchMonitorDataAccess.CompleteTransRuleArbitrage(input);

            Action action = () => matchMonitorServices.CompleteTransRuleArbitrage(input);

            action.Should().NotThrow<Exception>();
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void CleanTransRuleArbitrage_WhenSuccess_ReturnNonException(bool isLive, UInt64 maxSequenceID)
        {
            matchMonitorDataAccess.CleanTransRuleArbitrage(isLive, maxSequenceID);

            Action action = () => matchMonitorServices.CleanTransRuleArbitrage(isLive, maxSequenceID);

            action.Should().NotThrow<Exception>();
        }
    }
}