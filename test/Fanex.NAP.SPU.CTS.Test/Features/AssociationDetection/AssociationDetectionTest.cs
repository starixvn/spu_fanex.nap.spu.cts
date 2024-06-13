namespace Fanex.NAP.SPU.CTS.Tests.Features.AssociationDetection
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Models;
    using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;
    using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.Consts.AssociationDetectionConsts;

    public class AssociationDetectionTest
    {
        #region Initial
        private readonly IAssociationDetectionDataAccess _dataAccess;
        private readonly IAssociationDetectionService _service;

        public AssociationDetectionTest()
        {
            _dataAccess = Substitute.For<IAssociationDetectionDataAccess>();

            _service = new AssociationDetectionService(_dataAccess);
        }
        #endregion

        #region GetAssociationCustByAgent
        [Fact]
        public void GetAssociationCustByAgent_WhenSuccess()
        {
            // Arrange
            var getAssociationCustByPrimary = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationCustByAgent);

            var dataAcess = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryEntity>>(JsonString.GetAssociationCust);

            // Act
            _dataAccess.DetectAssociationByAgent(Arg.Any<string>()).Returns(dataAcess);

            var result = getAssociationCustByPrimary.Invoke(_service, new object[] { string.Empty });

            // Assert
            _dataAccess.Received(1).DetectAssociationByAgent(Arg.Any<string>());

            result.Should().BeEquivalentTo(JsonString.GetAssociationCust);
        }

        [Fact]
        public void GetAssociationCustByAgent_ThrowException()
        {
            // Arrange
            var getAssociationCustByPrimary = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationCustByAgent);

            var dataAcess = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryEntity>>(JsonString.GetAssociationCust);

            // Act
            _dataAccess.DetectAssociationByAgent(Arg.Any<string>()).Throws<Exception>();

            Action action = () => getAssociationCustByPrimary.Invoke(_service, new object[] { string.Empty });

            // Assert
            action.Should().Throw<Exception>();
        }
        #endregion

        #region GetAssociationCustByPrimary
        [Theory]
        [InlineData(AssociationDetectionType.AI)]
        [InlineData(AssociationDetectionType.Device)]
        [InlineData(AssociationDetectionType.IP)]
        public void GetAssociationCustByPrimary_WhenSuccess(AssociationDetectionType associationType)
        {
            // Arrange
            var getAssociationCustByPrimary = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationCustByPrimary);

            var dataAcess = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryEntity>>(JsonString.GetAssociationCust);

            var model = new DetectAssociationByPrimaryFilterModel(string.Empty, associationType);

            // Act
            _dataAccess.DetectAssociationByPrimary(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Returns(dataAcess);

            var result = getAssociationCustByPrimary.Invoke(_service, new object[] { model });

            // Assert
            _dataAccess.Received(1).DetectAssociationByPrimary(Arg.Any<DetectAssociationByPrimaryFilterModel>());

            result.Should().BeEquivalentTo(JsonString.GetAssociationCust);
        }

        [Theory]
        [InlineData(AssociationDetectionType.AI)]
        [InlineData(AssociationDetectionType.Device)]
        [InlineData(AssociationDetectionType.IP)]
        public void GetAssociationCustByPrimary_WhenThrowException(AssociationDetectionType associationType)
        {
            // Arrange
            var getAssociationCustByPrimary = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationCustByPrimary);

            var dataAcess = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryEntity>>(JsonString.GetAssociationCust);

            var model = new DetectAssociationByPrimaryFilterModel(string.Empty, associationType);

            // Act
            _dataAccess.DetectAssociationByPrimary(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Throws(new Exception());

            Action action = () => getAssociationCustByPrimary.Invoke(_service, new object[] { model });

            // Assert
            action.Should().Throw<Exception>();
        }
        #endregion

        #region GetAssociationCustByPrimaryLv1
        [Theory]
        [InlineData(AssociationDetectionType.AI)]
        [InlineData(AssociationDetectionType.Device)]
        [InlineData(AssociationDetectionType.IP)]
        public void GetAssociationCustByPrimaryLv1_WhenSuccess(AssociationDetectionType associationType)
        {
            // Arrange
            var getAssociationCustByPrimaryLv1 = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationCustByPrimaryLv1);

            var dataAcess = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryLv1Entity>>(JsonString.GetAssociationCustLv1);

            // Act
            _dataAccess.DetectAssociationByPrimaryLv1(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Returns(dataAcess);

            var result = getAssociationCustByPrimaryLv1.Invoke(_service, new object[] { string.Empty, associationType });

            // Assert
            _dataAccess.Received(1).DetectAssociationByPrimaryLv1(Arg.Any<DetectAssociationByPrimaryFilterModel>());

            result.Should().BeEquivalentTo(JsonString.GetAssociationCustLv1);
        }

        [Theory]
        [InlineData(AssociationDetectionType.AI)]
        [InlineData(AssociationDetectionType.Device)]
        [InlineData(AssociationDetectionType.IP)]
        public void GetAssociationCustByPrimaryLv1_WhenThrowException(AssociationDetectionType associationType)
        {
            // Arrange
            var getAssociationCustByPrimaryLv1 = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationCustByPrimaryLv1);

            var dataAcess = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryLv1Entity>>(JsonString.GetAssociationCustLv1);

            // Act
            _dataAccess.DetectAssociationByPrimaryLv1(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Throws(new Exception());

            Action action = () => getAssociationCustByPrimaryLv1.Invoke(_service, new object[] { string.Empty, associationType });

            // Assert
            action.Should().Throw<Exception>();
        }
        #endregion

        #region GetAssociationBySharedMatches
        [Fact]
        public void GetAssociationBySharedMatches_WhenSuccess()
        {
            // Arrange
            var getAssociationCustByPrimaryLv1 = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationBySharedMatches);

            var dataAcess = JsonConvert.DeserializeObject<IEnumerable<DetectAssBySharedIPMatchesEntity>>(JsonString.GetAssociationCustLv1);

            // Act
            _dataAccess.DetectAssociationBySharedMatches(Arg.Any<string>()).Returns(dataAcess);

            var result = getAssociationCustByPrimaryLv1.Invoke(_service, new object[] { string.Empty, new Dictionary<int, int>() });

            // Assert
            _dataAccess.Received(1).DetectAssociationBySharedMatches(Arg.Any<string>());

            result.Should().NotBeNull();
        }

        [Fact]
        public void GetAssociationBySharedMatches_WhenThrowException()
        {
            // Arrange
            var getAssociationCustByPrimaryLv1 = TestExtension.GetPrivateMethod(typeof(AssociationDetectionService), PrivateMethodName.GetAssociationBySharedMatches);

            // Act
            _dataAccess.DetectAssociationBySharedMatches(Arg.Any<string>()).Throws(new Exception());

            Action action = () => getAssociationCustByPrimaryLv1.Invoke(_service, new object[] { string.Empty });

            // Assert
            action.Should().Throw<Exception>();
        }
        #endregion

        #region DetectAssociation
        [Theory]
        [InlineData("", true, true, true, true)]
        [InlineData("1,2,3", true, true, true, true)]
        public void DetectAssociation_WhenSuccess(string ctsCustIDs, bool isDevice, bool isAI, bool isIP, bool is3MatchesLast7Days)
        {
            // Arrange
            var assCustData = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryEntity>>(JsonString.GetAssociationCust);
            var assMatchData = JsonConvert.DeserializeObject<IEnumerable<DetectAssBySharedIPMatchesEntity>>(JsonString.GetAssociationCust);
            var detectAss = new List<DetectAssociationMergeGroupEntity>
            {
                new DetectAssociationMergeGroupEntity
                {
                    GroupID = 123,
                    CustID = 234
                }
            };

            var model = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDs,
                CustIds = ctsCustIDs,
                IsDevice = isDevice,
                IsAI = isAI,
                IsIP = isIP,
                Is3MatchesLast7Days = is3MatchesLast7Days,
                MappingCustDictionary = new Dictionary<int, int>() { { 1, 2 } }
            };

            // Act
            _dataAccess.DetectAssociationByPrimary(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Returns(assCustData);
            _dataAccess.DetectAssociationBySharedMatches(Arg.Any<string>()).Returns(assMatchData);
            _dataAccess.DetectGroupDetectAssociation(Arg.Any<MergeGroupAssociationFilterModel>()).Returns(detectAss);

            var result = _service.DetectAssociation(model);

            // Assert
            if (string.IsNullOrEmpty(ctsCustIDs))
            {
                result.Should().BeEmpty();
                return;
            }

            _dataAccess.Received(3).DetectAssociationByPrimary(Arg.Any<DetectAssociationByPrimaryFilterModel>());
            _dataAccess.Received(1).DetectAssociationBySharedMatches(Arg.Any<string>());
            _dataAccess.Received(1).DetectGroupDetectAssociation(Arg.Any<MergeGroupAssociationFilterModel>());

            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("1,2,3", true, true, true, true)]
        [InlineData("1,2,3", true, true, true, false)]
        [InlineData("1,2,3", true, true, false, true)]
        [InlineData("1,2,3", true, false, true, true)]
        [InlineData("1,2,3", false, true, true, true)]
        public void DetectAssociation_WhenThrowException(string ctsCustIDs, bool isDevice, bool isAI, bool isIP, bool is3MatchesLast7Days)
        {
            // Arrange
            var model = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDs,
                IsDevice = isDevice,
                IsAI = isAI,
                IsIP = isIP,
                Is3MatchesLast7Days = is3MatchesLast7Days
            };

            // Act
            _dataAccess.DetectAssociationByPrimary(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Throws(new Exception());
            _dataAccess.DetectAssociationBySharedMatches(Arg.Any<string>()).Throws(new Exception());
            _dataAccess.DetectGroupDetectAssociation(Arg.Any<MergeGroupAssociationFilterModel>()).Throws(new Exception());

            Action action = () => _service.DetectAssociation(model);

            // Assert
            action.Should().Throw<Exception>();
        }
        #endregion

        #region DetectAssociationLv1
        [Theory]
        [InlineData("", true, true, true, true)]
        [InlineData("1,2,3", true, true, true, true)]
        [InlineData("1,2,3", true, true, false, false)]
        public void DetectAssociationLv1_WhenSuccess(string ctsCustIDs, bool isDevice, bool isAI, bool isIP, bool is3MatchesLast7Days)
        {
            // Arrange
            var assCustData = JsonConvert.DeserializeObject<IEnumerable<DetectAssociationByPrimaryLv1Entity>>(JsonString.GetAssociationCustLv1);
            var assMatchData = JsonConvert.DeserializeObject<IEnumerable<DetectAssBySharedIPMatchesEntity>>(JsonString.GetAssociationCustLv1);
            var detectAss = new List<DetectAssociationMergeGroupLv1Entity>
            {
                new DetectAssociationMergeGroupLv1Entity
                {
                    GroupID = 123,
                    NodeID = 234
                }
            };

            var model = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDs,
                CustIds = ctsCustIDs,
                IsDevice = isDevice,
                IsAI = isAI,
                IsIP = isIP,
                Is3MatchesLast7Days = is3MatchesLast7Days,
                MappingCustDictionary = new Dictionary<int, int>() { { 1, 2 } }
            };

            // Act
            _dataAccess.DetectAssociationByPrimaryLv1(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Returns(assCustData);
            _dataAccess.DetectAssociationBySharedMatches(Arg.Any<string>()).Returns(assMatchData);
            _dataAccess.DetectGroupDetectAssociationLv1(Arg.Any<MergeGroupAssociationFilterLv1Model>()).Returns(detectAss);

            var result = _service.DetectAssociationLv1(model);

            // Assert
            if (string.IsNullOrEmpty(ctsCustIDs))
            {
                result.Should().BeEmpty();
                return;
            }

            if (isIP && is3MatchesLast7Days)
            {
                _dataAccess.Received(3).DetectAssociationByPrimaryLv1(Arg.Any<DetectAssociationByPrimaryFilterModel>());
                _dataAccess.Received(1).DetectAssociationBySharedMatches(Arg.Any<string>());
            }

            if (!isIP && !is3MatchesLast7Days)
            {
                _dataAccess.Received(2).DetectAssociationByPrimaryLv1(Arg.Any<DetectAssociationByPrimaryFilterModel>());
                _dataAccess.Received(0).DetectAssociationBySharedMatches(Arg.Any<string>());
            }

            _dataAccess.Received(1).DetectGroupDetectAssociationLv1(Arg.Any<MergeGroupAssociationFilterLv1Model>());
            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("1,2,3", true, true, true, true)]
        [InlineData("1,2,3", true, true, true, false)]
        [InlineData("1,2,3", true, true, false, true)]
        [InlineData("1,2,3", true, false, true, true)]
        [InlineData("1,2,3", false, true, true, true)]
        public void DetectAssociationLv1_WhenThrowException(string ctsCustIDs, bool isDevice, bool isAI, bool isIP, bool is3MatchesLast7Days)
        {
            // Arrange
            var model = new DetectAssociationParamModel
            {
                CTSCustIds = ctsCustIDs,
                IsDevice = isDevice,
                IsAI = isAI,
                IsIP = isIP,
                Is3MatchesLast7Days = is3MatchesLast7Days
            };

            // Act
            _dataAccess.DetectAssociationByPrimaryLv1(Arg.Any<DetectAssociationByPrimaryFilterModel>()).Throws(new Exception());
            _dataAccess.DetectAssociationBySharedMatches(Arg.Any<string>()).Throws(new Exception());
            _dataAccess.DetectGroupDetectAssociationLv1(Arg.Any<MergeGroupAssociationFilterLv1Model>()).Throws(new Exception());

            Action action = () => _service.DetectAssociationLv1(model);

            // Assert
            action.Should().Throw<Exception>();
        }
        #endregion
    }
}