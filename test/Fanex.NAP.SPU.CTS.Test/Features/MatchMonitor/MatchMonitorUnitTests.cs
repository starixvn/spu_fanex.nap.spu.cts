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
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public class MatchMonitorUnitTests
    {
        private readonly IMatchMonitorDataAccess matchMonitorDataAccess;
        private readonly IMatchMonitorServices matchMonitorServices;

        private readonly ISystemParameterServices commonServices;
        private readonly SystemParameterDataAccess commonDataAccess;

        public MatchMonitorUnitTests()
        {
            matchMonitorDataAccess = Substitute.For<IMatchMonitorDataAccess>();
            matchMonitorServices = new MatchMonitorServices(matchMonitorDataAccess);

            commonDataAccess = Substitute.For<SystemParameterDataAccess>();
            commonServices = new SystemParameterServices(commonDataAccess);
        }

        [Fact]
        public void GetLastScannedTransID_WhenSuccess_ReturnTransID()
        {
            var result = commonServices.GetLastScannedId(SystemParameters.MatchMonitorLastScannedTrans);

            result.Should().NotBe(null);
        }

        [Theory]
        [InlineData(1, 10, true)]
        [InlineData(10, 100, true)]
        [InlineData(1, 10, false)]
        [InlineData(10, 100, false)]
        public void GetMatchTrans_WhenSuccess_ReturnMat(Int64 LastScannedTransID, int batchSize, bool isLive)
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

            matchMonitorDataAccess.GetMatchTrans(LastScannedTransID, batchSize, isLive).Returns(output);

            var result = matchMonitorServices.GetMatchTrans(LastScannedTransID, batchSize, isLive);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(output);
        }

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void InsertMatchMonitorStaging_WhenSuccess_ReturnListMatchStaging(bool isLive)
        //{
        //    var ticketDetails = new List<TicketDetailEntity>
        //    {
        //        new TicketDetailEntity
        //        {
        //            CustId = 1,
        //            AwayId = 1,
        //            BetTeam = string.Empty,
        //            BetType = 1,
        //            Hdp1 = 1,
        //            Hdp2 = 1,
        //            MatchId = 1,
        //            HomeId = 1,
        //            LeagueId = 1,
        //            LiveAwayScore = 1,
        //            LiveHomeScore = 1,
        //            LiveIndicator = true,
        //            Odds = 1,
        //            Stake = 1,
        //            TransId = 1,
        //            EventDate = DateTime.Parse("2021-07-05 22:50:55"),
        //            EventStatus = string.Empty,
        //            KickOffTime = DateTime.Parse("2021-07-05 22:50:55"),
        //            TicketStatus = 1,
        //            TransDate = DateTime.Parse("2021-07-05 22:50:55")
        //        },
        //        new TicketDetailEntity
        //        {
        //            CustId = 2,
        //            AwayId = 2,
        //            BetTeam = string.Empty,
        //            BetType = 2,
        //            Hdp1 = 2,
        //            Hdp2 = 2,
        //            MatchId = 2,
        //            HomeId = 2,
        //            LeagueId = 2,
        //            LiveAwayScore = 2,
        //            LiveHomeScore = 2,
        //            LiveIndicator = true,
        //            Odds = 2,
        //            Stake = 2,
        //            TransId = 2,
        //            EventDate = DateTime.Parse("2021-07-05 22:50:55"),
        //            EventStatus = string.Empty,
        //            KickOffTime = DateTime.Parse("2021-07-05 22:50:55"),
        //            TicketStatus = 2,
        //            TransDate = DateTime.Parse("2021-07-05 22:50:55")
        //        }
        //    };

        //    var matchStagings = new List<MatchStagingModel> { 
        //        new MatchStagingModel{ 
        //            MatchId = 1,
        //            BetTypeId = 1,
        //            TotalScore = 3
        //        },
        //        new MatchStagingModel{
        //            MatchId = 2,
        //            BetTypeId = 3,
        //            TotalScore = 2
        //        },
        //        new MatchStagingModel{
        //            MatchId = 3,
        //            BetTypeId = 7,
        //            TotalScore = 0
        //        },
        //    };

        //    var ticketDetailToJson = JsonConvert.SerializeObject(ticketDetails);

        //    matchMonitorDataAccess.InsertMatchMonitorStaging(ticketDetailToJson, isLive).Returns(matchStagings);

        //    var result = matchMonitorServices.InsertMatchMonitorStaging(ticketDetailToJson, isLive);

        //    result.Should().NotBeNull();
        //    result.Should().BeEquivalentTo(matchStagings);
        //}

        //[Theory]
        //[InlineData(true, 1, 2, 1)]
        //[InlineData(true, 1, 0, 3)]
        //[InlineData(true, 1, 3, 7)]
        //[InlineData(true, 1, 3, 8)]
        //[InlineData(false, 1, 2, 1)]
        //[InlineData(false, 1, 0, 3)]
        //[InlineData(false, 1, 3, 7)]
        //[InlineData(false, 1, 3, 8)]
        //public void InsertMatchMonitorRule_WhenSuccess_ReturnNotException(bool isLive, int matchId, int totalScore, int bettypeId)
        //{
        //    var result = new List<MatchRuleCompletedModel> {
        //        new MatchRuleCompletedModel
        //        {
        //            GroupId = 1,
        //            IsHedging = false,
        //            CustIdList = "17990615,50294551,50294551",
        //            TransIdList = "111845019100,111845032053,111845032772",
        //            OldGroupIdList = "0",
        //            NoOfOldGroupIdList = 0
        //        }
        //    };

        //    matchMonitorDataAccess.InsertMatchMonitorRule(isLive, matchId, totalScore, bettypeId).Returns(result);

            
        //}

        //[Fact]
        //public void UpdateMatchTransComplete_WhenSuccess_ReturnNotException()
        //{
        //    matchMonitorDataAccess.UpdateMatchTransComplete();

        //    Action action = () => matchMonitorServices.UpdateMatchTransComplete();

        //    action.Should().NotThrow<Exception>();
        //}
    }
}