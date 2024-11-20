using Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus;
using Fanex.NAP.SPU.CTS.Features.InitialCustomerStatus.Entities;
using Fanex.NAP.SPU.CTS.Tests.Infrastructure.Extensions;
using NSubstitute;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.Shares.Consts;
using static Fanex.NAP.SPU.CTS.Tests.Infrastructure.Consts.CustomerStatusConsts;

namespace Fanex.NAP.SPU.CTS.Tests.Features.InitialCustomerStatus
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3415:Assertion arguments should be passed in the correct order", Justification = "<Pending>")]
    public class CustomerStatusServicesTests
    {
        private readonly ICustomerStatusDataAccess _dataAccess;
        private readonly ICustomerStatusServices _service;

        public CustomerStatusServicesTests()
        {
            _dataAccess = Substitute.For<ICustomerStatusDataAccess>();
            _service = new CustomerStatusServices(_dataAccess);
        }

        [Fact]
        public void GetCustIds_WhenSuccess()
        {
            // Arrange
            var lastCustIdField = TestExtension.GetPrivateFieldInfo(typeof(CustomerStatusServices), PrivateFieldName.LastCustId);
            var custIdsToStringField = TestExtension.GetPrivateFieldInfo(typeof(CustomerStatusServices), PrivateFieldName.CustIdsToString);

            // Act
            _dataAccess.GetCustIds(Arg.Any<int>()).Returns(new List<long> { 1, 2, 3, 4 });

            _service.GetCustIds(999);

            var lastCustId = lastCustIdField.GetValue(_service);
            var custIdsToString = custIdsToStringField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).GetCustIds(Arg.Any<int>());

            Assert.Equal((long)lastCustId, 1);
            Assert.Equal("1,2,3,4", custIdsToString);
        }

        [Fact]
        public void GetCustomerFromMainDBForUpdate_WhenSuccess()
        {
            // Arrange
            var custIdsToStringField = TestExtension.GetPrivateFieldInfo(typeof(CustomerStatusServices), PrivateFieldName.CustIdsToString);

            var custStatusInfoField = TestExtension.GetPrivateFieldInfo(typeof(CustomerStatusServices), PrivateFieldName.CustStatusInfo);

            // Act
            _dataAccess.GetCustomerFromMainDBForUpdate(Arg.Any<string>(), Arg.Any<UpdateCustomer>()).Returns(new List<CustomerMainDBModel>());

            custIdsToStringField.SetValue(_service, "1,2,3,4");

            _service.GetCustomerFromMainDBForUpdate(UpdateCustomer.ForNewCustomer);

            var result = custStatusInfoField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).GetCustomerFromMainDBForUpdate(Arg.Any<string>(), Arg.Any<UpdateCustomer>());

            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateCustomerStatus_WhenSucess()
        {
            // Arrange
            var lastCustIdField = TestExtension.GetPrivateFieldInfo(typeof(CustomerStatusServices), PrivateFieldName.LastCustId);

            var custStatusInfoField = TestExtension.GetPrivateFieldInfo(typeof(CustomerStatusServices), PrivateFieldName.CustStatusInfo);

            // Act
            lastCustIdField.SetValue(_service, (long)999);

            custStatusInfoField.SetValue(_service, new List<CustomerMainDBModel> { new CustomerMainDBModel { CustSubID = 999 } });

            _service.UpdateCustomerStatus();

            // Assert
            _dataAccess.Received(1).UpdateCustomerStatus(Arg.Any<string>(), Arg.Any<long>());
        }
    }
}
