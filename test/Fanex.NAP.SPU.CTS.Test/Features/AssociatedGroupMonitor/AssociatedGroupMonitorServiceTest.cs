namespace Fanex.NAP.SPU.CTS.Tests.Features.AssociatedGroupMonitor
{
    using Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor;
    using Fanex.NAP.SPU.CTS.Shares.Models;
    using FluentAssertions;
    using NSubstitute;
    using System.Collections.Generic;   
    using Xunit;

    public class AssociatedGroupMonitorServiceTest
    {
        private readonly IAssociatedGroupMonitorDataAccess associatedGroupMonitorDataAccess;
        private readonly IAssociatedGroupMonitorService associatedGroupMonitorService;

        public AssociatedGroupMonitorServiceTest()
        {
            associatedGroupMonitorDataAccess = Substitute.For<IAssociatedGroupMonitorDataAccess>();
            associatedGroupMonitorService = new AssociatedGroupMonitorService(associatedGroupMonitorDataAccess);
        }

        [Fact]
        public void ScanAssociatedAccount_WhenSuccess_ReturnData()
        {
            // Arrange
            var expectedResult = new ScanAssociatedAccountModel();

            // Act
            associatedGroupMonitorDataAccess.ScanAssociatedAccount().Returns(expectedResult);
            var result = associatedGroupMonitorService.ScanAssociatedAccount();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void AddAssociatedAccount_WhenSuccess_ReturnNotException()
        {
            // Arrange
            var assAccountModel = new AddAssociatedAccountModel();
            var expectedResult = new DBResult();

            // Act
            associatedGroupMonitorDataAccess.AddAssociatedAccount(assAccountModel).Returns(expectedResult);
            var result = associatedGroupMonitorService.AddAssociatedAccount(assAccountModel);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetCustomerInfoMainDB_WhenSuccess_ReturnData()
        {
            // Arrange
            var expectedResult = new List<CustomerInfoMainDBEntity>();
            var custIdList = "1,2,3";
            var sourceType = 2;

            // Act
            associatedGroupMonitorDataAccess.GetCustomerInfoMainDB(custIdList, sourceType).Returns(expectedResult);
            var result = associatedGroupMonitorService.GetCustomerInfoMainDB(custIdList, sourceType);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void UpdateDangerLevelToMainDB_WhenSuccess_ReturnData()
        {
            // Arrange
            var custInfoMainDBModel = new List<CustomerInfoMainDBEntity>();
            var dangerLevelMode = new List<DangerLevelModel>();
            var dangerType = 2;
            var expectedResult = new DBResult();

            // Act
            associatedGroupMonitorDataAccess.UpdateDangerLevelToMainDB(dangerLevelMode, dangerType).Returns(expectedResult);
            var result = associatedGroupMonitorService.UpdateDangerLevelToMainDB(custInfoMainDBModel, dangerType);

            // Assert            
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
