using Xunit;
using System;
using NSubstitute;
using System.Collections.Generic;
using NSubstitute.ExceptionExtensions;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Models;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.MatchMonitorRuleSabaGroupBetting.Entities;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.MMConsts;

namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    public class MatchMonitorRuleSabaGBUnitTest
    {
        private readonly IAssociationDetectionService _assDetect;
        private readonly IMMRuleSabaGBDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMatchMonitorRuleProcessService<MatchStagingRuleSabaGBGroupModel> _mmProcessService;
        private readonly IMMRuleSabaGBService _mmRuleService;

        public MatchMonitorRuleSabaGBUnitTest()
        {
            _assDetect = Substitute.For<IAssociationDetectionService>();
            _dataAccess = Substitute.For<IMMRuleSabaGBDataAccess>();
            _serviceProvider = Substitute.For<IServiceProvider>();

            _mmProcessService = new MMRuleSabaGBProcessService(_assDetect, _dataAccess);
            _mmRuleService = new MMRuleSabaGBService(_dataAccess, _serviceProvider);
        }

        #region Rule GroupBetting
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleSabaGB_WhenSuccess(bool isLive)
        {
            // Arrange
            var output = new MatchMonitorRuleSabaGBModel
            {
                MaxSequenceID = 123123,
                MatchStagingList = new List<MatchStagingRuleSabaGBEntity>
                {
                   new MatchStagingRuleSabaGBEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "a", OldGroup = 1  },
                   new MatchStagingRuleSabaGBEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "a", OldGroup = 2  },
                   new MatchStagingRuleSabaGBEntity { MatchId = 1, ScoreDiff = 2, BetTypeID = 3, BetID = 4, BetTeam = "h", OldGroup = 1  }
                }
            };

            // Act
            _dataAccess.GetMatchRuleSabaGB(isLive).Returns(output);

            var result = _mmRuleService.GetMatchRuleSabaGB(isLive);
            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMatchRuleSabaGB_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.GetMatchRuleSabaGB(isLive).Throws<Exception>();

            Action action = () => _mmRuleService.GetMatchRuleSabaGB(isLive);
            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRuleSabaGB_WhenSuccess(bool isLive)
        {
            // Arrange
            var processRuleSabaGBMethod = TestExtension.GetPrivateMethod(typeof(MMRuleSabaGBProcessService), PrivateMethodName.ProcessRuleSabaGB);

            // Act
            processRuleSabaGBMethod.Invoke(_mmProcessService, new object[] { isLive, new MatchStagingRuleSabaGBEntity(), "asd", "asd" });

            // Assert
            _dataAccess.Received(1).ProcessRuleSabaGB(isLive, Arg.Any<MatchStagingRuleSabaGBEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenSuccess(bool isLive)
        {
            // Arrange
            var matchStagingField = TestExtension.GetPrivateFieldInfo(typeof(MMRuleSabaGBProcessService), PrivateFieldName.MatchStaging);
            matchStagingField.SetValue(_mmProcessService, new MatchStagingRuleSabaGBGroupModel
            {
                MatchGroupStagingList = new List<MatchStagingRuleSabaGBEntity>() { new MatchStagingRuleSabaGBEntity() }
            });

            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MMRuleSabaGBProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMRuleSabaGBProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService, new List<MatchProcessRuleSabaGBModel>());

            // Act
            _assDetect.DetectAssociationRuleSabaGB(Arg.Any<string>()).Returns(new List<DetectAssociationMergeGroupModel>() { new DetectAssociationMergeGroupModel { GroupID = 1 } });
            _dataAccess.ProcessRuleSabaGB(isLive, Arg.Any<MatchStagingRuleSabaGBEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleSabaGBModel());

            var result = _mmProcessService.ProcessRound1();
            // Assert
            _assDetect.Received(1).DetectAssociationRuleSabaGB(Arg.Any<string>());
            _dataAccess.Received(1).ProcessRuleSabaGB(isLive, Arg.Any<MatchStagingRuleSabaGBEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenSuccess(bool isLive)
        {
            // Arrange
            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MMRuleSabaGBProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMRuleSabaGBProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleSabaGBModel> { new MatchProcessRuleSabaGBModel {
                    CompleteMatches = new List<MatchCompleteRuleSabaGBEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleSabaGBInfoEntity> { new MatchProcessRuleSabaGBInfoEntity() },
                    MatchInfo = new MatchStagingRuleSabaGBEntity()
                } });

            // Act
            _assDetect.DetectAssociationRuleSabaGB(Arg.Any<string>()).Returns(new List<DetectAssociationMergeGroupModel>() { new DetectAssociationMergeGroupModel { GroupID = 1 } });
            _dataAccess.ProcessRuleSabaGB(isLive, Arg.Any<MatchStagingRuleSabaGBEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleSabaGBModel());

            var result = _mmProcessService.ProcessRound2();
            // Assert
            _assDetect.Received(1).DetectAssociationRuleSabaGB(Arg.Any<string>());
            _dataAccess.Received(1).ProcessRuleSabaGB(isLive, Arg.Any<MatchStagingRuleSabaGBEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CompleteTransRuleSabaGB_WhenSuccess()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMRuleSabaGBProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleSabaGBModel> { new MatchProcessRuleSabaGBModel {
                    CompleteMatches = new List<MatchCompleteRuleSabaGBEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleSabaGBInfoEntity>(),
                    MatchInfo = new MatchStagingRuleSabaGBEntity()
                } });

            // Act
            _dataAccess.CompleteTransRuleSabaGB(Arg.Any<MatchCompleteRuleSabaGBParameterModel>());

            _mmProcessService.Complete(123123);

            // Assert
            _dataAccess.Received(1).CompleteTransRuleSabaGB(Arg.Any<MatchCompleteRuleSabaGBParameterModel>());
        }

        [Fact]
        public void CompleteTransRuleSabaGB_ThrowException()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMRuleSabaGBProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleSabaGBModel> { new MatchProcessRuleSabaGBModel {
                    CompleteMatches = new List<MatchCompleteRuleSabaGBEntity>(),
                    ReprocessMatches = new List<MatchProcessRuleSabaGBInfoEntity>(),
                    MatchInfo = new MatchStagingRuleSabaGBEntity()
                } });

            // Act
            _dataAccess.When(x => x.CompleteTransRuleSabaGB(Arg.Any<MatchCompleteRuleSabaGBParameterModel>())).Throw<Exception>();

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
            _dataAccess.CleanTransRuleSabaGB(Arg.Any<bool>(), Arg.Any<UInt64>());

            _mmRuleService.CleanTransRuleSabaGB(isLive, 123);
            // Assert
            _dataAccess.Received(1).CleanTransRuleSabaGB(Arg.Any<bool>(), Arg.Any<UInt64>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleSabaGB_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.When(it => it.CleanTransRuleSabaGB(Arg.Any<bool>(), Arg.Any<UInt64>())).Throw<Exception>();

            Action action = () => _mmRuleService.CleanTransRuleSabaGB(isLive, 123);

            // Assert
            Assert.Throws<Exception>(action);
        }
        #endregion

    }
}
