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
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities;
using Fanex.NAP.SPU.CTS.Features.MatchMonitor.Models;
using System.Linq;
using System.Reflection;
using static Fanex.NAP.SPU.CTS.Shares.Consts;
using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.MMConsts;

namespace Fanex.NAP.SPU.CTS.Tests.Features.MatchMonitor
{
    public class MatchMonitorRuleGroupBettingUnitTest
    {
        private static readonly HashSet<int> ValidSportForIPAnd3MatchesLast7Days = new()
        {
            (int)MatchMonitor_SportType.Basketball,
            (int)MatchMonitor_SportType.Esport,
            (int)MatchMonitor_SportType.Cricket,
            (int)MatchMonitor_SportType.Badminton,
            (int)MatchMonitor_SportType.TableTennis
        };

        private static readonly HashSet<int> ValidSportForIPLast3Days = new()
        {
            (int)MatchMonitor_SportType.Basketball,
            (int)MatchMonitor_SportType.Esport
        };

        private readonly IAssociationDetectionService _assDetect;
        private readonly IMatchMonitorRuleGroupBettingDataAccess _dataAccess;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMatchMonitorRuleProcessService<MatchStagingRuleGroupBettingGroupModel> _mmProcessService;
        private readonly IMatchMonitorRuleGroupBettingService _mmRuleService;

        private readonly MatchDetectAssociationSettingModel _assDetectSettingModel;

        public MatchMonitorRuleGroupBettingUnitTest()
        {
            _assDetect = Substitute.For<IAssociationDetectionService>();
            _dataAccess = Substitute.For<IMatchMonitorRuleGroupBettingDataAccess>();
            _serviceProvider = Substitute.For<IServiceProvider>();

            _mmProcessService = new MatchMonitorRuleGroupBettingProcessService(_assDetect, _dataAccess);
            _mmRuleService = new MatchMonitorRuleGroupBettingService(_dataAccess, _serviceProvider);

            _assDetectSettingModel = new MatchDetectAssociationSettingModel
            {
                DeviceSports = ValidSportForIPAnd3MatchesLast7Days.ToHashSet(),
                AISports = ValidSportForIPAnd3MatchesLast7Days.ToHashSet(),
                IPSports = ValidSportForIPAnd3MatchesLast7Days.ToHashSet(),
                _3MatchesLast7DaysSports = ValidSportForIPAnd3MatchesLast7Days.ToHashSet(),
                IPLast3DaysSports = ValidSportForIPLast3Days.ToHashSet()
            };
        }

        #region Rule GroupBetting

        #region GetMatchRuleGroupBetting
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
                },
                CTSCustomerList = new List<MatchMonitorMappingCustEntity>()
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
        #endregion

        #region ProcessRuleGroupBetting
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
        public void ProcessRuleGroupBetting_ThrowException(bool isLive)
        {
            // Arrange
            var processRuleSabaGBMethod = TestExtension.GetPrivateMethod(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateMethodName.ProcessRuleGroupBetting);

            // Act
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Throws<Exception>();

            Action action = () => processRuleSabaGBMethod.Invoke(_mmProcessService, new object[] { isLive, new MatchStagingRuleGroupBettingEntity(), "asd", "asd" });

            // Assert
            _dataAccess.Received(0).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());

            Assert.Throws<TargetInvocationException>(action);
        }
        #endregion

        #region ProcessRound1
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenSuccess_ReturnData(bool isLive)
        {
            // Arrange
            var matchStagingEntity = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                ScoreDiff = 2,
                BettypeID = 3,
                BetID = 4,
                Betteam = "h",
                SportType = (int)MatchMonitor_SportType.Basketball,
                CTSCustIDList = "1,2,3",
                CustIDList = "4,5,6",
                SequenceIDList = "seq1,seq2,seq3",
                LicCustList = "lic1,lic2"
            };

            MockDataMatchStaging(matchStagingEntity);
            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound1();

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound1();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_mmProcessService, result); // Should return self for method chaining
            _assDetect.Received(1).DetectAssociationLv1(Arg.Is<DetectAssociationParamModel>(p =>
                p.CTSCustIds == "1,2,3" &&
                p.CustIds == "4,5,6" &&
                p.IsDevice == true &&
                p.IsAI == true &&
                p.IsIP == true &&
                p.Is3MatchesLast7Days == true &&
                p.IsIPLast3Days == true &&
                p.CustIdsIPLast3Days == "lic1,lic2"));
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenNoAssociationDetected_ShouldNotProcess(bool isLive)
        {
            // Arrange
            var matchStagingEntity = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball,
                CTSCustIDList = "1,2,3",
                CustIDList = "4,5,6",
                SequenceIDList = "seq1,seq2,seq3",
                LicCustList = "lic1,lic2"
            };

            MockDataMatchStaging(matchStagingEntity);
            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound1();

            // Act - Return empty list (no associations detected)
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>());

            var result = _mmProcessService.ProcessRound1();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(0).ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenAssociationDetectedButNoGroupID_ShouldNotProcess(bool isLive)
        {
            // Arrange
            var matchStagingEntity = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball,
                CTSCustIDList = "1,2,3",
                CustIDList = "4,5,6",
                SequenceIDList = "seq1,seq2,seq3",
                LicCustList = "lic1,lic2"
            };

            MockDataMatchStaging(matchStagingEntity);
            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound1();

            // Act - Return associations without GroupID
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = null } });

            var result = _mmProcessService.ProcessRound1();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(0).ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenMatchStagingListIsEmpty_ShouldNotProcess(bool isLive)
        {
            // Arrange
            MockDataMatchStaging(new MatchStagingRuleGroupBettingGroupModel
            {
                MatchGroupStagingList = new List<MatchStagingRuleGroupBettingEntity>()
            });
            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound1();

            // Act
            var result = _mmProcessService.ProcessRound1();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(0).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(0).ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound1_WhenMultipleMatches_ShouldProcessAll(bool isLive)
        {
            // Arrange
            var matchStagingEntities = new List<MatchStagingRuleGroupBettingEntity>
            {
                new MatchStagingRuleGroupBettingEntity
                {
                    MatchID = 123,
                    SportType = (int)MatchMonitor_SportType.Basketball,
                    CTSCustIDList = "1,2,3",
                    CustIDList = "4,5,6",
                    SequenceIDList = "seq1,seq2,seq3",
                    LicCustList = "lic1,lic2"
                },
                new MatchStagingRuleGroupBettingEntity
                {
                    MatchID = 456,
                    SportType = (int)MatchMonitor_SportType.Esport,
                    CTSCustIDList = "7,8,9",
                    CustIDList = "10,11,12",
                    SequenceIDList = "seq4,seq5,seq6",
                    LicCustList = "lic3,lic4"
                }
            };

            MockDataMatchStaging(new MatchStagingRuleGroupBettingGroupModel
            {
                MatchGroupStagingList = matchStagingEntities
            });
            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound1();

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound1();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(2).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(2).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData((int)MatchMonitor_SportType.Basketball, true, true, true, true, true)]
        [InlineData((int)MatchMonitor_SportType.Esport, true, true, true, true, true)]
        [InlineData((int)MatchMonitor_SportType.Cricket, true, true, true, true, false)]
        [InlineData((int)MatchMonitor_SportType.Badminton, true, true, true, true, false)]
        [InlineData((int)MatchMonitor_SportType.TableTennis, true, true, true, true, false)]
        public void ProcessRound1_WhenDifferentSportTypes_ShouldSetCorrectDetectionFlags(int sportType, bool expectedDevice, bool expectedAI, bool expectedIP, bool expected3Matches, bool expectedIPLast3Days)
        {
            // Arrange
            var matchStagingEntity = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = sportType,
                CTSCustIDList = "1,2,3",
                CustIDList = "4,5,6",
                SequenceIDList = "seq1,seq2,seq3",
                LicCustList = "lic1,lic2"
            };

            MockDataMatchStaging(matchStagingEntity);
            MockDataIsLive(true);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound1();

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound1();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(1).DetectAssociationLv1(Arg.Is<DetectAssociationParamModel>(p =>
                p.IsDevice == expectedDevice &&
                p.IsAI == expectedAI &&
                p.IsIP == expectedIP &&
                p.Is3MatchesLast7Days == expected3Matches &&
                p.IsIPLast3Days == expectedIPLast3Days));
        }
        #endregion

        #region ProcessRound2
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenSuccess(bool isLive)
        {
            // Arrange
            var matchInfo = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball,
                CTSCustIDList = "1,2,3",
                CustIDList = "4,5,6",
                SequenceIDList = "seq1,seq2,seq3",
                LicCustList = "lic1,lic2"
            };

            var reprocessInfo = new MatchProcessRuleGroupBettingInfoEntity
            {
                CTSCustIDList = "7,8,9",
                CustIDList = "10,11,12",
                SequenceIDList = "seq4,seq5,seq6",
                LicCustList = "lic3,lic4"
            };

            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2(matchInfo, reprocessInfo);

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_mmProcessService, result); // Should return self for method chaining
            _assDetect.Received(1).DetectAssociationLv1(Arg.Is<DetectAssociationParamModel>(p =>
                p.CTSCustIds == "7,8,9" &&
                p.CustIds == "10,11,12" &&
                p.IsDevice == true &&
                p.IsAI == true &&
                p.IsIP == true &&
                p.Is3MatchesLast7Days == true &&
                p.IsIPLast3Days == true &&
                p.CustIdsIPLast3Days == "lic3,lic4"));
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenNoMatchProcesses_ShouldNotProcess(bool isLive)
        {
            // Arrange
            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound1(); // Empty match processes

            // Act
            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(0).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(0).ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenNoReprocessMatches_ShouldNotProcess(bool isLive)
        {
            // Arrange
            var matchInfo = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball
            };

            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2(matchInfo); // No reprocess matches

            // Act
            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(0).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(0).ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenNoAssociationDetected_ShouldNotProcess(bool isLive)
        {
            // Arrange
            var matchInfo = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball
            };

            var reprocessInfo = new MatchProcessRuleGroupBettingInfoEntity
            {
                CTSCustIDList = "7,8,9",
                CustIDList = "10,11,12",
                SequenceIDList = "seq4,seq5,seq6",
                LicCustList = "lic3,lic4"
            };

            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2(matchInfo, reprocessInfo);

            // Act - Return empty list (no associations detected)
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>());

            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(0).ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenAssociationDetectedButNoGroupID_ShouldNotProcess(bool isLive)
        {
            // Arrange
            var matchInfo = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball
            };

            var reprocessInfo = new MatchProcessRuleGroupBettingInfoEntity
            {
                CTSCustIDList = "7,8,9",
                CustIDList = "10,11,12",
                SequenceIDList = "seq4,seq5,seq6",
                LicCustList = "lic3,lic4"
            };

            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2(matchInfo, reprocessInfo);

            // Act - Return associations without GroupID
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = null } });

            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(0).ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenMultipleReprocessMatches_ShouldProcessAll(bool isLive)
        {
            // Arrange
            var matchInfo = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball
            };

            var reprocessInfos = new List<MatchProcessRuleGroupBettingInfoEntity>
            {
                new MatchProcessRuleGroupBettingInfoEntity
                {
                    CTSCustIDList = "7,8,9",
                    CustIDList = "10,11,12",
                    SequenceIDList = "seq4,seq5,seq6",
                    LicCustList = "lic3,lic4"
                },
                new MatchProcessRuleGroupBettingInfoEntity
                {
                    CTSCustIDList = "13,14,15",
                    CustIDList = "16,17,18",
                    SequenceIDList = "seq7,seq8,seq9",
                    LicCustList = "lic5,lic6"
                }
            };

            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2(matchInfo, reprocessInfos);

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(2).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(2).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_WhenMultipleMatchProcesses_ShouldProcessAllReprocessMatches(bool isLive)
        {
            // Arrange
            var matchInfo1 = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball
            };

            var matchInfo2 = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 456,
                SportType = (int)MatchMonitor_SportType.Esport
            };

            var reprocessInfo1 = new MatchProcessRuleGroupBettingInfoEntity
            {
                CTSCustIDList = "7,8,9",
                CustIDList = "10,11,12",
                SequenceIDList = "seq4,seq5,seq6",
                LicCustList = "lic3,lic4"
            };

            var reprocessInfo2 = new MatchProcessRuleGroupBettingInfoEntity
            {
                CTSCustIDList = "13,14,15",
                CustIDList = "16,17,18",
                SequenceIDList = "seq7,seq8,seq9",
                LicCustList = "lic5,lic6"
            };

            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2Multiple(matchInfo1, matchInfo2, reprocessInfo1, reprocessInfo2);

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(2).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(2).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData((int)MatchMonitor_SportType.Basketball, true, true, true, true, true)]
        [InlineData((int)MatchMonitor_SportType.Esport, true, true, true, true, true)]
        [InlineData((int)MatchMonitor_SportType.Cricket, true, true, true, true, false)]
        [InlineData((int)MatchMonitor_SportType.Badminton, true, true, true, true, false)]
        [InlineData((int)MatchMonitor_SportType.TableTennis, true, true, true, true, false)]
        public void ProcessRound2_WhenDifferentSportTypes_ShouldSetCorrectDetectionFlags(int sportType, bool expectedDevice, bool expectedAI, bool expectedIP, bool expected3Matches, bool expectedIPLast3Days)
        {
            // Arrange
            var matchInfo = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = sportType
            };

            var reprocessInfo = new MatchProcessRuleGroupBettingInfoEntity
            {
                CTSCustIDList = "7,8,9",
                CustIDList = "10,11,12",
                SequenceIDList = "seq4,seq5,seq6",
                LicCustList = "lic3,lic4"
            };

            MockDataIsLive(true);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2(matchInfo, reprocessInfo);

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            _dataAccess.ProcessRuleGroupBetting(Arg.Any<bool>(), Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new MatchProcessRuleGroupBettingModel());

            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            _assDetect.Received(1).DetectAssociationLv1(Arg.Is<DetectAssociationParamModel>(p =>
                p.IsDevice == expectedDevice &&
                p.IsAI == expectedAI &&
                p.IsIP == expectedIP &&
                p.Is3MatchesLast7Days == expected3Matches &&
                p.IsIPLast3Days == expectedIPLast3Days));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessRound2_ShouldSetRoundNumberTo2(bool isLive)
        {
            // Arrange
            var matchInfo = new MatchStagingRuleGroupBettingEntity
            {
                MatchID = 123,
                SportType = (int)MatchMonitor_SportType.Basketball
            };

            var reprocessInfo = new MatchProcessRuleGroupBettingInfoEntity
            {
                CTSCustIDList = "7,8,9",
                CustIDList = "10,11,12",
                SequenceIDList = "seq4,seq5,seq6",
                LicCustList = "lic3,lic4"
            };

            MockDataIsLive(isLive);
            MockDataAssDetectSetting();
            MockDataMatchProcessRound2(matchInfo, reprocessInfo);

            // Act
            _assDetect.DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>()).Returns(new List<DetectAssociationMergeGroupLv1Entity>() { new DetectAssociationMergeGroupLv1Entity { GroupID = 1 } });
            var returnedModel = new MatchProcessRuleGroupBettingModel();
            _dataAccess.ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>()).Returns(returnedModel);

            var result = _mmProcessService.ProcessRound2();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, returnedModel.RoundNumber); // Should set RoundNumber to 2
            _assDetect.Received(1).DetectAssociationLv1(Arg.Any<DetectAssociationParamModel>());
            _dataAccess.Received(1).ProcessRuleGroupBetting(isLive, Arg.Any<MatchStagingRuleGroupBettingEntity>(), Arg.Any<string>(), Arg.Any<string>());
        }
        #endregion

        #region CompleteTransRuleSabaGB
        [Fact]
        public void CompleteTransRuleGroupBetting_WhenSuccess()
        {
            // Arrange
            MockDataMatchProcessRound2();

            // Act
            _dataAccess.CompleteTransRuleGroupBetting(Arg.Any<MatchCompleteRuleGroupBettingParameterModel>());

            _mmProcessService.Complete(123123);

            // Assert
            _dataAccess.Received(1).CompleteTransRuleGroupBetting(Arg.Any<MatchCompleteRuleGroupBettingParameterModel>());
        }

        [Fact]
        public void CompleteTransRuleGroupBetting_ThrowException()
        {
            // Arrange
            MockDataMatchProcessRound2();

            // Act
            _dataAccess.When(x => x.CompleteTransRuleGroupBetting(Arg.Any<MatchCompleteRuleGroupBettingParameterModel>())).Throw<Exception>();

            Action action = () => _mmProcessService.Complete(123123);

            // Assert
            Assert.Throws<Exception>(action);
        }
        #endregion

        #region CleanTransRuleSabaGB
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

        #endregion

        #region private method
        private void MockDataMatchStaging(MatchStagingRuleGroupBettingEntity matchStagingEntity = null)
        {
            var matchStagingField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchStaging);

            matchStagingField.SetValue(_mmProcessService, new MatchStagingRuleGroupBettingGroupModel
            {
                MatchGroupStagingList = new List<MatchStagingRuleGroupBettingEntity>() { matchStagingEntity ?? new MatchStagingRuleGroupBettingEntity() }
            });
        }

        private void MockDataMatchStaging(MatchStagingRuleGroupBettingGroupModel matchStagingGroup)
        {
            var matchStagingField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchStaging);
            matchStagingField.SetValue(_mmProcessService, matchStagingGroup);
        }

        private void MockDataIsLive(bool isLive)
        {
            var isLiveField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.IsLive);
            isLiveField.SetValue(_mmProcessService, isLive);
        }

        private void MockDataAssDetectSetting()
        {
            var assDetectSettingField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.AssDetectSetting);
            assDetectSettingField.SetValue(_mmProcessService, _assDetectSettingModel);
        }

        private void MockDataMatchProcessRound1()
        {
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);
            matchProcessField.SetValue(_mmProcessService, new List<MatchProcessRuleGroupBettingModel>());
        }

        private void MockDataMatchProcessRound2(MatchStagingRuleGroupBettingEntity matchInfo = null, MatchProcessRuleGroupBettingInfoEntity reprocessInfo = null)
        {
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);

            var reprocessMatches = reprocessInfo != null
                ? new List<MatchProcessRuleGroupBettingInfoEntity> { reprocessInfo }
                : new List<MatchProcessRuleGroupBettingInfoEntity>();

            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleGroupBettingModel> { new MatchProcessRuleGroupBettingModel {
                    CompleteMatches = new List<MatchCompleteRuleGroupBettingEntity>{ new MatchCompleteRuleGroupBettingEntity() },
                    ReprocessMatches = reprocessMatches,
                    MatchInfo = matchInfo ?? new MatchStagingRuleGroupBettingEntity()
                } });
        }

        private void MockDataMatchProcessRound2(MatchStagingRuleGroupBettingEntity matchInfo, List<MatchProcessRuleGroupBettingInfoEntity> reprocessInfos)
        {
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);

            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleGroupBettingModel> { new MatchProcessRuleGroupBettingModel {
                    CompleteMatches = new List<MatchCompleteRuleGroupBettingEntity>{ new MatchCompleteRuleGroupBettingEntity() },
                    ReprocessMatches = reprocessInfos ?? new List<MatchProcessRuleGroupBettingInfoEntity>(),
                    MatchInfo = matchInfo ?? new MatchStagingRuleGroupBettingEntity()
                } });
        }

        private void MockDataMatchProcessRound2Multiple(MatchStagingRuleGroupBettingEntity matchInfo1, MatchStagingRuleGroupBettingEntity matchInfo2,
            MatchProcessRuleGroupBettingInfoEntity reprocessInfo1, MatchProcessRuleGroupBettingInfoEntity reprocessInfo2)
        {
            var matchProcessField = TestExtension.GetPrivateFieldInfo(typeof(MatchMonitorRuleGroupBettingProcessService), PrivateFieldName.MatchProcesses);

            matchProcessField.SetValue(_mmProcessService,
                new List<MatchProcessRuleGroupBettingModel>
                {
                    new MatchProcessRuleGroupBettingModel {
                        CompleteMatches = new List<MatchCompleteRuleGroupBettingEntity>{ new MatchCompleteRuleGroupBettingEntity() },
                        ReprocessMatches = new List<MatchProcessRuleGroupBettingInfoEntity> { reprocessInfo1 },
                        MatchInfo = matchInfo1
                    },
                    new MatchProcessRuleGroupBettingModel {
                        CompleteMatches = new List<MatchCompleteRuleGroupBettingEntity>{ new MatchCompleteRuleGroupBettingEntity() },
                        ReprocessMatches = new List<MatchProcessRuleGroupBettingInfoEntity> { reprocessInfo2 },
                        MatchInfo = matchInfo2
                    }
                });
        }
        #endregion
    }
}
