using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorHedging;
using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Entities;
using Fanex.NAP.SPU.CTS.Features.MatchMonitorHedging.Models;
using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.MMConsts;

namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    public class MatchMonitorRuleHedgingUnitTest
    {
        private readonly IAssociationDetectionService _assDetect;
        private readonly IMatchMonitorRuleHedgingDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMatchMonitorRuleProcessService<MatchStagingRuleHedgingGroupModel> _mmProcessService;
        private readonly IMatchMonitorRuleHedgingService _mmRuleService;

        public MatchMonitorRuleHedgingUnitTest()
        {
            _assDetect = Substitute.For<IAssociationDetectionService>();
            _dataAccess = Substitute.For<IMatchMonitorRuleHedgingDataAccess>();
            _serviceProvider = Substitute.For<IServiceProvider>();

            _mmProcessService = new MatchMonitorRuleHedgingProcessService(_dataAccess, _assDetect);
            _mmRuleService = new MatchMonitorRuleHedgingService(_dataAccess, _serviceProvider);
        }

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
                   new MatchStagingRuleHedgingEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4 },
                   new MatchStagingRuleHedgingEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4 },
                   new MatchStagingRuleHedgingEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4 }
                }
            };

            // Act
            _dataAccess.GetMatchRuleHedging(isLive).Returns(output);

            var result = _mmRuleService.GetMatchRuleHedging(isLive);
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

            Action action = () => _mmRuleService.GetMatchRuleHedging(isLive);
            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRuleHedging_WhenSuccess(bool isLive)
        {
            // Arrange
            var processRuleHedgingMethod = TestExtension.GetPrivateMethod(typeof(MatchMonitorRuleHedgingProcessService), PrivateMethodName.ProcessRuleHedging);

            // Act
            processRuleHedgingMethod.Invoke(_mmProcessService, new object[] { isLive, new MatchStagingRuleHedgingEntity(), "asd", "asd" });

            // Assert
            _dataAccess.Received(1).ProcessRuleHedging(isLive, Arg.Any<MatchStagingRuleHedgingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenSuccess(bool isLive)
        {
            // Arrange
            var matchStagingField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.MatchStaging);
            matchStagingField.SetValue(_mmProcessService, new MatchStagingRuleHedgingGroupModel
            {
                MatchGroupStagingList = new List<MatchStagingRuleHedgingEntity>() { new MatchStagingRuleHedgingEntity() }
            });

            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService, new List<MatchProcessRuleHedgingModel>());

            var numberOfThreads = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.NumberInternalOfThread);
            numberOfThreads.SetValue(_mmProcessService, 3);

            // Act
            _assDetect.DetectAssociation(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupEntity>() { new DetectAssociationMergeGroupEntity { GroupID = 1 } });
            _dataAccess.ProcessRuleHedging(isLive, Arg.Any<MatchStagingRuleHedgingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleHedgingModel());

            var result = _mmProcessService.ProcessRound1();
            // Assert
            _assDetect.Received(1).DetectAssociation(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleHedging(isLive, Arg.Any<MatchStagingRuleHedgingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenSuccess(bool isLive)
        {
            // Arrange
            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleHedgingModel> { new MatchProcessRuleHedgingModel {
                    CompleteMatches = new List<MatchCompleteRuleHedgingEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleHedgingInfoEntity> { new MatchProcessRuleHedgingInfoEntity() },
                    MatchInfo = new MatchStagingRuleHedgingEntity()
                } });

            var numberOfThreads = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.NumberInternalOfThread);
            numberOfThreads.SetValue(_mmProcessService, 3);

            // Act
            _assDetect.DetectAssociation(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupEntity>() { new DetectAssociationMergeGroupEntity { GroupID = 1 } });
            _dataAccess.ProcessRuleHedging(isLive, Arg.Any<MatchStagingRuleHedgingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleHedgingModel());

            var result = _mmProcessService.ProcessRound2();
            // Assert
            _assDetect.Received(1).DetectAssociation(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleHedging(isLive, Arg.Any<MatchStagingRuleHedgingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CompleteTransRuleHedging_WhenSuccess()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleHedgingModel> { new MatchProcessRuleHedgingModel {
                    CompleteMatches = new List<MatchCompleteRuleHedgingEntity>() { new MatchCompleteRuleHedgingEntity() },
                    ReprocessMatches = new List<MatchProcessRuleHedgingInfoEntity>(),
                    MatchInfo = new MatchStagingRuleHedgingEntity()
                } });

            // Act
            _dataAccess.CompleteTransRuleHedging(Arg.Any<MatchCompleteRuleHedgingParameterModel>());

            _mmProcessService.Complete(123123);

            // Assert
            _dataAccess.Received(1).CompleteTransRuleHedging(Arg.Any<MatchCompleteRuleHedgingParameterModel>());
        }

        [Fact]
        public void CompleteTransRuleHedging_ThrowException()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleHedgingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleHedgingModel> { new MatchProcessRuleHedgingModel {
                    CompleteMatches = new List<MatchCompleteRuleHedgingEntity>() { new MatchCompleteRuleHedgingEntity() },
                    ReprocessMatches = new List<MatchProcessRuleHedgingInfoEntity>(),
                    MatchInfo = new MatchStagingRuleHedgingEntity()
                } });

            // Act
            _dataAccess.When(x => x.CompleteTransRuleHedging(Arg.Any<MatchCompleteRuleHedgingParameterModel>())).Throw<Exception>();

            Action action = () => _mmProcessService.Complete(123123);

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

            _mmRuleService.CleanTransRuleHedging(isLive, 123);
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

            Action action = () => _mmRuleService.CleanTransRuleHedging(isLive, 123);

            // Assert
            Assert.Throws<Exception>(action);
        }
    }
}