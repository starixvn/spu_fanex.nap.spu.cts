namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models;
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
                MatchStagingList = new List<MatchStagingRuleHedgingEntity>
                {
                   new MatchStagingRuleHedgingEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "a", OldGroup = 1  },
                   new MatchStagingRuleHedgingEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "a", OldGroup = 2  },
                   new MatchStagingRuleHedgingEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "h", OldGroup = 1  }
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
            processRuleHedgingMethod.Invoke(_service, new object[] { isLive, new MatchStagingRuleHedgingEntity(), "asd", "asd" });

            // Assert
            _dataAccess.Received(1).ProcessRuleHedging(isLive, Arg.Any<MatchStagingRuleHedgingEntity>(), Arg.Any<string>(), Arg.Any<string>());
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