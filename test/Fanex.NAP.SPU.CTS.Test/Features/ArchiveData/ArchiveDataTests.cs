namespace Fanex.NAP.SPU.CTS.Tests.Features.ArchiveData
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class ArchiveDataTests
    {
        private readonly IArchiveHistoryClassificationDataAccess dataAccess;
        private readonly IArchiveHistoryClassificationServices services;

        public ArchiveDataTests()
        {
            dataAccess = Substitute.For<IArchiveHistoryClassificationDataAccess>();

            services = new ArchiveHistoryClassificationServices(dataAccess);
        }

        [Fact]
        public void ArchiveChangedCustClassificationHistory_WhenSuccess_ReturnNonException()
        {
            var historyData = new HistoryClassificationArchiveEntity
            {
                CustClassificationHistory = new List<HistoryClassificationEntity>
                {
                  new HistoryClassificationEntity
                  {
                      ID = 1,
                      CustID = 1,
                      CTSCustID = 1
                  },
                  new HistoryClassificationEntity
                  {
                      ID = 2,
                      CustID = 2,
                      CTSCustID = 2
                  }
                },
                IsExistData = false
            };

            var historyDataJson = JsonConvert.SerializeObject(historyData.CustClassificationHistory);

            dataAccess.ArchiveHistoryClassification().Returns(historyData);

            dataAccess.InsertHistoryClassificationToArchiveDB(historyDataJson);

            Action action = () => services.ArchiveHistoryClassification();

            action.Should().NotThrow<Exception>();
        }
    }
}