namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Tests.Infrastructure;
    using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class MatchMonitorUnitTests
    {
        private readonly IMatchMonitorDataAccess _dataAccess;
        private readonly IMatchMonitorServices _service;
        private readonly IAssociationDetectionService _associationDetectionService;

        public MatchMonitorUnitTests()
        {
            _dataAccess = Substitute.For<IMatchMonitorDataAccess>();
            _associationDetectionService = Substitute.For<IAssociationDetectionService>();
            _service = new MatchMonitorServices(_dataAccess, _associationDetectionService);
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

            var result = _service.GetMatchTrans(lastScannedSequenceID, batchSize, isLive, string.Empty);

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

            _dataAccess.InsertMatchMonitorStaging(ticketDetailToJson, isLive);

            Action action = () => _service.InsertMatchMonitorStaging(ticketDetailToJson, isLive);

            action.Should().NotThrow<Exception>();
        }

        #region Rule Arbitrage
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

            _dataAccess.GetMatchRuleArbitrage(isLive).Returns(output);

            var result = _service.GetMatchRuleArbitrage(isLive);

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

            _dataAccess.ProcessRuleArbitrage(isLive, maxSequenceID, input).Returns(output);

            var result = _service.ProcessRuleArbitrage(isLive, maxSequenceID, input);

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

            _dataAccess.CompleteTransRuleArbitrage(input);

            Action action = () => _service.CompleteTransRuleArbitrage(input);

            action.Should().NotThrow<Exception>();
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void CleanTransRuleArbitrage_WhenSuccess_ReturnNonException(bool isLive, UInt64 maxSequenceID)
        {
            _dataAccess.CleanTransRuleArbitrage(isLive, maxSequenceID);

            Action action = () => _service.CleanTransRuleArbitrage(isLive, maxSequenceID);

            action.Should().NotThrow<Exception>();
        }
        #endregion

        #region Rule Hedging
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleHedging_WhenSuccess(bool isLive)
        {
            // Arrange
            var output = new MatchMonitorRuleHedgingModel
            {
                MaxSequenceID = 123123,
                MatchStagingList = new List<MatchStagingRuleHedgingModel>
                {
                   new MatchStagingRuleHedgingModel { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "a", OldGroup = 1  },
                   new MatchStagingRuleHedgingModel { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "a", OldGroup = 2  },
                   new MatchStagingRuleHedgingModel { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "h", OldGroup = 1  }
                }
            };

            // Act
            _dataAccess.GetMatchRuleHedging(isLive).Returns(output);

            var result = _service.GetMatchRuleHedging(isLive);
            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleHedging_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.GetMatchRuleHedging(isLive).Throws<Exception>();

            Action action = () => _service.GetMatchRuleHedging(isLive);
            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRuleHedging_WhenSuccess(bool isLive)
        {
            // Arrange
            var processRuleHedgingMethod = TestExtension.GetPrivateMethod(typeof(MatchMonitorServices), MMConsts.PrivateMethodName.ProcessRuleHedging);

            // Act
            processRuleHedgingMethod.Invoke(_service, new object[] { isLive, new MatchStagingRuleHedgingModel(), "asd", "asd" });

            // Assert
            _dataAccess.Received(1).ProcessRuleHedging(isLive, Arg.Any<MatchStagingRuleHedgingModel>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CompleteTransRuleHedging_WhenSuccess()
        {
            // Arrange

            // Act
            _dataAccess.CompleteTransRuleHedging(Arg.Any<MatchCompleteRuleHedgingParameterModel>());

            _service.CompleteTransRuleHedging(new MatchCompleteRuleHedgingParameterModel());
            // Assert
            _dataAccess.Received(1).CompleteTransRuleHedging(Arg.Any<MatchCompleteRuleHedgingParameterModel>());
        }

        [Fact]
        public void CompleteTransRuleHedging_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(it => it.CompleteTransRuleHedging(Arg.Any<MatchCompleteRuleHedgingParameterModel>())).Throw<Exception>();

            Action action = () => _service.CompleteTransRuleHedging(new MatchCompleteRuleHedgingParameterModel());
            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleHedging_WhenSuccess(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.CleanTransRuleHedging(Arg.Any<bool>(), Arg.Any<UInt64>());

            _service.CleanTransRuleHedging(isLive, 123);
            // Assert
            _dataAccess.Received(1).CleanTransRuleHedging(Arg.Any<bool>(), Arg.Any<UInt64>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleHedging_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.When(it => it.CleanTransRuleHedging(Arg.Any<bool>(), Arg.Any<UInt64>())).Throw<Exception>();

            Action action = () => _service.CleanTransRuleHedging(isLive, 123);

            // Assert
            Assert.Throws<Exception>(action);
        }
        #endregion
    }
}