using Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.CTS.Tests.Features.ArchiveData
{
    public class ArchiveHistoryClassificationBySportServicesTest
    {
        private readonly IArchiveHistoryClassificationBySportDataAccess _dataAccess;
        private readonly IArchiveHistoryClassificationBySportServices _service;

        public ArchiveHistoryClassificationBySportServicesTest()
        {
            _dataAccess = Substitute.For<IArchiveHistoryClassificationBySportDataAccess>();
            _service = new ArchiveHistoryClassificationBySportServices(_dataAccess);
        }

        [Fact]
        public void ArchiveHistoryClassificationBySport_WhenSuccess()
        {
            // Arrange

            // Act
            _dataAccess.ArchiveHistoryClassificationBySport()
                .Returns(new HistoryClassificationBySportArchiveEntity { CustClassificationHistory = new List<HistoryClassificationBySportEntity> { new HistoryClassificationBySportEntity() } });

            _service.ArchiveHistoryClassificationBySport();

            // Assert
            _dataAccess.Received(1).ArchiveHistoryClassificationBySport();
            _dataAccess.Received(1).InsertHistoryClassificationBySportToArchiveDB(Arg.Any<string>());
        }

        [Fact]
        public void ArchiveHistoryClassificationBySport_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.ArchiveHistoryClassificationBySport()
                .Returns(new HistoryClassificationBySportArchiveEntity { CustClassificationHistory = new List<HistoryClassificationBySportEntity> { new HistoryClassificationBySportEntity() } });

            _dataAccess.When(x => x.InsertHistoryClassificationBySportToArchiveDB(Arg.Any<string>())).Throw<Exception>();

            Action action = () => _service.ArchiveHistoryClassificationBySport();

            // Assert
            _dataAccess.Received(0).InsertHistoryClassificationBySportToArchiveDB(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }
    }
}
