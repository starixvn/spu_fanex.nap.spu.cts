using Fanex.NAP.SPU.CTS.Features.AffectedEvidence;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.CTS.Tests.Features.AffectedEvidence.TranformAffectedEvidence
{
    public class AffectedEvidenceServiceTests
    {
        private readonly IAffectedEvidenceDataAccess _dataAccess;
        private readonly IAffectedEvidenceService _service;

        public AffectedEvidenceServiceTests()
        {
            _dataAccess = Substitute.For<IAffectedEvidenceDataAccess>();
            _service = new AffectedEvidenceService(_dataAccess);
        }

        [Fact]
        public void TransformAssociationAffectedEvidence_WhenSuccess()
        {
            // Arrange
            var data = new List<int> { 1, 2, 3, 4 };

            // Act
            _dataAccess.TransformAssociationAffectedEvidence().Returns(data);

            var result = _service.TransformAssociationAffectedEvidence();

            // Assert
            _dataAccess.Received(1).TransformAssociationAffectedEvidence();

            Assert.Equal(result, data);
        }

        [Fact]
        public void TransformAssociationAffectedEvidence_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.TransformAssociationAffectedEvidence().Throws<Exception>();

            Action action = () => _service.TransformAssociationAffectedEvidence();

            // Assert
            _dataAccess.Received(0).TransformAssociationAffectedEvidence();

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void UpdateBetLimitOfFlaggedCustomer_WhenSuccess()
        {
            // Arrange

            // Act
            _service.UpdateBetLimitOfFlaggedCustomer(new List<int> { 1, 2, 3, 4 });

            // Assert
            _dataAccess.Received(1).UpdateBetLimitOfFlaggedCustomer("1,2,3,4");
        }

        [Fact]
        public void UpdateBetLimitOfFlaggedCustomer_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(x => x.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>())).Throw<Exception>();

            Action action = () => _service.UpdateBetLimitOfFlaggedCustomer(new List<int> { 1, 2, 3, 4 });

            // Assert
            _dataAccess.Received(0).UpdateBetLimitOfFlaggedCustomer("1,2,3,4");

            Assert.Throws<Exception>(action);
        }
    }
}
