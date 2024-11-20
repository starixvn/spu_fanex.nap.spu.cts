using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
using Fanex.NAP.SPU.CTS.Shares.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.CTS.Tests.Features.AffectedEvidence.PlaceAffectedEvidenceFromQueue
{
    public class AffectedEvidenceFromQueueServiceTests
    {
        private readonly IAffectedEvidenceFromQueueDataAccess _dataAccess;
        private readonly IAffectedEvidenceFromQueueService _service;

        public AffectedEvidenceFromQueueServiceTests()
        {
            _dataAccess = Substitute.For<IAffectedEvidenceFromQueueDataAccess>();
            _service = new AffectedEvidenceFromQueueService(_dataAccess);
        }

        [Fact]
        public void InsertAssociationAffectedEvidenceFromQueue_WhenSuccess()
        {
            // Arrange
            var data = new ResultAffectedEvidenceFromQueueEntity
            {
                CustIds = new List<long> { 1, 2, 3, 4 },
                IsContinue = true
            };

            // Act
            _dataAccess.InsertAssociationAffectedEvidence().Returns(data);
            _dataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

            _service.InsertAssociationAffectedEvidenceFromQueue();

            // Assert
            _dataAccess.Received(1).InsertAssociationAffectedEvidence();
            _dataAccess.Received(1).UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>());
        }

        [Fact]
        public void InsertAssociationAffectedEvidenceFromQueue_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.InsertAssociationAffectedEvidence().Throws<Exception>();

            Action action = () => _service.InsertAssociationAffectedEvidenceFromQueue();

            // Assert
            _dataAccess.Received(0).InsertAssociationAffectedEvidence();
            _dataAccess.Received(0).UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void RemoveAssociationAffectedEvidenceFromQueue_WhenSuccess()
        {
            // Arrange
            var data = new ResultAffectedEvidenceFromQueueEntity
            {
                CustIds = new List<long> { 1, 2, 3, 4 },
                IsContinue = true
            };

            // Act
            _dataAccess.RemoveAssociationAffectedEvidence().Returns(data);
            _dataAccess.UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

            _service.RemoveAssociationAffectedEvidenceFromQueue();

            // Assert
            _dataAccess.Received(1).RemoveAssociationAffectedEvidence();
            _dataAccess.Received(1).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());
        }

        [Fact]
        public void RemoveAssociationAffectedEvidenceFromQueue_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.RemoveAssociationAffectedEvidence().Throws<Exception>();

            Action action = () => _service.RemoveAssociationAffectedEvidenceFromQueue();

            // Assert
            _dataAccess.Received(0).RemoveAssociationAffectedEvidence();
            _dataAccess.Received(0).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }
    }
}
