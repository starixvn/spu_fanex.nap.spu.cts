namespace Fanex.NAP.SPU.CTS.Tests.Features.Transform
{
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice;
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice.Entities;
    using FluentAssertions;
    using NSubstitute;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class AssociationByDeviceUnitTest
    {
        private readonly IAssociationByDeviceService associationByDeviceService;
        private readonly IAssociationByDeviceDataAccess dataAccess;

        public AssociationByDeviceUnitTest()
        {
            dataAccess = Substitute.For<IAssociationByDeviceDataAccess>();
            associationByDeviceService = new AssociationByDeviceService(dataAccess);
        }

        [Fact]
        public void GetRawAssociation_Success_ReturnAssociationList()
        {
            var rawAssociationEntity = new List<RawAssociationEntity> {
                new RawAssociationEntity
                {
                   AccountId = 1,
                   AssociationId = 1,
                   BatchId = 1,
                   CreatedTime = DateTime.Now,
                   DeviceId = 1,
                   SubscriberId = 1
                }, 
                new RawAssociationEntity
                {
                   AccountId = 2,
                   AssociationId = 2,
                   BatchId = 2,
                   CreatedTime = DateTime.Now,
                   DeviceId = 2,
                   SubscriberId = 2
                },
                 new RawAssociationEntity
                {
                   AccountId = 3,
                   AssociationId = 3,
                   BatchId = 3,
                   CreatedTime = DateTime.Now,
                   DeviceId = 3,
                   SubscriberId = 3
                }
            };

            dataAccess.GetRawAssociation(100, 2).Returns(rawAssociationEntity);

            var result = associationByDeviceService.GetRawAssociation(100, 2);

            result.Should().BeEquivalentTo(rawAssociationEntity);
        }

        [Fact]
        public void GetRawAssociation_Success_ReturnEmpty()
        {          

            dataAccess.GetRawAssociation(100, 2).Returns(new List<RawAssociationEntity>());

            var result = associationByDeviceService.GetRawAssociation(100, 2);

            result.Should().BeEmpty();
        }
    }
}