using System;
using NSubstitute;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Models;
using Xunit;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorArbitrage.Entities;
using System.Collections.Generic;
using NSubstitute.ExceptionExtensions;
using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.MMConsts;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;

namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    public class MatchMonitorRuleArbitrageUnitTest
    {
        private readonly IAssociationDetectionService _assDetect;
        private readonly IMatchMonitorRuleArbitrageDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMatchMonitorRuleProcessService<MatchStagingRuleArbitrageGroupModel> _mmProcessService;
        private readonly IMatchMonitorRuleArbitrageService _mmRuleService;

        public MatchMonitorRuleArbitrageUnitTest()
        {
            _assDetect = Substitute.For<IAssociationDetectionService>();
            _dataAccess = Substitute.For<IMatchMonitorRuleArbitrageDataAccess>();
            _serviceProvider = Substitute.For<IServiceProvider>();

            _mmProcessService = new MatchMonitorRuleArbitrageProcessService(_assDetect, _dataAccess);
            _mmRuleService = new MatchMonitorRuleArbitrageService(_dataAccess, _serviceProvider);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleArbitrage_WhenSuccess(bool isLive)
        {
            // Arrange
            var output = new MatchMonitorRuleArbitrageModel
            {
                MaxSequenceID = 123123,
                MatchStagingList = new List<MatchStagingRuleArbitrageEntity>
                {
                   new MatchStagingRuleArbitrageEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4 },
                   new MatchStagingRuleArbitrageEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4 },
                   new MatchStagingRuleArbitrageEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4 }
                }
            };

            // Act
            _dataAccess.GetMatchRuleArbitrage(isLive).Returns(output);

            var result = _mmRuleService.GetMatchRuleArbitrage(isLive);
            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleArbitrage_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.GetMatchRuleArbitrage(isLive).Throws<Exception>();

            Action action = () => _mmRuleService.GetMatchRuleArbitrage(isLive);
            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRuleArbitrage_WhenSuccess(bool isLive)
        {
            // Arrange
            var processRuleArbitrageMethod = TestExtension.GetPrivateMethod(typeof(MatchMonitorRuleArbitrageProcessService), PrivateMethodName.ProcessRuleArbitrage);

            // Act
            processRuleArbitrageMethod.Invoke(_mmProcessService, new object[] { isLive, new MatchStagingRuleArbitrageEntity(), "asd", "asd" });

            // Assert
            _dataAccess.Received(1).ProcessRuleArbitrage(isLive, Arg.Any<MatchStagingRuleArbitrageEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenSuccess(bool isLive)
        {
            // Arrange
            var matchStagingField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleArbitrageProcessService), PrivateFieldName.MatchStaging);
            matchStagingField.SetValue(_mmProcessService, new MatchStagingRuleArbitrageGroupModel
            {
                MatchGroupStagingList = new List<MatchStagingRuleArbitrageEntity>() { new MatchStagingRuleArbitrageEntity() }
            });

            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleArbitrageProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleArbitrageProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService, new List<MatchProcessRuleArbitrageModel>());

            // Act
            _assDetect.DetectAssociation(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupModel>() { new DetectAssociationMergeGroupModel { GroupID = 1 } });
            _dataAccess.ProcessRuleArbitrage(isLive, Arg.Any<MatchStagingRuleArbitrageEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleArbitrageModel());

            var result = _mmProcessService.ProcessRound1();
            // Assert
            _assDetect.Received(1).DetectAssociation(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleArbitrage(isLive, Arg.Any<MatchStagingRuleArbitrageEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenSuccess(bool isLive)
        {
            // Arrange
            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleArbitrageProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleArbitrageProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleArbitrageModel> { new MatchProcessRuleArbitrageModel {
                    CompleteMatches = new List<MatchCompleteRuleArbitrageEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleArbitrageInfoEntity> { new MatchProcessRuleArbitrageInfoEntity() },
                    MatchInfo = new MatchStagingRuleArbitrageEntity()
                } });

            // Act
            _assDetect.DetectAssociation(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupModel>() { new DetectAssociationMergeGroupModel { GroupID = 1 } });
            _dataAccess.ProcessRuleArbitrage(isLive, Arg.Any<MatchStagingRuleArbitrageEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleArbitrageModel());

            var result = _mmProcessService.ProcessRound2();
            // Assert
            _assDetect.Received(1).DetectAssociation(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleArbitrage(isLive, Arg.Any<MatchStagingRuleArbitrageEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CompleteTransRuleArbitrage_WhenSuccess()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleArbitrageProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleArbitrageModel> { new MatchProcessRuleArbitrageModel {
                    CompleteMatches = new List<MatchCompleteRuleArbitrageEntity>() { new MatchCompleteRuleArbitrageEntity() },
                    ReprocessMatches = new List<MatchProcessRuleArbitrageInfoEntity>(),
                    MatchInfo = new MatchStagingRuleArbitrageEntity()
                } });

            // Act
            _dataAccess.CompleteTransRuleArbitrage(Arg.Any<MatchCompleteRuleArbitrageParameterModel>());

            _mmProcessService.Complete(123123);

            // Assert
            _dataAccess.Received(1).CompleteTransRuleArbitrage(Arg.Any<MatchCompleteRuleArbitrageParameterModel>());
        }

        [Fact]
        public void CompleteTransRuleArbitrage_ThrowException()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleArbitrageProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleArbitrageModel> { new MatchProcessRuleArbitrageModel {
                    CompleteMatches = new List<MatchCompleteRuleArbitrageEntity>() { new MatchCompleteRuleArbitrageEntity() },
                    ReprocessMatches = new List<MatchProcessRuleArbitrageInfoEntity>(),
                    MatchInfo = new MatchStagingRuleArbitrageEntity()
                } });

            // Act
            _dataAccess.When(x => x.CompleteTransRuleArbitrage(Arg.Any<MatchCompleteRuleArbitrageParameterModel>())).Throw<Exception>();

            Action action = () => _mmProcessService.Complete(123123);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleArbitrage_WhenSuccess(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.CleanTransRuleArbitrage(Arg.Any<bool>(), Arg.Any<UInt64>());

            _mmRuleService.CleanTransRuleArbitrage(isLive, 123);
            // Assert
            _dataAccess.Received(1).CleanTransRuleArbitrage(Arg.Any<bool>(), Arg.Any<UInt64>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleArbitrage_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.When(it => it.CleanTransRuleArbitrage(Arg.Any<bool>(), Arg.Any<UInt64>())).Throw<Exception>();

            Action action = () => _mmRuleService.CleanTransRuleArbitrage(isLive, 123);

            // Assert
            Assert.Throws<Exception>(action);
        }
    }
}
