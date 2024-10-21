namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor.Parlay
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Parlay.MMParlayRuleGroupBetting;
    using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;    
    using Xunit;
    using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.MMConsts;

    public class MMParlayRuleGroupBettingServiceTest
    {
        private readonly IMMParlayRuleGroupBettingDataAccess _dataAccess;
        private readonly IAssociationDetectionService _assDetectService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMMParlayRuleGroupBettingService _service;
        private readonly IMMParlayRuleProcessService<MMParlayStagingRuleGroupBettingGroupModel> _mmProcessService;

        public MMParlayRuleGroupBettingServiceTest()
        {
            _assDetectService = Substitute.For<IAssociationDetectionService>();
            _serviceProvider = Substitute.For<IServiceProvider>();
            _dataAccess = Substitute.For<IMMParlayRuleGroupBettingDataAccess>();

            _mmProcessService = new MMParlayRuleGroupBettingProcessService(_assDetectService, _dataAccess);
            _service = new MMParlayRuleGroupBettingService(_dataAccess, _serviceProvider);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMMParlayRuleGroupBetting_WhenSuccess_ReturnData(bool isLive)
        {
            // Arrange
            var expectedResult = new MMParlayRuleGroupBettingModel
            {
                MaxSequenceID = 123,
                MatchStagingList = new List<MMParlayStagingRuleGroupBettingEntity>
                {
                   new MMParlayStagingRuleGroupBettingEntity { MatchID = 1, ScoreDiff = 2, BettypeID = 3, BetID = 4, Betteam = "a"  },
                   new MMParlayStagingRuleGroupBettingEntity { MatchID = 1, ScoreDiff = 2, BettypeID = 3, BetID = 4, Betteam = "a"  },
                   new MMParlayStagingRuleGroupBettingEntity { MatchID = 1, ScoreDiff = 2, BettypeID = 3, BetID = 4, Betteam = "h"  }
                }
            };

            // Act
            _dataAccess.GetMMParlayRuleGroupBettting(isLive).Returns(expectedResult);

            var result = _service.GetMMParlayRuleGroupBetting(isLive);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMMParlayRuleGroupBetting_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.GetMMParlayRuleGroupBettting(isLive).Throws<Exception>();

            Action action = () => _service.GetMMParlayRuleGroupBetting(isLive);
            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRuleGroupBetting_WhenSuccess(bool isLive)
        {
            // Arrange
            var processRuleMethod = TestExtension.GetPrivateMethod(typeof(MMParlayRuleGroupBettingProcessService), PrivateMethodName.ProcessRuleGroupBetting);

            // Act
            processRuleMethod.Invoke(_mmProcessService, new object[] { isLive, new MMParlayStagingRuleGroupBettingEntity(), "a", "a" });

            // Assert
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MMParlayStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenSuccess(bool isLive)
        {
            // Arrange
            var matchStagingField = TestExtension.GetPrivateFieldInfo(typeof(MMParlayRuleGroupBettingProcessService), PrivateFieldName.MatchStaging);

            matchStagingField.SetValue(_mmProcessService, new MMParlayStagingRuleGroupBettingGroupModel
            {
                MatchGroupStagingList = new List<MMParlayStagingRuleGroupBettingEntity>() { new MMParlayStagingRuleGroupBettingEntity() }
            });

            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MMParlayRuleGroupBettingProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMParlayRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService, new List<MMParlayProcessRuleGroupBettingModel>());

            // Act
            _assDetectService.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MMParlayStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MMParlayProcessRuleGroupBettingModel());

            _mmProcessService.ProcessRound1();

            // Assert
            _assDetectService.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MMParlayStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenSuccess(bool isLive)
        {
            // Arrange
            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MMParlayRuleGroupBettingProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);

            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMParlayRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MMParlayProcessRuleGroupBettingModel> { new MMParlayProcessRuleGroupBettingModel {
                    MatchInfo = new MMParlayStagingRuleGroupBettingEntity(),
                    CompleteMatches = new List<MMParlayCompleteRuleGroupBettingEntity>(),
                    ReprocessMatches = new List<MMParlayProcessRuleGroupBettingInfoEntity> { new MMParlayProcessRuleGroupBettingInfoEntity() }                    
                } });

            // Act
            _assDetectService.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MMParlayStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MMParlayProcessRuleGroupBettingModel());

           _mmProcessService.ProcessRound2();

            // Assert
            _assDetectService.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());

            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MMParlayStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CompleteTransRuleGroupBetting_WhenSuccess()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMParlayRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                new List<MMParlayProcessRuleGroupBettingModel> { new MMParlayProcessRuleGroupBettingModel {
                    MatchInfo = new MMParlayStagingRuleGroupBettingEntity(),
                    CompleteMatches = new List<MMParlayCompleteRuleGroupBettingEntity>() { new MMParlayCompleteRuleGroupBettingEntity() },
                    ReprocessMatches = new List<MMParlayProcessRuleGroupBettingInfoEntity>()                    
                } });

            // Act
            _dataAccess.CompleteTransRuleGroupBetting(Arg.Any<MMParlayCompleteRuleGroupBettingParameterModel>());

            _mmProcessService.Complete(111);

            // Assert
            _dataAccess.Received(1).CompleteTransRuleGroupBetting(Arg.Any<MMParlayCompleteRuleGroupBettingParameterModel>());
        }

        [Fact]
        public void CompleteTransRuleGroupBetting_ThrowException()
        {
            // Arrange
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MMParlayRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService,
                 new List<MMParlayProcessRuleGroupBettingModel> { new MMParlayProcessRuleGroupBettingModel {
                        MatchInfo = new MMParlayStagingRuleGroupBettingEntity(),
                        CompleteMatches = new List<MMParlayCompleteRuleGroupBettingEntity>() { new MMParlayCompleteRuleGroupBettingEntity() },
                        ReprocessMatches = new List<MMParlayProcessRuleGroupBettingInfoEntity>()
                } });

            // Act
            _dataAccess.When(x => x.CompleteTransRuleGroupBetting(Arg.Any<MMParlayCompleteRuleGroupBettingParameterModel>())).Throw<Exception>();

            Action action = () => _mmProcessService.Complete(123123);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleGroupBetting_WhenSuccess(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.CleanTransRuleGroupBetting(Arg.Any<bool>(), Arg.Any<UInt64>());

            _service.CleanTransRuleGroupBetting(isLive, 123);
            // Assert
            _dataAccess.Received(1).CleanTransRuleGroupBetting(Arg.Any<bool>(), Arg.Any<UInt64>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CleanTransRuleGroupBetting_ThrowException(bool isLive)
        {
            // Arrange

            // Act
            _dataAccess.When(it => it.CleanTransRuleGroupBetting(Arg.Any<bool>(), Arg.Any<UInt64>())).Throw<Exception>();

            Action action = () => _service.CleanTransRuleGroupBetting(isLive, 123);

            // Assert
            Assert.Throws<Exception>(action);
        }
    }
}
