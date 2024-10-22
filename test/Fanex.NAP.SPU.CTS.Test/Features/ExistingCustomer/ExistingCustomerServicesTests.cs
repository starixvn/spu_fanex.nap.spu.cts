using Fanex.NAP.SPU.CTS.Features.ExistingCustomer;
using Fanex.NAP.SPU.CTS.Features.ExistingCustomer.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.CTS.Tests.Features.ExistingCustomer
{
    public class ExistingCustomerServicesTests
    {
        private readonly IExistingCustomerDataAccess _dataAccess;
        private readonly IExistingCustomerServices _service;

        public ExistingCustomerServicesTests()
        {
            _dataAccess = Substitute.For<IExistingCustomerDataAccess>();
            _service = new ExistingCustomerServices(_dataAccess);
        }

        [Fact]
        public void CheckCustomerNotExisted_WhenSuccess()
        {
            // Arrange
            var data = new List<CTSCustomerEntity>();

            // Act
            _dataAccess.CheckCustomerNotExisted(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());

            var result = _service.CheckCustomerNotExisted("TEST", DateTime.MinValue, DateTime.MaxValue);

            // Assert
            _dataAccess.Received(1).CheckCustomerNotExisted(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());

            Assert.Equal(data, result);
        }

        [Fact]
        public void CheckCustomerNotExisted_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(x => x.CheckCustomerNotExisted(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())).Throw<Exception>();

            Action result = () => _service.CheckCustomerNotExisted("TEST", DateTime.MinValue, DateTime.MaxValue);

            // Assert
            _dataAccess.Received(0).CheckCustomerNotExisted(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());

            Assert.Throws<Exception>(result);
        }

        [Fact]
        public void GetPlaceBetCustomer_WhenSuccess()
        {
            // Arrange
            var data = new List<CustomerEntity>();

            // Act
            _dataAccess.GetPlaceBetCustomers(Arg.Any<DateTime>(), Arg.Any<DateTime>());

            var result = _service.GetPlaceBetCustomer(DateTime.MinValue, DateTime.MaxValue);

            // Assert
            _dataAccess.Received(1).GetPlaceBetCustomers(Arg.Any<DateTime>(), Arg.Any<DateTime>());

            Assert.Equal(data, result);
        }

        [Fact]
        public void GetPlaceBetCustomer_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(x => x.GetPlaceBetCustomers(Arg.Any<DateTime>(), Arg.Any<DateTime>())).Throw<Exception>();

            Action result = () => _service.GetPlaceBetCustomer(DateTime.MinValue, DateTime.MaxValue);

            // Assert
            _dataAccess.Received(0).GetPlaceBetCustomers(Arg.Any<DateTime>(), Arg.Any<DateTime>());

            Assert.Throws<Exception>(result);
        }
    }
}
