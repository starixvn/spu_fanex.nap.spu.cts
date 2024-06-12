using System;
using Xunit;
using NSubstitute;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Models;
using System.Collections.Generic;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleGroupBetting.Entities;
using NSubstitute.ExceptionExtensions;
using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.MMConsts;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;

namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    public class MatchMonitorRuleGroupBettingUnitTest
    {
        private readonly IAssociationDetectionService _assDetect;
        private readonly IMatchMonitorRuleGroupBettingDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMatchMonitorRuleProcessService<MatchStagingRuleGroupBettingGroupModel> _mmProcessService;
        private readonly IMatchMonitorRuleGroupBettingService _mmRuleService;

        public MatchMonitorRuleGroupBettingUnitTest()
        {
            _assDetect = Substitute.For<IAssociationDetectionService>();
            _dataAccess = Substitute.For<IMatchMonitorRuleGroupBettingDataAccess>();
            _serviceProvider = Substitute.For<IServiceProvider>();

            _mmProcessService = new MatchMonitorRuleGroupBettingProcessService(_assDetect, _dataAccess);
            _mmRuleService = new MatchMonitorRuleGroupBettingService(_dataAccess, _serviceProvider);
        }

        #region Rule GroupBetting
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleGroupBetting_WhenSuccess(bool isLive)
        {
            // Arrange
            var output = new MatchMonitorRuleGroupBettingModel
            {
                MaxSequenceID = 123123,
                MatchStagingList = new List<MatchStagingRuleGroupBettingEntity>
                {
                   new MatchStagingRuleGroupBettingEntity { MatchID = 1, ScoreDiff = 2, BettypeID = 3, BetID = 4, Betteam = "a"  },
                   new MatchStagingRuleGroupBettingEntity { MatchID = 1, ScoreDiff = 2, BettypeID = 3, BetID = 4, Betteam = "a"  },
                   new MatchStagingRuleGroupBettingEntity { MatchID = 1, ScoreDiff = 2, BettypeID = 3, BetID = 4, Betteam = "h"  }
                }
            };

            // Act
            _dataAccess.GetMatchRuleGroupBetting(isLive).Returns(output);

            var result = _mmRuleService.GetMatchRuleGroupBetting(isLive);
            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleGroupBetting_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.GetMatchRuleGroupBetting(isLive).Throws<Exception>();

            Action action = () => _mmRuleService.GetMatchRuleGroupBetting(isLive);
            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRuleGroupBetting_WhenSuccess(bool isLive)
        {
            // Arrange
            var processRuleSabaGBMethod = TestExtension.GetPrivateMethod(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateMethodName.ProcessRuleGroupBetting);

            // Act
            processRuleSabaGBMethod.Invoke(_mmProcessService, new object[] { isLive, new MatchStagingRuleGroupBettingEntity(), "asd", "asd" });

            // Assert
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenSuccess(bool isLive)
        {
            // Arrange
            var matchStagingField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchStaging);
            matchStagingField.SetValue(_mmProcessService, new MatchStagingRuleGroupBettingGroupModel
            {
                MatchGroupStagingList = new List<MatchStagingRuleGroupBettingEntity>() { new MatchStagingRuleGroupBettingEntity() }
            });

            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService, new List<MatchProcessRuleGroupBettingModel>());

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound1();
            // Assert
            _assDetect.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenSuccess(bool isLive)
        {
            // Arrange
            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleGroupBettingModel> { new MatchProcessRuleGroupBettingModel {
                    CompleteMatches = new List<MatchCompleteRuleGroupBettingEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleGroupBettingInfoEntity> { new MatchProcessRuleGroupBettingInfoEntity() },
                    MatchInfo = new MatchStagingRuleGroupBettingEntity()
                } });

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound2();
            // Assert
            _assDetect.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CompleteTransRuleSabaGB_WhenSuccess()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleGroupBettingModel> { new MatchProcessRuleGroupBettingModel {
                    CompleteMatches = new List<MatchCompleteRuleGroupBettingEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleGroupBettingInfoEntity>(),
                    MatchInfo = new MatchStagingRuleGroupBettingEntity()
                } });

            // Act
            _dataAccess.CompleteTransRuleGroupBetting(Arg.Any<MatchCompleteRuleGroupBettingParameterModel>());

            _mmProcessService.Complete(123123);

            // Assert
            _dataAccess.Received(1).CompleteTransRuleGroupBetting(Arg.Any<MatchCompleteRuleGroupBettingParameterModel>());
        }

        [Fact]
        public void CompleteTransRuleSabaGB_ThrowException()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                 new List<MatchProcessRuleGroupBettingModel> { new MatchProcessRuleGroupBettingModel {
                    CompleteMatches = new List<MatchCompleteRuleGroupBettingEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleGroupBettingInfoEntity>(),
                    MatchInfo = new MatchStagingRuleGroupBettingEntity()
                } });

            // Act
            _dataAccess.When(x => x.CompleteTransRuleGroupBetting(Arg.Any<MatchCompleteRuleGroupBettingParameterModel>())).Throw<Exception>();

            Action action = () => _mmProcessService.Complete(123123);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleSabaGB_WhenSuccess(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.CleanTransRuleGroupBetting(Arg.Any<bool>(), Arg.Any<UInt64>());

            _mmRuleService.CleanTransRuleGroupBetting(isLive, 123);
            // Assert
            _dataAccess.Received(1).CleanTransRuleGroupBetting(Arg.Any<bool>(), Arg.Any<UInt64>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleSabaGB_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.When(it => it.CleanTransRuleGroupBetting(Arg.Any<bool>(), Arg.Any<UInt64>())).Throw<Exception>();

            Action action = () => _mmRuleService.CleanTransRuleGroupBetting(isLive, 123);

            // Assert
            Assert.Throws<Exception>(action);
        }
        #endregion
    }
}
