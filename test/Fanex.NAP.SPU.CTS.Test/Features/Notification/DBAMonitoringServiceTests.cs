using Fanex.NAP.SPU.CTS.Features.Notification;
using Fanex.NAP.SPU.CTS.Features.Notification.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.CTS.Tests.Features.Notification
{
    public class DBAMonitoringServiceTests
    {
        private readonly IDBAMonitoringDataAccess _dataAccess;
        private readonly IDBAMonitoringService _service;

        public DBAMonitoringServiceTests()
        {
            _dataAccess = Substitute.For<IDBAMonitoringDataAccess>();
            _service = new DBAMonitoringService(_dataAccess);
        }

        [Fact]
        public void ScanDataMonitoring_WhenSuccess()
        {
            // Arrange
            var data = new List<DBAMonitoringEntity>();

            // Act
            _dataAccess.ScanDataMonitoring().Returns(data);

            var result = _service.ScanDataMonitoring();

            // Assert
            _dataAccess.Received(1).ScanDataMonitoring();

            Assert.Equal(data, result);
        }

        [Fact]
        public void ScanDataMonitoring_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.ScanDataMonitoring().Throws<Exception>();

            Action action = () => _service.ScanDataMonitoring();

            // Assert
            _dataAccess.Received(0).ScanDataMonitoring();

            Assert.Throws<Exception>(action);
        }
    }
}
